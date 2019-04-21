using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using ProjectEntidades;
using ProjecDTO;
using System.Reflection;

namespace ProjectNegocio
{
    public class NegArchivo
    {

        /// <summary>
        /// Metodo que parsea excel y entrega al modelo
        /// </summary>
        /// <param name="rutaDeArchivoExcel"></param>
        /// <returns></returns>
        public int fnIngresaDatosAlModelo(string rutaDeArchivoExcel)
        {
            EntArchivo entArchivo;
            DtoArchivo dtoArchivo;
            DataTable dt = new DataTable();
            dtoArchivo = new DtoArchivo();
            entArchivo = new EntArchivo();
            int nroColumna = 0;

            dt = ConvertExcelToDataTable(rutaDeArchivoExcel);
            bool resp;


            int idArchivo = dtoArchivo.CreaIdArchivo();
            List<EntArchivo> arch = new List<EntArchivo>();

            foreach (DataRow row in dt.Rows)
            {
                if (!row.IsNull("Nro# Lista"))
                {
                    entArchivo = convieteRowEnModelo(row, idArchivo);
                    arch.Add(entArchivo);
                }
                else
                {
                    break;
                }
            }
            nroColumna = arch.Count;
            resp = dtoArchivo.dInsertaRegistrosArchivo(arch);

            return nroColumna;
        }

        private EntArchivo convieteRowEnModelo(DataRow row, int idArchivo)
        {
            EntArchivo entArchivoLocal;
            entArchivoLocal = new EntArchivo();
            entArchivoLocal.Id_Archivo = idArchivo;
            entArchivoLocal.Nro_lista = Convert.ToInt32(row["Nro# Lista"].ToString());
            entArchivoLocal.Fecha_clase = Convert.ToDateTime(row["Fecha Clase"].ToString());
            entArchivoLocal.Fecha_impresion = Convert.ToDateTime(row["Fecha Impresión"].ToString());
            entArchivoLocal.Excepsiones = row["Excepciones"].ToString();
            entArchivoLocal.Rut_docente = row["Rut Docente"].ToString();
            entArchivoLocal.Nombre_docente = row["Nombre Docente"].ToString();
            entArchivoLocal.Mail_docente = row["Mail Docente"].ToString();
            entArchivoLocal.Programa_estudio = row["Programa Estudios"].ToString();
            entArchivoLocal.Codigo_asignatura = row["Código Asignatura"].ToString();
            entArchivoLocal.Asignatura = row["Asignatura"].ToString();
            entArchivoLocal.Seccion = row["Sección"].ToString();
            entArchivoLocal.Sub_seccion = row["Sub Sección"].ToString();
            entArchivoLocal.Modalidad = row["Modalidad"].ToString();
            entArchivoLocal.Jornada = row["Jornada"].ToString();
            entArchivoLocal.Sala = row["Sala"].ToString();
            entArchivoLocal.Sede = row["Sede"].ToString();
            entArchivoLocal.Dia = row["Día"].ToString();
            entArchivoLocal.Modulo_inicio = Convert.ToInt32(row["Módulo inicio"].ToString());
            entArchivoLocal.Modulo_termino = Convert.ToInt32(row["Módulo término"].ToString());
            entArchivoLocal.Asistentes = Convert.ToInt32(row["Asistentes"].ToString());
            entArchivoLocal.Alumnos_totales = Convert.ToInt32(row["Alumnos totales"].ToString());
            entArchivoLocal.Materias_revisadas = Convert.ToInt32(row["Materias revisadas"].ToString());
            string fecha_retiro = Convert.ToString(row["Fecha retiro"].ToString());
            if (!string.IsNullOrEmpty(fecha_retiro.ToString()))
            {
                entArchivoLocal.Fecha_retiro = Convert.ToDateTime(row["Fecha retiro"].ToString());
            }
            else
            {
                entArchivoLocal.Fecha_retiro = new DateTime(1759, 1, 1);
            }
            string fecha_devolucion = Convert.ToString(row["Fecha devolución"].ToString());
            if (!string.IsNullOrEmpty(fecha_devolucion.ToString()))
            {
                entArchivoLocal.Fecha_devolucion = Convert.ToDateTime(row["Fecha devolución"].ToString());
            }
            else
            {
                entArchivoLocal.Fecha_devolucion = new DateTime(1759, 1, 1);
            }

            return entArchivoLocal;
        }


        public static DataTable ConvertExcelToDataTable(string FileName)
        {
            DataTable dtResult = null;
            int totalSheet = 0; //No of sheets on excel file  
            using (OleDbConnection objConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileName + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';"))
            {
                objConn.Open();
                OleDbCommand cmd = new OleDbCommand();
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                DataSet ds = new DataSet();
                DataTable dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string sheetName = string.Empty;
                if (dt != null)
                {
                    var tempDataTable = (from dataRow in dt.AsEnumerable() where !dataRow["TABLE_NAME"].ToString().Contains("FilterDatabase") select dataRow).CopyToDataTable();
                    dt = tempDataTable;
                    totalSheet = dt.Rows.Count;
                    sheetName = dt.Rows[0]["TABLE_NAME"].ToString();
                }
                cmd.Connection = objConn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM [" + sheetName + "]";
                oleda = new OleDbDataAdapter(cmd);
                oleda.Fill(ds, "excelData");
                dtResult = ds.Tables["excelData"];
                objConn.Close();
                return dtResult; //Returning Dattable  
            }
        }
        /// <summary>
        ///Entrega los datos de la entidad archivo a un dashboard.
        /// </summary>
        /// <returns></returns>
        public DataTable fnSeleccionaDatosArchivo()
        {
            DtoArchivo dtoArchivo = new DtoArchivo();
            return dtoArchivo.fncSelecDatosArchivo();
        }

        private DataTable CreaColumnasDT(DataTable copiaDt)
        {
            DataTable copia = new DataTable();

            for (int i = 0; i < copiaDt.Columns.Count; i++)
            {
                //copia.Columns.Add(copiaDt.Columns)
            }
            return copia;
        }
    }
}