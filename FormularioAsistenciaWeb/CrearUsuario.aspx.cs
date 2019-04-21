using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Configuration;
using System.Drawing;
using FormularioAsistenciaWeb.Helper;

namespace FormularioAsistenciaWeb
{

    public partial class CrearUsuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtNombre.Attributes.Add("onkeypress", "javascript:return lettersOnly(event);");
            txtApellido.Attributes.Add("onkeypress", "javascript:return lettersOnly(event);");
            //txtRut.Attributes.Add("onkeypress", "javascript:return onRutBlur(this);");

            if (Session["usuario"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                if (!VerificaAutorizacion.VerAutorizacion(Session["usuario"].ToString(), "crea"))
                {
                    Response.Redirect("PagAdmin.aspx");
                }
            }
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                string rut;
                rut = txtRut.Text;
                rut = rut.Replace(".", "");
                

                if (!VerificaUsuario(rut))
                {
                    var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                    using (SqlConnection sql = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_CreaUsuario", sql))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@UsuarioId", rut);
                            cmd.Parameters.AddWithValue("@Pwd", txtContTemp.Text);
                            cmd.Parameters.AddWithValue("@Tipousuario", tipoUsuario.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                            cmd.Parameters.AddWithValue("@Apellido", txtApellido.Text);
                            cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                            sql.Open();
                            cmd.ExecuteNonQuery();
                            sql.Close();
                            lblmsg.ForeColor = Color.Blue;
                            lblmsg.Text = "Usuario registrado exitosamente";
                            AgregaCreador.AgregaNombreCreador(Session["nombre"].ToString()+" "+ Session["apellido"].ToString());
                            InicializaControles();
                        }
                    }
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Usuario ya está registrado en SINDAS";
                }
            }
            catch (Exception ex)
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "No se registró al usuario" + ex.ToString();
            }          
        }

        

        private bool VerificaUsuario(string rut)
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_VerificaUsuario", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UsuarioId", rut);
                        sql.Open();
                        SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        rdr.Read();
                        string valor = rdr.GetInt32(rdr.GetOrdinal("EXISTE")).ToString();

                        if (valor != "0")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void InicializaControles()
        {
            txtRut.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtEmail.Text = string.Empty;
            tipoUsuario.SelectedIndex = 0;
        }
    }
}

