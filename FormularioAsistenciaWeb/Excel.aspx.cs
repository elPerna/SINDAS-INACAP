using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectNegocio;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using FormularioAsistenciaWeb.Helper;
using ProjectEntidades;
using EntityFramework.BulkInsert.Extensions;
using System.Configuration;

namespace FormularioAsistenciaWeb
{
   public partial class Contact : Page
    {      
        NegArchivo negArchivo;
        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Text = "";
            if (Session["usuario"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                if (!VerificaAutorizacion.VerAutorizacion(Session["usuario"].ToString(), "cargaArchivo"))
                {
                    Response.Redirect("PagAdmin.aspx");
                }
            }
        }

        protected void enviar_Click1(object sender, EventArgs e)
        {        
            negArchivo = new NegArchivo();
            string ExcelContentType = "application/vnd.ms-excel";
            string Excel2010ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            int resp;
            Label1.Text = string.Empty;

            if (validarContenido(cargaexcel, ExcelContentType) || validarContenido(cargaexcel, Excel2010ContentType))
            {
                try
                {
                    cargaexcel.SaveAs(string.Concat(Path.GetTempPath(), cargaexcel.FileName));

                    resp = negArchivo.fnIngresaDatosAlModelo(string.Concat(Path.GetTempPath(), cargaexcel.FileName));

                    if (resp != 0)
                    {                                       
                         int id = VerificaIdArchivo.VerificaIdExcel();
                         Label1.ForeColor = Color.Green;
                         Label1.Text = "Archivo ingresado correctamente en la base de datos bajo el Nro: " + id;
                         Session["idArchivo"] = null;
                         Session["idArchivo"] = id;

                        RegistraDatos(resp, id, Session["nombre"].ToString()+" "+ Session["apellido"].ToString());
                    }                  
                }
                catch (Exception ex)
                {
                    RequiredFieldValidator1.ErrorMessage = "Error : " + ex.Message.ToString();
                    Label1.ForeColor = Color.Red;
                    Label1.Text = "Archivo no contiene las columnas necesarias o contiene más de una hoja, favor revise e intente nuevamente";
                }
                
            }       
        }

        private void RegistraDatos(int nroColumnas, int id, string usuario)
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_RegistroDatos", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", id);
                        cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        cmd.Parameters.AddWithValue("@nroColumna", nroColumnas);
                        sql.Open();
                        cmd.ExecuteNonQuery();
                        sql.Close();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private bool validarContenido( FileUpload fileUpload , string contenido) {
            if (fileUpload.PostedFile.ContentType  == contenido)
            {
                return true;
            }
            return false;
        }
        //cargar gridview
       
    }
}