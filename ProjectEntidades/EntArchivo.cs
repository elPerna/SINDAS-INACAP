using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEntidades
{
    public class EntArchivo
    {
        private int id_archivo;

        public int Id_Archivo
        {
            get { return id_archivo; }
            set { id_archivo = value; }
        }

        private int nro_lista;

        public int Nro_lista
        {
            get { return nro_lista; }
            set { nro_lista = value; }
        }
        private DateTime fecha_clase;

        public DateTime Fecha_clase
        {
            get { return fecha_clase; }
            set { fecha_clase = value; }
        }
        private DateTime fecha_impresion;

        public DateTime Fecha_impresion
        {
            get { return fecha_impresion; }
            set { fecha_impresion = value; }
        }
        private string excepsiones;

        public string Excepsiones
        {
            get { return excepsiones; }
            set { excepsiones = value; }
        }
        private string rut_docente;

        public string Rut_docente
        {
            get { return rut_docente; }
            set { rut_docente = value; }
        }
        private string nombre_docente;

        public string Nombre_docente
        {
            get { return nombre_docente; }
            set { nombre_docente = value; }
        }
        private string mail_docente;

        public string Mail_docente
        {
            get { return mail_docente; }
            set { mail_docente = value; }
        }
        private string programa_estudio;

        public string Programa_estudio
        {
            get { return programa_estudio; }
            set { programa_estudio = value; }
        }
        private string codigo_asignatura;

        public string Codigo_asignatura
        {
            get { return codigo_asignatura; }
            set { codigo_asignatura = value; }
        }
        private string asignatura;

        public string Asignatura
        {
            get { return asignatura; }
            set { asignatura = value; }
        }
        private string seccion;

        public string Seccion
        {
            get { return seccion; }
            set { seccion = value; }
        }
        private string sub_seccion;

        public string Sub_seccion
        {
            get { return sub_seccion; }
            set { sub_seccion = value; }
        }
        private string modalidad;

        public string Modalidad
        {
            get { return modalidad; }
            set { modalidad = value; }
        }
        private string jornada;

        public string Jornada
        {
            get { return jornada; }
            set { jornada = value; }
        }
        private string sala;

        public string Sala
        {
            get { return sala; }
            set { sala = value; }
        }
        private string sede;

        public string Sede
        {
            get { return sede; }
            set { sede = value; }
        }
        private string dia;

        public string Dia
        {
            get { return dia; }
            set { dia = value; }
        }
        private int modulo_inicio;

        public int Modulo_inicio
        {
            get { return modulo_inicio; }
            set { modulo_inicio = value; }
        }
        private int modulo_termino;

        public int Modulo_termino
        {
            get { return modulo_termino; }
            set { modulo_termino = value; }
        }
        private int asistentes;

        public int Asistentes
        {
            get { return asistentes; }
            set { asistentes = value; }
        }
        private int alumnos_totales;

        public int Alumnos_totales
        {
            get { return alumnos_totales; }
            set { alumnos_totales = value; }
        }
        private int materias_revisadas;

        public int Materias_revisadas
        {
            get { return materias_revisadas; }
            set { materias_revisadas = value; }
        }
        private DateTime fecha_retiro;

        public DateTime Fecha_retiro
        {
            get { return fecha_retiro; }
            set { fecha_retiro = value; }
        }
        private DateTime fecha_devolucion;

        public DateTime Fecha_devolucion
        {
            get { return fecha_devolucion; }
            set { fecha_devolucion = value; }
        }
        private string nombre_archivo;

        public string Nombre_archivo
        {
            get { return nombre_archivo; }
            set { nombre_archivo = value; }
        }
    }
}
