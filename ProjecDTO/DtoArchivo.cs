using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.Threading.Tasks;
using ProjectEntidades;
using OOL.AccesoDatos.Plataforma;
using refDatos = OOL.AccesoDatos;
using System.IO;
using System.Xml.Serialization;
using System.Data.SqlClient;

namespace ProjecDTO
{
    /// <summary>
    /// Clase de abstraccion de datos correspondiente al archivo subido.
    /// </summary>
    public class DtoArchivo
    {
        public string strConexion = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

        /// <summary>
        /// Inserta datos recopilados en excel.
        /// </summary>
        /// <param name="entArchivo"></param>
        /// <returns></returns>
        public bool dInsertaRegistrosArchivo(List<EntArchivo> entArchivo)
        {
            try
            {
                string xml = SerializeObject<List<EntArchivo>>(entArchivo);

                using (SqlConnection sql = new SqlConnection(strConexion))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_bulkInsertArchivo", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UsersXml", xml);
                        sql.Open();
                        cmd.CommandTimeout = 300;
                        cmd.ExecuteNonQuery();
                        sql.Close();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
        /// <summary>
        /// Selecciona datos para dashboard.
        /// </summary>
        /// <returns></returns>
        public DataTable fncSelecDatosArchivo()
        {
            DataTable _dtRespuesta = new DataTable();
            SqlServer.StoredProcedure _objData = new SqlServer.StoredProcedure("ASIS_s_archivo", strConexion);
            try
            {
                _dtRespuesta = _objData.SpExecuteQuery("Mensajeria").Tables[0];
                return _dtRespuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _objData = null;
                _dtRespuesta = null;
            }
        }
        /// <summary>
        /// Valida que el numero de lista no exista en la base de datos.
        /// </summary>
        /// <param name="numeroLista"></param>
        /// <returns></returns>
        public bool validaNumeroDeLista(int numeroLista)
        {
            DataTable _dtRespuesta = new DataTable();
            SqlServer.StoredProcedure _objData = new SqlServer.StoredProcedure("ASIS_s_archivo_lista", strConexion);

            try
            {
                _objData.AgregarParametro("nro_lista", numeroLista, (int)refDatos.Comun.DireccionParamSP.Input);
                _dtRespuesta = _objData.SpExecuteQuery("Mensajeria").Tables[0];
                if (Convert.ToInt32(_dtRespuesta.Rows[0][0].ToString()) != 0)
                {
                    return false;
                }
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _objData = null;
                _dtRespuesta = null;
            }
        }

        /// <summary>
        /// Crea el Id del archivo en la base de datos.
        /// </summary>
        /// <returns></returns>
        public int CreaIdArchivo()
        {
            DataTable _dtRespuesta = new DataTable();
            SqlServer.StoredProcedure _objData = new SqlServer.StoredProcedure("ASIS_s_CreaIdArchivo", strConexion);
            int id=0;

            try
            {
                //_objData.AgregarParametro("nro_lista", numeroLista, (int)refDatos.Comun.DireccionParamSP.Input);
                _dtRespuesta = _objData.SpExecuteQuery("Mensajeria").Tables[0];

                if (_dtRespuesta.Rows.Count > 0)
                {
                    id = Convert.ToInt32(_dtRespuesta.Rows[0][0]) + 1;
                }
                else
                {
                    id = 1;
                }

                return id;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _objData = null;
                _dtRespuesta = null;
            }
        }


        public string SerializeObject<T>(T Obj)
        {
            string strxml = string.Empty;

            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                xs.Serialize(sw, Obj);
                strxml = sw.ToString();
            }
            return strxml;

        }
    }

}
