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
    public partial class CambioContraseña : System.Web.UI.Page
    {
       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                if (!VerificaAutorizacion.VerAutorizacion(Session["usuario"].ToString(), "cambContrasena"))
                {
                    Response.Redirect("PagAdmin.aspx");
                }
            }
        }

        protected void btnCambiaPass_Click(object sender, EventArgs e)
        {
           if( ValidaUsuario(txtRut.Text , txtContActual.Text))
            {
                string rut;
                rut = txtRut.Text;
                rut = rut.Replace(".", "");
                try
                {
                    var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                    using (SqlConnection sql = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_CambiaPass", sql))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@rut", rut);
                            cmd.Parameters.AddWithValue("@pass", txtContActual.Text);
                            cmd.Parameters.AddWithValue("@nuevoPass", txtContNueva.Text);
                            sql.Open();
                            cmd.ExecuteNonQuery();
                            sql.Close();
                            AgregaCreador.AgregaNombreCreador(Session["nombre"].ToString() + " " + Session["apellido"].ToString());
                            txtRut.Text = string.Empty;
                            lblmsg.ForeColor = Color.Blue;
                            lblmsg.Text = "La contraseña ha sido cambiada exitosamente";
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            else
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Cambio de contraseña no se realizó, datos incorrectos";
            }
            
        }

        private bool ValidaUsuario(string rut, string pass)
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_IniciaSession", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@rut", rut);
                        cmd.Parameters.AddWithValue("@pass", pass);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        if (dt.Rows.Count > 0)
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