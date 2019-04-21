using FormularioAsistenciaWeb.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FormularioAsistenciaWeb
{
    public partial class EliminarUsuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            frmEditar.Visible = false;
            if (Session["usuario"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                if (!VerificaAutorizacion.VerAutorizacion(Session["usuario"].ToString(), "elimina"))
                {
                    Response.Redirect("PagAdmin.aspx");
                }
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                string rut;
                rut = txtRut.Text;
                rut = rut.Replace(".", "");

                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_EliminaUsuario", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UsuarioId", rut);
                        sql.Open();
                        cmd.ExecuteNonQuery();
                        sql.Close();
                        lblmsg.Visible = true;
                        lblmsg.ForeColor = Color.Blue;
                        lblmsg.Text = "Usuario eliminado exitosamente";
                        AgregaCreador.AgregaNombreCreador(Session["nombre"].ToString() + " " + Session["apellido"].ToString());
                        frmEditar.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblmsg.Visible = true;
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "No se eliminó el usuario";
            }
        }

        protected void btnBusqueda_Click(object sender, EventArgs e)
        {
            try
            {
                int rolIndex = 0;
                string rut;
                rut = txtRut.Text;
                rut = rut.Replace(".", "");


                if (VerificaUsuario(rut))
                {
                    var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                    using (SqlConnection sql = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_DatosUsuario", sql))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@rut", rut);
                            DataTable dt = new DataTable();
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            sql.Open();
                            da.Fill(dt);
                            sql.Close();

                            if (dt.Rows.Count > 0)
                            {
                                lblmsg.Visible = false;
                                rolIndex = ObtieneComboRol(dt.Rows[0]["TipoUsuario"].ToString());
                                tipoUsuario.SelectedIndex = rolIndex;
                                txtNombre.Text = dt.Rows[0]["Nombre"].ToString();
                                txtApellido.Text = dt.Rows[0]["Apellido"].ToString();
                                txtRutFrm.Text = dt.Rows[0]["UsuarioId"].ToString();
                                txtEmail.Text = dt.Rows[0]["Email"].ToString();
                                frmEditar.Visible = true;
                            }
                        }
                    }
                }
                else
                {
                    frmEditar.Visible = false;
                    lblmsg.Visible = true;
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Usuario no está registrado en SINDAS";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private int ObtieneComboRol(string rol)
        {
            int i = 0;
            foreach (var item in tipoUsuario.Items)
            {
                if (item.ToString().Equals(rol))
                {
                    return i;
                }
                else
                {
                    i++;
                }
            }

            return i;
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
    }
}