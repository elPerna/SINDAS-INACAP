using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEntidades
{
    [Serializable]
    public class EntExcel
    {
        private int id_archivo;
        private string nombre_archivo;
        private int cantidad_registros;
        private DateTime fecha_ingreso;

        public int Id_archivo
        {
            get { return id_archivo; }
            set { id_archivo = value; }
        }
        
        public string Nombre_archivo
        {
            get { return nombre_archivo; }
            set { nombre_archivo = value; }
        }
        
        public int Cantidad_registros
        {
            get { return cantidad_registros; }
            set { cantidad_registros = value; }
        }

        public DateTime Fecha_ingreso
        {
            get { return fecha_ingreso; }
            set { fecha_ingreso = value; }
        }        
        
    }
}
