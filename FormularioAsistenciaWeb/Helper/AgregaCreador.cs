using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FormularioAsistenciaWeb.Helper
{
    public class AgregaCreador
    {
        public static void AgregaNombreCreador(string nombreCreador)
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_AgregaCreador", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@NombreUsuario", nombreCreador);
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
    }
}