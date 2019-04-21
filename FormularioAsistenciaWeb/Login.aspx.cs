using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace FormularioAsistenciaWeb
{
    public partial class Login : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
        
        }

        protected void btnIniciaSesion_Click(object sender, EventArgs e)
        {
            string rut;

            rut = txtRut.Text;
            rut = rut.Replace(".", "");

            if (IniciaSesion(rut, txtPwd.Text))
            {
                Response.Redirect("PagAdmin.aspx");
                return;
            }
            else
            {
                Response.Write("<script>alert('Datos inválidos');</script>");
                txtRut.Text = string.Empty;
            }
        }

        private bool IniciaSesion(string rut, string pass)
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
                            Session["usuario"] = dt.Rows[0]["TipoUsuario"];
                            Session["nombre"] = dt.Rows[0]["Nombre"];
                            Session["apellido"] = dt.Rows[0]["Apellido"];
                            Session["email"] = dt.Rows[0]["Email"];
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