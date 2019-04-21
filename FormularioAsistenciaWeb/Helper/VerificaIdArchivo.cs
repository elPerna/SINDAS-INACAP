using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FormularioAsistenciaWeb.Helper
{
    public class VerificaIdArchivo
    {
        public static int VerificaIdExcel()
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("ASIS_s_CreaIdArchivo", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        return Convert.ToInt32(dt.Rows[0]["id_archivo"]);
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