
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

namespace ProjecDTO
{
    /// <summary>
    /// Clase que Gestiona la interfaz de datos de la entidad archivo excel.
    /// </summary>
    public class DtoExcel
    {
        public string strConexion = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

        /// <summary>
        /// Metodo que inserta en tabla archivo.
        /// </summary>
        /// <param name="EntExcel"></param>
        /// <returns></returns>
        public bool dInsertaDatosArchivo(ProjectEntidades.EntExcel EntExcel)
        {
            bool bResultado;
            SqlServer.StoredProcedure _objData = new SqlServer.StoredProcedure("ASIS_i_excel", strConexion);
            if (buscasiexisteArchivo(EntExcel.Nombre_archivo.ToString())==1)
            {
                
                _objData.AgregarParametro("nombre_archivo", EntExcel.Nombre_archivo, (int)refDatos.Comun.DireccionParamSP.Input);
                _objData.AgregarParametro("cantidad_registros", EntExcel.Cantidad_registros, (int)refDatos.Comun.DireccionParamSP.Input);                

                try
                {
                    bResultado = _objData.SpExecuteNonQuery();
                    return bResultado;
                }
                catch (Exception ex)
                {
                    return false;
                    throw ex;
                }
                finally
                {
                    _objData = null;

                }
            }
            else {
                return false;
            }
            
        }

        /// <summary>
        /// Metodo que valida si existe el archivo en bd.
        /// </summary>
        /// <param name="nombre_archivo"></param>
        /// <returns></returns>
        private int buscasiexisteArchivo(string nombre_archivo) {            
            DataTable _dtRespuesta = new DataTable();
            SqlServer.StoredProcedure _objData = new SqlServer.StoredProcedure("ASIS_s_existe", strConexion);

            try {
                _objData.AgregarParametro("nombre_archivo", nombre_archivo, (int)refDatos.Comun.DireccionParamSP.Input);
                _dtRespuesta = _objData.SpExecuteQuery("Mensajeria").Tables[0];

                return Convert.ToInt32(_dtRespuesta.Rows[0][0].ToString());               
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

    }
}
