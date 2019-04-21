using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using FormularioAsistenciaWeb.Helper;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace FormularioAsistenciaWeb
{
    public partial class DashBoard : System.Web.UI.Page
    {    
        protected void Page_Load(object sender, EventArgs e)
        {
            divGrafico.Visible = false;
            divBtnPDF.Visible = false;           
            if (!IsPostBack)
            {
                LlenaComboExterno();
                LimpiaCheck();
            }

            if (Session["usuario"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                if (!VerificaAutorizacion.VerAutorizacion(Session["usuario"].ToString(), "verDash"))
                {
                    Response.Redirect("PagAdmin.aspx");
                }
            }
        }

        #region Clase realizada 
        private void GrfClsRealizadaPorDocente(string idArchivo)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;
      
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DB_ClsRealizadaPorDocente", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos.Value = string.Empty;
                        txtIndex.Value = string.Empty;
                    }
                }

                if(cantGraficos > 0)
                {
                    if (cantGraficos.ToString().Equals("1"))
                    {
                        divBtnGrafica.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica.Visible = true;
                    }

                    txtcantGraficos.Value = cantGraficos.ToString();
                    txtIndex.Value = 1.ToString();

                    for (int i = 1; i < cantGraficos + 1; i++)
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_DB_T4_ClsRealizadaPorDocente_Paginado", sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", i);
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    cant.Add(rdr.GetInt32(0));
                                    docente.Add(rdr.GetString(1));
                                }
                                rdr.Close();
                                sql.Close();
        
                                ChartBarra.Series[0].Points.DataBindXY(docente, cant);
                                CargaImagenes(ChartBarra, i);
                                divGrafico.Visible = true;
                                lblmsg.Text = string.Empty;
                                divBtnPDF.Visible = true;
                                btnAnterior.Enabled = false;        
                            }
                        }
                    }
                    //Setea el primer grafico
                    txtSp.Value = "sp_DB_T4_ClsRealizadaPorDocente_Paginado";
                    SeteaGrafico(idArchivo, Convert.ToInt32(txtIndex.Value), "sp_DB_T4_ClsRealizadaPorDocente_Paginado_Top");
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico";
                    divGrafico.Visible = false;
                    divBtnPDF.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfClsRealizadaPorFecha(string idArchivo)
        {
            ArrayList cant = new ArrayList();
            ArrayList fecha = new ArrayList();
            decimal cantGraficos;

            try
            {             
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DB_ClsRealizadaPorFecha", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos.Value = string.Empty;
                        txtIndex.Value = string.Empty;                  
                    }
                }

                if (cantGraficos > 0)
                {
                    if (cantGraficos.ToString().Equals("1"))
                    {
                        divBtnGrafica.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica.Visible = true;
                    }

                    txtcantGraficos.Value = cantGraficos.ToString();
                    txtIndex.Value = 1.ToString();

                    for (int i = 1; i < cantGraficos + 1; i++)
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_DB_T1_ClsRealizadaPorFecha_Paginado", sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", i);
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                fecha.RemoveRange(0, fecha.Count);
                                while (rdr.Read())
                                {
                                    cant.Add(rdr.GetInt32(0));
                                    fecha.Add(rdr.GetDateTime(1));
                                }
                                rdr.Close();
                                sql.Close();

                                ChartBarra.Series[0].Points.DataBindXY(fecha, cant);
                                CargaImagenes(ChartBarra, i);
                                divGrafico.Visible = true;
                                lblmsg.Text = string.Empty;
                                divBtnPDF.Visible = true;
                                btnAnterior.Enabled = false;
                            }
                        }
                    }
                    //Setea el primer grafico
                    txtSp.Value = "sp_DB_T1_ClsRealizadaPorFecha_Paginado";
                    SeteaGrafico(idArchivo, Convert.ToInt32(txtIndex.Value), "sp_DB_T1_ClsRealizadaPorFecha_Paginado_Top");
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico";
                    divGrafico.Visible = false;
                    divBtnPDF.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfClsRealizadaPorAsignatura(string idArchivo)
        {
            ArrayList cant = new ArrayList();
            ArrayList asignatura = new ArrayList();
            decimal cantGraficos;

            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DB_ClsRealizadaPorAsignatura", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos.Value = string.Empty;
                        txtIndex.Value = string.Empty;                             
                    }
                }

                if (cantGraficos > 0)
                {
                    if (cantGraficos.ToString().Equals("1"))
                    {
                        divBtnGrafica.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica.Visible = true;
                    }

                    txtcantGraficos.Value = cantGraficos.ToString();
                    txtIndex.Value = 1.ToString();

                    for (int i = 1; i < cantGraficos + 1; i++)
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_DB_T4_ClsRealizadaPorAsignatura_Paginado", sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", i);
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                asignatura.RemoveRange(0, asignatura.Count);
                                while (rdr.Read())
                                {
                                    cant.Add(rdr.GetInt32(0));
                                    asignatura.Add(rdr.GetString(1));
                                }
                                rdr.Close();
                                sql.Close();

                                ChartBarra.Series[0].Points.DataBindXY(asignatura, cant);
                                CargaImagenes(ChartBarra, i);
                                divGrafico.Visible = true;
                                lblmsg.Text = string.Empty;
                                divBtnPDF.Visible = true;
                                btnAnterior.Enabled = false;
                            }
                        }
                    }
                    //Setea el primer grafico
                    txtSp.Value = "sp_DB_T4_ClsRealizadaPorAsignatura_Paginado";
                    SeteaGrafico(idArchivo, Convert.ToInt32(txtIndex.Value), "sp_DB_T4_ClsRealizadaPorAsignatura_Paginado_Top");
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico";
                    divGrafico.Visible = false;
                    divBtnPDF.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfClsRealizadaPorJornada(string idArchivo)
        {
            ArrayList cant = new ArrayList();
            ArrayList asignatura = new ArrayList();
            decimal cantGraficos;

            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DB_ClsRealizadaPorJornada", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos.Value = string.Empty;
                        txtIndex.Value = string.Empty;       
                    }
                }

                if (cantGraficos > 0)
                {
                    if (cantGraficos.ToString().Equals("1"))
                    {
                        divBtnGrafica.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica.Visible = true;
                    }

                    txtcantGraficos.Value = cantGraficos.ToString();
                    txtIndex.Value = 1.ToString();

                    for (int i = 1; i < cantGraficos + 1; i++)
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_DB_T4_ClsRealizadaPorJornada_Paginado", sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", i);
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                asignatura.RemoveRange(0, asignatura.Count);
                                while (rdr.Read())
                                {
                                    cant.Add(rdr.GetInt32(0));
                                    asignatura.Add(rdr.GetString(1));
                                }
                                rdr.Close();
                                sql.Close();

                                ChartBarra.Series[0].Points.DataBindXY(asignatura, cant);
                                CargaImagenes(ChartBarra, i);
                                divGrafico.Visible = true;
                                lblmsg.Text = string.Empty;
                                divBtnPDF.Visible = true;
                                btnAnterior.Enabled = false;
                            }
                        }
                    }
                    //Setea el primer grafico
                    txtSp.Value = "sp_DB_T4_ClsRealizadaPorJornada_Paginado";
                    SeteaGrafico(idArchivo, Convert.ToInt32(txtIndex.Value), "sp_DB_T4_ClsRealizadaPorJornada_Paginado_Top");
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico";
                    divGrafico.Visible = false;
                    divBtnPDF.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Clase no realizada 
        private void GrfClsNoRealizadaPorDocente(string idArchivo)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;

            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DB_ClsNoRealizadaPorDocente", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos.Value = string.Empty;
                        txtIndex.Value = string.Empty;   
                    }
                }

                if (cantGraficos > 0)
                {
                    if (cantGraficos.ToString().Equals("1"))
                    {
                        divBtnGrafica.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica.Visible = true;
                    }

                    txtcantGraficos.Value = cantGraficos.ToString();
                    txtIndex.Value = 1.ToString();

                    for (int i = 1; i < cantGraficos + 1; i++)
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_DB_T4_ClsNoRealizadaPorDocente_Paginado", sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", i);
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    cant.Add(rdr.GetInt32(0));
                                    docente.Add(rdr.GetString(1));
                                }
                                rdr.Close();
                                sql.Close();

                                ChartBarra.Series[0].Points.DataBindXY(docente, cant);
                                CargaImagenes(ChartBarra, i);
                                divGrafico.Visible = true;
                                lblmsg.Text = string.Empty;
                                divBtnPDF.Visible = true;
                                btnAnterior.Enabled = false;
                            }
                        }
                    }
                    //Setea el primer grafico
                    txtSp.Value = "sp_DB_T4_ClsNoRealizadaPorDocente_Paginado";
                    SeteaGrafico(idArchivo, Convert.ToInt32(txtIndex.Value), "sp_DB_T4_ClsNoRealizadaPorDocente_Paginado_Top");
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico";
                    divGrafico.Visible = false;
                    divBtnPDF.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfClsNoRealizadaPorFecha(string idArchivo)
        {
            ArrayList cant = new ArrayList();
            ArrayList fecha = new ArrayList();
            decimal cantGraficos;

            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DB_ClsNoRealizadaPorFecha", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos.Value = string.Empty;
                        txtIndex.Value = string.Empty;      
                    }
                }

                if (cantGraficos > 0)
                {
                    if (cantGraficos.ToString().Equals("1"))
                    {
                        divBtnGrafica.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica.Visible = true;
                    }

                    txtcantGraficos.Value = cantGraficos.ToString();
                    txtIndex.Value = 1.ToString();

                    for (int i = 1; i < cantGraficos + 1; i++)
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_DB_T1_ClsNoRealizadaPorFecha_Paginado", sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", i);
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                fecha.RemoveRange(0, fecha.Count);
                                while (rdr.Read())
                                {
                                    cant.Add(rdr.GetInt32(0));
                                    fecha.Add(rdr.GetDateTime(1));
                                }
                                rdr.Close();
                                sql.Close();

                                ChartBarra.Series[0].Points.DataBindXY(fecha, cant);
                                CargaImagenes(ChartBarra, i);
                                divGrafico.Visible = true;
                                lblmsg.Text = string.Empty;
                                divBtnPDF.Visible = true;
                                btnAnterior.Enabled = false;
                            }
                        }
                    }
                    //Setea el primer grafico
                    txtSp.Value = "sp_DB_T1_ClsNoRealizadaPorFecha_Paginado";
                    SeteaGrafico(idArchivo, Convert.ToInt32(txtIndex.Value), "sp_DB_T1_ClsNoRealizadaPorFecha_Paginado_Top");
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico";
                    divGrafico.Visible = false;
                    divBtnPDF.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfClsNoRealizadaPorAsignatura(string idArchivo)
        {
            ArrayList cant = new ArrayList();
            ArrayList asignatura = new ArrayList();
            decimal cantGraficos;

            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DB_ClsNoRealizadaPorAsignatura", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos.Value = string.Empty;
                        txtIndex.Value = string.Empty;
                    }
                }

                if (cantGraficos > 0)
                {
                    if (cantGraficos.ToString().Equals("1"))
                    {
                        divBtnGrafica.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica.Visible = true;
                    }

                    txtcantGraficos.Value = cantGraficos.ToString();
                    txtIndex.Value = 1.ToString();

                    for (int i = 1; i < cantGraficos + 1; i++)
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_DB_T4_ClsNoRealizadaPorAsignatura_Paginado", sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", i);
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                asignatura.RemoveRange(0, asignatura.Count);
                                while (rdr.Read())
                                {
                                    cant.Add(rdr.GetInt32(0));
                                    asignatura.Add(rdr.GetString(1));
                                }
                                rdr.Close();
                                sql.Close();

                                ChartBarra.Series[0].Points.DataBindXY(asignatura, cant);
                                CargaImagenes(ChartBarra, i);
                                divGrafico.Visible = true;
                                lblmsg.Text = string.Empty;
                                divBtnPDF.Visible = true;
                                btnAnterior.Enabled = false;
                            }
                        }
                    }
                    //Setea el primer grafico
                    txtSp.Value = "sp_DB_T4_ClsNoRealizadaPorAsignatura_Paginado";
                    SeteaGrafico(idArchivo, Convert.ToInt32(txtIndex.Value), "sp_DB_T4_ClsNoRealizadaPorAsignatura_Paginado_Top");
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico";
                    divGrafico.Visible = false;
                    divBtnPDF.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfClsNoRealizadaPorJornada(string idArchivo)
        {
            ArrayList cant = new ArrayList();
            ArrayList asignatura = new ArrayList();
            decimal cantGraficos;

            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DB_ClsNoRealizadaPorJornada", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos.Value = string.Empty;
                        txtIndex.Value = string.Empty;                        
                    }
                }

                if (cantGraficos > 0)
                {
                    if (cantGraficos.ToString().Equals("1"))
                    {
                        divBtnGrafica.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica.Visible = true;
                    }

                    txtcantGraficos.Value = cantGraficos.ToString();
                    txtIndex.Value = 1.ToString();

                    for (int i = 1; i < cantGraficos + 1; i++)
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_DB_T4_ClsNoRealizadaPorJornada_Paginado", sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", i);
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                asignatura.RemoveRange(0, asignatura.Count);
                                while (rdr.Read())
                                {
                                    cant.Add(rdr.GetInt32(0));
                                    asignatura.Add(rdr.GetString(1));
                                }
                                rdr.Close();
                                sql.Close();

                                ChartBarra.Series[0].Points.DataBindXY(asignatura, cant);
                                CargaImagenes(ChartBarra, i);
                                divGrafico.Visible = true;
                                lblmsg.Text = string.Empty;
                                divBtnPDF.Visible = true;
                                btnAnterior.Enabled = false;
                            }
                        }
                    }
                    //Setea el primer grafico
                    txtSp.Value = "sp_DB_T4_ClsNoRealizadaPorJornada_Paginado";
                    SeteaGrafico(idArchivo, Convert.ToInt32(txtIndex.Value), "sp_DB_T4_ClsNoRealizadaPorJornada_Paginado_Top");
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico";
                    divGrafico.Visible = false;
                    divBtnPDF.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Libro no marcado al devolverlo
        private void GrfLibroNoMarcadoPorDocente(string idArchivo)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;

            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DB_LibroNoMarcadoPorDocente", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos.Value = string.Empty;
                        txtIndex.Value = string.Empty;                                 
                    }
                }

                if (cantGraficos > 0)
                {
                    if (cantGraficos.ToString().Equals("1"))
                    {
                        divBtnGrafica.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica.Visible = true;
                    }

                    txtcantGraficos.Value = cantGraficos.ToString();
                    txtIndex.Value = 1.ToString();

                    for (int i = 1; i < cantGraficos + 1; i++)
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_DB_T4_LibroNoMarcadoPorDocente_Paginado", sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", i);
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    cant.Add(rdr.GetInt32(0));
                                    docente.Add(rdr.GetString(1));
                                }
                                rdr.Close();
                                sql.Close();

                                ChartBarra.Series[0].Points.DataBindXY(docente, cant);
                                CargaImagenes(ChartBarra, i);
                                divGrafico.Visible = true;
                                lblmsg.Text = string.Empty;
                                divBtnPDF.Visible = true;
                                btnAnterior.Enabled = false;
                            }
                        }
                    }
                    //Setea el primer grafico
                    txtSp.Value = "sp_DB_T4_LibroNoMarcadoPorDocente_Paginado";
                    SeteaGrafico(idArchivo, Convert.ToInt32(txtIndex.Value), "sp_DB_T4_LibroNoMarcadoPorDocente_Paginado_Top");
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico";
                    divGrafico.Visible = false;
                    divBtnPDF.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfLibroNoMarcadoPorFecha(string idArchivo)
        {
            ArrayList cant = new ArrayList();
            ArrayList fecha = new ArrayList();
            decimal cantGraficos;

            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DB_LibroNoMarcadoPorFecha", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos.Value = string.Empty;
                        txtIndex.Value = string.Empty;       
                    }
                }

                if (cantGraficos > 0)
                {
                    if (cantGraficos.ToString().Equals("1"))
                    {
                        divBtnGrafica.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica.Visible = true;
                    }

                    txtcantGraficos.Value = cantGraficos.ToString();
                    txtIndex.Value = 1.ToString();

                    for (int i = 1; i < cantGraficos + 1; i++)
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_DB_T1_LibroNoMarcadoPorFecha_Paginado", sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", i);
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                fecha.RemoveRange(0, fecha.Count);
                                while (rdr.Read())
                                {
                                    cant.Add(rdr.GetInt32(0));
                                    fecha.Add(rdr.GetDateTime(1));
                                }
                                rdr.Close();
                                sql.Close();

                                ChartBarra.Series[0].Points.DataBindXY(fecha, cant);
                                CargaImagenes(ChartBarra, i);
                                divGrafico.Visible = true;
                                lblmsg.Text = string.Empty;
                                divBtnPDF.Visible = true;
                                btnAnterior.Enabled = false;
                            }
                        }
                    }
                    //Setea el primer grafico
                    txtSp.Value = "sp_DB_T1_LibroNoMarcadoPorFecha_Paginado";
                    SeteaGrafico(idArchivo, Convert.ToInt32(txtIndex.Value), "sp_DB_T1_LibroNoMarcadoPorFecha_Paginado_Top");
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico";
                    divGrafico.Visible = false;
                    divBtnPDF.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfLibroNoMarcadoPorAsignatura(string idArchivo)
        {
            ArrayList cant = new ArrayList();
            ArrayList asignatura = new ArrayList();
            decimal cantGraficos;
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DB_LibroNoMarcadoPorAsignatura", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos.Value = string.Empty;
                        txtIndex.Value = string.Empty;                     
                    }
                }

                if (cantGraficos > 0)
                {
                    if (cantGraficos.ToString().Equals("1"))
                    {
                        divBtnGrafica.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica.Visible = true;
                    }

                    txtcantGraficos.Value = cantGraficos.ToString();
                    txtIndex.Value = 1.ToString();

                    for (int i = 1; i < cantGraficos + 1; i++)
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_DB_T4_LibroNoMarcadoPorAsignatura_Paginado", sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", i);
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                asignatura.RemoveRange(0, asignatura.Count);
                                while (rdr.Read())
                                {
                                    cant.Add(rdr.GetInt32(0));
                                    asignatura.Add(rdr.GetString(1));
                                }
                                rdr.Close();
                                sql.Close();

                                ChartBarra.Series[0].Points.DataBindXY(asignatura, cant);
                                CargaImagenes(ChartBarra, i);
                                divGrafico.Visible = true;
                                lblmsg.Text = string.Empty;
                                divBtnPDF.Visible = true;
                                btnAnterior.Enabled = false;
                            }
                        }
                    }
                    //Setea el primer grafico
                    txtSp.Value = "sp_DB_T4_LibroNoMarcadoPorAsignatura_Paginado";
                    SeteaGrafico(idArchivo, Convert.ToInt32(txtIndex.Value), "sp_DB_T4_LibroNoMarcadoPorAsignatura_Paginado_Top");
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico";
                    divGrafico.Visible = false;
                    divBtnPDF.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfLibroNoMarcadoPorJornada(string idArchivo)
        {
            ArrayList cant = new ArrayList();
            ArrayList asignatura = new ArrayList();
            decimal cantGraficos;

            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DB_LibroNoMarcadoPorJornada", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos.Value = string.Empty;
                        txtIndex.Value = string.Empty;                      
                    }
                }

                if (cantGraficos > 0)
                {
                    if (cantGraficos.ToString().Equals("1"))
                    {
                        divBtnGrafica.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica.Visible = true;
                    }

                    txtcantGraficos.Value = cantGraficos.ToString();
                    txtIndex.Value = 1.ToString();

                    for (int i = 1; i < cantGraficos + 1; i++)
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_DB_T4_LibroNoMarcadoPorJornada_Paginado", sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", i);
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                asignatura.RemoveRange(0, asignatura.Count);
                                while (rdr.Read())
                                {
                                    cant.Add(rdr.GetInt32(0));
                                    asignatura.Add(rdr.GetString(1));
                                }
                                rdr.Close();
                                sql.Close();

                                ChartBarra.Series[0].Points.DataBindXY(asignatura, cant);
                                CargaImagenes(ChartBarra, i);
                                divGrafico.Visible = true;
                                lblmsg.Text = string.Empty;
                                divBtnPDF.Visible = true;
                                btnAnterior.Enabled = false;
                            }
                        }
                    }
                    //Setea el primer grafico
                    txtSp.Value = "sp_DB_T4_LibroNoMarcadoPorJornada_Paginado";
                    SeteaGrafico(idArchivo, Convert.ToInt32(txtIndex.Value), "sp_DB_T4_LibroNoMarcadoPorJornada_Paginado_Top");
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico";
                    divGrafico.Visible = false;
                    divBtnPDF.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Referencia porcentual de alumnos asistentes
        private void GrfPorcentajealumnosExistentesPorAsignatura(string idArchivo)
        {
            ArrayList asignatura = new ArrayList();
            ArrayList porcentaje = new ArrayList();
            decimal cantGraficos;

            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_PorcentajePorAsignatura", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos.Value = string.Empty;
                        txtIndex.Value = string.Empty;
                    }
                }

                if (cantGraficos > 0)
                {
                    if (cantGraficos.ToString().Equals("1"))
                    {
                        divBtnGrafica.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica.Visible = true;
                    }

                    txtcantGraficos.Value = cantGraficos.ToString();
                    txtIndex.Value = 1.ToString();

                    for (int i = 1; i < cantGraficos + 1; i++)
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_DB_T3_PorcentajePorAsignatura_Paginado", sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", i);
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                porcentaje.RemoveRange(0, porcentaje.Count);
                                asignatura.RemoveRange(0, asignatura.Count);
                                while (rdr.Read())
                                {
                                    porcentaje.Add(rdr.GetDecimal(0));
                                    asignatura.Add(rdr.GetString(1));
                                }
                                rdr.Close();
                                sql.Close();

                                ChartBarra.Series[0].Points.DataBindXY(asignatura, porcentaje);
                                CargaImagenes(ChartBarra, i);
                                divGrafico.Visible = true;
                                lblmsg.Text = string.Empty;
                                divBtnPDF.Visible = true;
                                btnAnterior.Enabled = false;
                            }
                        }
                    }
                    //Setea el primer grafico
                    txtSp.Value = "sp_DB_T3_PorcentajePorAsignatura_Paginado";
                    SeteaGrafico(idArchivo, Convert.ToInt32(txtIndex.Value), "sp_DB_T3_PorcentajePorAsignatura_Paginado_Top");
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico";
                    divGrafico.Visible = false;
                    divBtnPDF.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GrfPorcentajealumnosExistentesPorFecha(string idArchivo)
        {
            ArrayList cant = new ArrayList();
            ArrayList fecha = new ArrayList();
            decimal cantGraficos;

            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_PorcentajePorFecha", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos.Value = string.Empty;
                        txtIndex.Value = string.Empty;
                    }
                }

                if (cantGraficos > 0)
                {
                    if (cantGraficos.ToString().Equals("1"))
                    {
                        divBtnGrafica.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica.Visible = true;
                    }

                    txtcantGraficos.Value = cantGraficos.ToString();
                    txtIndex.Value = 1.ToString();

                    for (int i = 1; i < cantGraficos + 1; i++)
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_DB_T2_PorcentajePorFecha_Paginado", sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", i);
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                fecha.RemoveRange(0, fecha.Count);
                                while (rdr.Read())
                                {                               
                                    cant.Add(rdr.GetDecimal(0));
                                    fecha.Add(rdr.GetDateTime(1));
                                }
                                rdr.Close();
                                sql.Close();

                                ChartBarra.Series[0].Points.DataBindXY(fecha, cant);
                                CargaImagenes(ChartBarra, i);
                                divGrafico.Visible = true;
                                lblmsg.Text = string.Empty;
                                divBtnPDF.Visible = true;
                                btnAnterior.Enabled = false;
                            }
                        }
                    }
                    //Setea el primer grafico
                    txtSp.Value = "sp_DB_T2_PorcentajePorFecha_Paginado";
                    SeteaGrafico(idArchivo, Convert.ToInt32(txtIndex.Value), "sp_DB_T2_PorcentajePorFecha_Paginado_Top");
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico";
                    divGrafico.Visible = false;
                    divBtnPDF.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfPorcentajealumnosExistentesPorJornada(string idArchivo)
        {
            ArrayList cant = new ArrayList();
            ArrayList asignatura = new ArrayList();
            decimal cantGraficos;

            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_PorcentajePorJornada", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos.Value = string.Empty;
                        txtIndex.Value = string.Empty;
                    }
                }

                if (cantGraficos > 0)
                {
                    if (cantGraficos.ToString().Equals("1"))
                    {
                        divBtnGrafica.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica.Visible = true;
                    }

                    txtcantGraficos.Value = cantGraficos.ToString();
                    txtIndex.Value = 1.ToString();

                    for (int i = 1; i < cantGraficos + 1; i++)
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_DB_T3_PorcentajePorJornada_Paginado", sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", i);
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                asignatura.RemoveRange(0, asignatura.Count);
                                while (rdr.Read())
                                {                                 
                                    cant.Add(rdr.GetDecimal(0));
                                    asignatura.Add(rdr.GetString(1));
                                }
                                rdr.Close();
                                sql.Close();

                                ChartBarra.Series[0].Points.DataBindXY(asignatura, cant);
                                CargaImagenes(ChartBarra, i);
                                divGrafico.Visible = true;
                                lblmsg.Text = string.Empty;
                                divBtnPDF.Visible = true;
                                btnAnterior.Enabled = false;
                            }
                        }
                    }
                    //Setea el primer grafico
                    txtSp.Value = "sp_DB_T3_PorcentajePorJornada_Paginado";
                    SeteaGrafico(idArchivo, Convert.ToInt32(txtIndex.Value), "sp_DB_T3_PorcentajePorJornada_Paginado_Top");
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico";
                    divGrafico.Visible = false;
                    divBtnPDF.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private DataTable DatosPorcentaje(string idArchivo, string spPorcentaje)
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(spPorcentaje, sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);                       
                        sql.Close();

                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Atraso en retiro libro de clases

        private void GrfAtrasoRetiroLibroPorDocente(string idArchivo)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;

            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DatosAtrasoLibroClasesPorDocente", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos.Value = string.Empty;
                        txtIndex.Value = string.Empty;
                    }
                }

                if (cantGraficos > 0)
                {
                    if (cantGraficos.ToString().Equals("1"))
                    {
                        divBtnGrafica.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica.Visible = true;
                    }

                    txtcantGraficos.Value = cantGraficos.ToString();
                    txtIndex.Value = 1.ToString();

                    for (int i = 1; i < cantGraficos + 1; i++)
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_DB_T4_AtrasoRetiroLibroPorDocente_Paginado", sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", i);
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    cant.Add(rdr.GetInt32(0));
                                    docente.Add(rdr.GetString(1));
                                }
                                rdr.Close();
                                sql.Close();

                                ChartBarra.Series[0].Points.DataBindXY(docente, cant);
                                CargaImagenes(ChartBarra, i);
                                divGrafico.Visible = true;
                                lblmsg.Text = string.Empty;
                                divBtnPDF.Visible = true;
                                btnAnterior.Enabled = false;
                            }
                        }
                    }
                    //Setea el primer grafico
                    txtSp.Value = "sp_DB_T4_AtrasoRetiroLibroPorDocente_Paginado";
                    SeteaGrafico(idArchivo, Convert.ToInt32(txtIndex.Value), "sp_DB_T4_AtrasoRetiroLibroPorDocente_Paginado_Top");
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico";
                    divGrafico.Visible = false;
                    divBtnPDF.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfAtrasoRetiroLibroPorFecha (string idArchivo)
        {
            ArrayList cant = new ArrayList();
            ArrayList fecha = new ArrayList();
            decimal cantGraficos;

            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DatosAtrasoLibroClasesPorFecha", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos.Value = string.Empty;
                        txtIndex.Value = string.Empty;
                    }
                }

                if (cantGraficos > 0)
                {
                    if (cantGraficos.ToString().Equals("1"))
                    {
                        divBtnGrafica.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica.Visible = true;
                    }

                    txtcantGraficos.Value = cantGraficos.ToString();
                    txtIndex.Value = 1.ToString();

                    for (int i = 1; i < cantGraficos + 1; i++)
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_DB_T1_AtrasoRetiroLibroPorFecha_Paginado", sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", i);
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                fecha.RemoveRange(0, fecha.Count);
                                while (rdr.Read())
                                {
                                    cant.Add(rdr.GetInt32(0));
                                    fecha.Add(rdr.GetDateTime(1));
                                }
                                rdr.Close();
                                sql.Close();

                                ChartBarra.Series[0].Points.DataBindXY(fecha, cant);
                                CargaImagenes(ChartBarra, i);
                                divGrafico.Visible = true;
                                lblmsg.Text = string.Empty;
                                divBtnPDF.Visible = true;
                                btnAnterior.Enabled = false;
                            }
                        }
                    }
                    //Setea el primer grafico
                    txtSp.Value = "sp_DB_T1_AtrasoRetiroLibroPorFecha_Paginado";
                    SeteaGrafico(idArchivo, Convert.ToInt32(txtIndex.Value), "sp_DB_T1_AtrasoRetiroLibroPorFecha_Paginado_Top");
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico";
                    divGrafico.Visible = false;
                    divBtnPDF.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfAtrasoRetiroLibroPorAsignatura(string idArchivo)
        {
            ArrayList cant = new ArrayList();
            ArrayList asignatura = new ArrayList();
            decimal cantGraficos;

            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DatosAtrasoLibroClasesPorAsignatura ", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos.Value = string.Empty;
                        txtIndex.Value = string.Empty;
                    }
                }

                if (cantGraficos > 0)
                {
                    if (cantGraficos.ToString().Equals("1"))
                    {
                        divBtnGrafica.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica.Visible = true;
                    }

                    txtcantGraficos.Value = cantGraficos.ToString();
                    txtIndex.Value = 1.ToString();

                    for (int i = 1; i < cantGraficos + 1; i++)
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_DB_T4_AtrasoRetiroLibroPorAsignatura_Paginado", sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", i);
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                asignatura.RemoveRange(0, asignatura.Count);
                                while (rdr.Read())
                                {
                                    cant.Add(rdr.GetInt32(0));
                                    asignatura.Add(rdr.GetString(1));
                                }
                                rdr.Close();
                                sql.Close();

                                ChartBarra.Series[0].Points.DataBindXY(asignatura, cant);
                                CargaImagenes(ChartBarra, i);
                                divGrafico.Visible = true;
                                lblmsg.Text = string.Empty;
                                divBtnPDF.Visible = true;
                                btnAnterior.Enabled = false;
                            }
                        }
                    }
                    //Setea el primer grafico
                    txtSp.Value = "sp_DB_T4_AtrasoRetiroLibroPorAsignatura_Paginado";
                    SeteaGrafico(idArchivo, Convert.ToInt32(txtIndex.Value), "sp_DB_T4_AtrasoRetiroLibroPorAsignatura_Paginado_Top");
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico";
                    divGrafico.Visible = false;
                    divBtnPDF.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfAtrasoRetiroLibroPorJornada(string idArchivo)
        {
            ArrayList cant = new ArrayList();
            ArrayList asignatura = new ArrayList();
            decimal cantGraficos;

            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DatosAtrasoLibroClasesPorJornada", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos.Value = string.Empty;
                        txtIndex.Value = string.Empty;
                    }
                }

                if (cantGraficos > 0)
                {
                    if (cantGraficos.ToString().Equals("1"))
                    {
                        divBtnGrafica.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica.Visible = true;
                    }

                    txtcantGraficos.Value = cantGraficos.ToString();
                    txtIndex.Value = 1.ToString();

                    for (int i = 1; i < cantGraficos + 1; i++)
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_DB_T4_AtrasoRetiroLibroPorJornada_Paginado", sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", i);
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                asignatura.RemoveRange(0, asignatura.Count);
                                while (rdr.Read())
                                {
                                    cant.Add(rdr.GetInt32(0));
                                    asignatura.Add(rdr.GetString(1));
                                }
                                rdr.Close();
                                sql.Close();

                                ChartBarra.Series[0].Points.DataBindXY(asignatura, cant);
                                CargaImagenes(ChartBarra, i);
                                divGrafico.Visible = true;
                                lblmsg.Text = string.Empty;
                                divBtnPDF.Visible = true;
                                btnAnterior.Enabled = false;
                            }
                        }
                    }
                    //Setea el primer grafico
                    txtSp.Value = "sp_DB_T4_AtrasoRetiroLibroPorJornada_Paginado";
                    SeteaGrafico(idArchivo, Convert.ToInt32(txtIndex.Value), "sp_DB_T4_AtrasoRetiroLibroPorJornada_Paginado_Top");
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico";
                    divGrafico.Visible = false;
                    divBtnPDF.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private DataTable DatosPorExcel(string idArchivo, string spAtrasoRetiroLibro)
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(spAtrasoRetiroLibro, sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private DataTable DatosPorBloque(string idBloque)
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DatosBloqueAcademico", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idBloque", idBloque);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Libros entregados antes de hora de termino
        private void GrfLibroDevueltosAntesPorDocente(string idArchivo)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;

            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DatosLibroClasesDevueltoAntesPorDocente", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos.Value = string.Empty;
                        txtIndex.Value = string.Empty;
                    }
                }

                if (cantGraficos > 0)
                {
                    if (cantGraficos.ToString().Equals("1"))
                    {
                        divBtnGrafica.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica.Visible = true;
                    }

                    txtcantGraficos.Value = cantGraficos.ToString();
                    txtIndex.Value = 1.ToString();

                    for (int i = 1; i < cantGraficos + 1; i++)
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_DB_T4_LibroClasesDevueltoAntesPorDocente_Paginado", sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", i);
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    cant.Add(rdr.GetInt32(0));
                                    docente.Add(rdr.GetString(1));
                                }
                                rdr.Close();
                                sql.Close();

                                ChartBarra.Series[0].Points.DataBindXY(docente, cant);
                                CargaImagenes(ChartBarra, i);
                                divGrafico.Visible = true;
                                lblmsg.Text = string.Empty;
                                divBtnPDF.Visible = true;
                                btnAnterior.Enabled = false;
                            }
                        }
                    }
                    //Setea el primer grafico
                    txtSp.Value = "sp_DB_T4_LibroClasesDevueltoAntesPorDocente_Paginado";
                    SeteaGrafico(idArchivo, Convert.ToInt32(txtIndex.Value), "sp_DB_T4_LibroClasesDevueltoAntesPorDocente_Paginado_Top");
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico";
                    divGrafico.Visible = false;
                    divBtnPDF.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfLibroDevueltosAntesPorFecha(string idArchivo)
        {
            ArrayList cant = new ArrayList();
            ArrayList fecha = new ArrayList();
            decimal cantGraficos;

            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DatosLibroClasesDevueltoAntesPorFecha", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos.Value = string.Empty;
                        txtIndex.Value = string.Empty;
                    }
                }

                if (cantGraficos > 0)
                {
                    if (cantGraficos.ToString().Equals("1"))
                    {
                        divBtnGrafica.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica.Visible = true;
                    }

                    txtcantGraficos.Value = cantGraficos.ToString();
                    txtIndex.Value = 1.ToString();

                    for (int i = 1; i < cantGraficos + 1; i++)
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_DB_T1_LibroClasesDevueltoAntesPorFecha_Paginado", sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", i);
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                fecha.RemoveRange(0, fecha.Count);
                                while (rdr.Read())
                                {
                                    cant.Add(rdr.GetInt32(0));
                                    fecha.Add(rdr.GetDateTime(1));
                                }
                                rdr.Close();
                                sql.Close();

                                ChartBarra.Series[0].Points.DataBindXY(fecha, cant);
                                CargaImagenes(ChartBarra, i);
                                divGrafico.Visible = true;
                                lblmsg.Text = string.Empty;
                                divBtnPDF.Visible = true;
                                btnAnterior.Enabled = false;
                            }
                        }
                    }
                    //Setea el primer grafico
                    txtSp.Value = "sp_DB_T1_LibroClasesDevueltoAntesPorFecha_Paginado";
                    SeteaGrafico(idArchivo, Convert.ToInt32(txtIndex.Value), "sp_DB_T1_LibroClasesDevueltoAntesPorFecha_Paginado_Top");
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico";
                    divGrafico.Visible = false;
                    divBtnPDF.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfLibroDevueltosAntesPorAsignatura(string idArchivo)
        {
            ArrayList cant = new ArrayList();
            ArrayList asignatura = new ArrayList();
            decimal cantGraficos;

            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DatosLibroClasesDevueltoAntesPorAsignatura ", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos.Value = string.Empty;
                        txtIndex.Value = string.Empty;
                    }
                }

                if (cantGraficos > 0)
                {
                    if (cantGraficos.ToString().Equals("1"))
                    {
                        divBtnGrafica.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica.Visible = true;
                    }

                    txtcantGraficos.Value = cantGraficos.ToString();
                    txtIndex.Value = 1.ToString();

                    for (int i = 1; i < cantGraficos + 1; i++)
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_DB_T4_LibroClasesDevueltoAntesPorAsignatura_Paginado", sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", i);
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                asignatura.RemoveRange(0, asignatura.Count);
                                while (rdr.Read())
                                {
                                    cant.Add(rdr.GetInt32(0));
                                    asignatura.Add(rdr.GetString(1));
                                }
                                rdr.Close();
                                sql.Close();

                                ChartBarra.Series[0].Points.DataBindXY(asignatura, cant);
                                CargaImagenes(ChartBarra, i);
                                divGrafico.Visible = true;
                                lblmsg.Text = string.Empty;
                                divBtnPDF.Visible = true;
                                btnAnterior.Enabled = false;
                            }
                        }
                    }
                    //Setea el primer grafico
                    txtSp.Value = "sp_DB_T4_LibroClasesDevueltoAntesPorAsignatura_Paginado";
                    SeteaGrafico(idArchivo, Convert.ToInt32(txtIndex.Value), "sp_DB_T4_LibroClasesDevueltoAntesPorAsignatura_Paginado_Top");
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico";
                    divGrafico.Visible = false;
                    divBtnPDF.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfLibroDevueltosAntesPorJornada(string idArchivo)
        {
            ArrayList cant = new ArrayList();
            ArrayList asignatura = new ArrayList();
            decimal cantGraficos;

            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DatosLibroClasesDevueltoAntesPorJornada", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos.Value = string.Empty;
                        txtIndex.Value = string.Empty;
                    }
                }

                if (cantGraficos > 0)
                {
                    if (cantGraficos.ToString().Equals("1"))
                    {
                        divBtnGrafica.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica.Visible = true;
                    }

                    txtcantGraficos.Value = cantGraficos.ToString();
                    txtIndex.Value = 1.ToString();

                    for (int i = 1; i < cantGraficos + 1; i++)
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_DB_T4_LibroClasesDevueltoAntesPorJornada_Paginado", sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", i);
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                asignatura.RemoveRange(0, asignatura.Count);
                                while (rdr.Read())
                                {
                                    cant.Add(rdr.GetInt32(0));
                                    asignatura.Add(rdr.GetString(1));
                                }
                                rdr.Close();
                                sql.Close();

                                ChartBarra.Series[0].Points.DataBindXY(asignatura, cant);
                                CargaImagenes(ChartBarra, i);
                                divGrafico.Visible = true;
                                lblmsg.Text = string.Empty;
                                divBtnPDF.Visible = true;
                                btnAnterior.Enabled = false;
                            }
                        }
                    }
                    //Setea el primer grafico
                    txtSp.Value = "sp_DB_T4_LibroClasesDevueltoAntesPorJornada_Paginado";
                    SeteaGrafico(idArchivo, Convert.ToInt32(txtIndex.Value), "sp_DB_T4_LibroClasesDevueltoAntesPorJornada_Paginado_Top");
                }
                else
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico";
                    divGrafico.Visible = false;
                    divBtnPDF.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
            txtIndex.Value = Convert.ToString(Convert.ToInt32(txtIndex.Value) + Convert.ToInt32(1));
            ActualizaContador();
            if (txtIndex.Value.Equals(txtcantGraficos.Value))
            {
                btnSiguiente.Enabled = false;
            }
            if (Convert.ToInt32(txtIndex.Value) > 1)
            {
                btnAnterior.Enabled = true;
            }
            else
            {
                btnSiguiente.Enabled = true;
            }

            if (Session["idArchivo"] != null)
            {
                if (txtidArchivo.Text.Equals(string.Empty))
                {
                    SeteaGrafico(Session["idArchivo"].ToString(), Convert.ToInt32(txtIndex.Value), txtSp.Value+"_Top");
                }
                else
                {
                    SeteaGrafico(txtidArchivo.Text, Convert.ToInt32(txtIndex.Value), txtSp.Value + "_Top");
                }
            }
            if (Session["idArchivo"] == null && txtidArchivo.Text != string.Empty)
            {
                SeteaGrafico(txtidArchivo.Text, Convert.ToInt32(txtIndex.Value), txtSp.Value + "_Top");
            }
        }

        protected void btnAnterior_Click(object sender, EventArgs e)
        {                    
            txtIndex.Value = Convert.ToString(Convert.ToInt32(txtIndex.Value) - Convert.ToInt32(1));
            ActualizaContador();
            if (txtIndex.Value.Equals("1"))
            {
                btnAnterior.Enabled = false;
            }
            if (Convert.ToInt32(txtIndex.Value) < Convert.ToInt32(txtcantGraficos.Value))
            {
                btnSiguiente.Enabled = true;
            }
            else
            {
                btnAnterior.Enabled = true;
            }

            if (Session["idArchivo"] != null)
            {
                if (txtidArchivo.Text.Equals(string.Empty))
                {
                    SeteaGrafico(Session["idArchivo"].ToString(), Convert.ToInt32(txtIndex.Value), txtSp.Value + "_Top");
                }
                else
                {
                    SeteaGrafico(txtidArchivo.Text, Convert.ToInt32(txtIndex.Value), txtSp.Value + "_Top");
                }
            }
            if (Session["idArchivo"] == null && txtidArchivo.Text != string.Empty)
            {
                SeteaGrafico(txtidArchivo.Text, Convert.ToInt32(txtIndex.Value), txtSp.Value + "_Top");
            }
        }

        private void SeteaGrafico(string idArchivo, int index, string spTop)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();

            var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

            if(checkTop.Checked == false)
            {
                GraficoTop.Visible = false;
                if (index > 0 || index <= Convert.ToInt32(txtcantGraficos.Value))
                {
                    using (SqlConnection sql = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(txtSp.Value, sql))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                            cmd.Parameters.AddWithValue("@Pag", txtIndex.Value);
                            cmd.Parameters.AddWithValue("@TamPag", 20);
                            sql.Open();
                            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                            cant.RemoveRange(0, cant.Count);
                            docente.RemoveRange(0, docente.Count);
                            while (rdr.Read())
                            {
                                // cant = int, decimal docente = datetime, string
                                //doc datetime + int; doc datime + decimal;  doc string + decimal; doc string + int 

                                if (txtSp.Value.Contains("_T1_"))
                                {
                                    docente.Add(rdr.GetDateTime(1));
                                    cant.Add(rdr.GetInt32(0));
                                }
                                if (txtSp.Value.Contains("_T2_"))
                                {
                                    docente.Add(rdr.GetDateTime(1));
                                    cant.Add(rdr.GetDecimal(0));
                                }
                                if (txtSp.Value.Contains("_T3_"))
                                {
                                    docente.Add(rdr.GetString(1));
                                    cant.Add(rdr.GetDecimal(0));
                                }
                                if (txtSp.Value.Contains("_T4_"))
                                {
                                    docente.Add(rdr.GetString(1));
                                    cant.Add(rdr.GetInt32(0));
                                }
                            }
                            rdr.Close();
                            sql.Close();

                            ActualizaContador();
                            if (txtcantGraficos.Value.Equals(txtIndex.Value))
                            {
                                btnSiguiente.Enabled = false;
                            }
                            else
                            {
                                btnSiguiente.Enabled = true;
                            }

                            ChartBarra.Series[0].Points.DataBindXY(docente, cant);
                            divGrafico.Visible = true;
                            lblmsg.Text = string.Empty;
                            divBtnPDF.Visible = true;
                        }
                    }
                }
            }
            else
            {
                if (index > 0 || index <= Convert.ToInt32(txtcantGraficos.Value))
                {
                    GraficoTop.Visible = true;
                    using (SqlConnection sql = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(txtSp.Value, sql))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                            cmd.Parameters.AddWithValue("@Pag", txtIndex.Value);
                            cmd.Parameters.AddWithValue("@TamPag", 20);
                            sql.Open();
                            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                            cant.RemoveRange(0, cant.Count);
                            docente.RemoveRange(0, docente.Count);
                            while (rdr.Read())
                            {
                                // cant = int, decimal docente = datetime, string
                                //doc datetime + int; doc datime + decimal;  doc string + decimal; doc string + int 

                                if (txtSp.Value.Contains("_T1_"))
                                {
                                    docente.Add(rdr.GetDateTime(1));
                                    cant.Add(rdr.GetInt32(0));
                                }
                                if (txtSp.Value.Contains("_T2_"))
                                {
                                    docente.Add(rdr.GetDateTime(1));
                                    cant.Add(rdr.GetDecimal(0));
                                }
                                if (txtSp.Value.Contains("_T3_"))
                                {
                                    docente.Add(rdr.GetString(1));
                                    cant.Add(rdr.GetDecimal(0));
                                }
                                if (txtSp.Value.Contains("_T4_"))
                                {
                                    docente.Add(rdr.GetString(1));
                                    cant.Add(rdr.GetInt32(0));
                                }
                            }
                            rdr.Close();
                            sql.Close();

                            ActualizaContador();
                            if (txtcantGraficos.Value.Equals(txtIndex.Value))
                            {
                                btnSiguiente.Enabled = false;
                            }
                            else
                            {
                                btnSiguiente.Enabled = true;
                            }

                            ChartBarra.Series[0].Points.DataBindXY(docente, cant);
                            divGrafico.Visible = true;
                            lblmsg.Text = string.Empty;
                            divBtnPDF.Visible = true;
                        }
                    }

                    // aqui va al vaina nueva   
                    using (SqlConnection sql = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(spTop, sql))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                            sql.Open();
                            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                            cant.RemoveRange(0, cant.Count);
                            docente.RemoveRange(0, docente.Count);
                            while (rdr.Read())
                            {
                                // cant = int, decimal docente = datetime, string
                                //doc datetime + int; doc datime + decimal;  doc string + decimal; doc string + int 

                                if (spTop.Contains("_T1_"))
                                {
                                    docente.Add(rdr.GetDateTime(1));
                                    cant.Add(rdr.GetInt32(0));
                                }
                                if (spTop.Contains("_T2_"))
                                {
                                    docente.Add(rdr.GetDateTime(1));
                                    cant.Add(rdr.GetDecimal(0));
                                }
                                if (spTop.Contains("_T3_"))
                                {
                                    docente.Add(rdr.GetString(1));
                                    cant.Add(rdr.GetDecimal(0));
                                }
                                if (spTop.Contains("_T4_"))
                                {
                                    docente.Add(rdr.GetString(1));
                                    cant.Add(rdr.GetInt32(0));
                                }
                            }
                            rdr.Close();
                            sql.Close();

                            //ActualizaContador();
                            //if (txtcantGraficos.Value.Equals(txtIndex.Value))
                            //{
                            //    btnSiguiente.Enabled = false;
                            //}
                            //else
                            //{
                            //    btnSiguiente.Enabled = true;
                            //}

                            ColumnTop.Series[0].Points.DataBindXY(docente, cant);
                            //divGrafico.Visible = true;
                            //lblmsg.Text = string.Empty;
                            //divBtnPDF.Visible = true;
                        }
                    }
                }
            }          
        }

        private void ActualizaContador()
        {
            lblContador.Text = txtIndex.Value + "/" + txtcantGraficos.Value;
        }

        private void LlenaComboExterno()
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_LlenaComboExterno", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        cmbexterno.DataSource = dt;
                        cmbexterno.DataTextField = "Descripcion";
                        cmbexterno.DataValueField = "idComboExterno";
                        cmbexterno.DataBind();
                        sql.Close();

                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void LlenaComboInterno()
        {
            try
            {
                string idComboPadre = cmbexterno.SelectedValue.ToString();
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_LlenaComboInterno", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idComboPadre", idComboPadre);
                        sql.Open();
                        SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        cmbinterno.DataSource = rdr;
                        cmbinterno.DataTextField = "Descripcion";
                        cmbinterno.DataValueField = "Valor";
                        cmbinterno.DataBind();

                        rdr.Close();
                        sql.Close();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        protected void cmbexterno_SelectedIndexChanged(object sender, EventArgs e)
        {

            lblmsg.Text = string.Empty;
            LimpiaCheck();
            LlenaComboInterno();
        }

        protected void cmbinterno_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idComboExterno = cmbexterno.SelectedValue.ToString();
            string idComboInterno = cmbinterno.SelectedValue.ToString();
            lblmsg.Text = string.Empty;

            VaciaCarpeta();
            if (!idComboInterno.Equals(""))
            {
                divGrafico.Visible = true;
                if (Session["idArchivo"] != null)
                {
                    if (txtidArchivo.Text.Equals(string.Empty))
                    {
                        if (idComboExterno.Equals("2") && idComboInterno.Equals("PD"))
                        {
                            GrfClsRealizadaPorDocente(Session["idArchivo"].ToString());
                        }
                        if (idComboExterno.Equals("2") && idComboInterno.Equals("PF"))
                        {
                            GrfClsRealizadaPorFecha(Session["idArchivo"].ToString());
                        }
                        if (idComboExterno.Equals("2") && idComboInterno.Equals("PA"))
                        {
                            GrfClsRealizadaPorAsignatura(Session["idArchivo"].ToString());
                        }
                        if (idComboExterno.Equals("2") && idComboInterno.Equals("PJ"))
                        {
                            GrfClsRealizadaPorJornada(Session["idArchivo"].ToString());
                        }

                        if (idComboExterno.Equals("3") && idComboInterno.Equals("PD"))
                        {
                            GrfClsNoRealizadaPorDocente(Session["idArchivo"].ToString());
                        }
                        if (idComboExterno.Equals("3") && idComboInterno.Equals("PF"))
                        {
                            GrfClsNoRealizadaPorFecha(Session["idArchivo"].ToString());
                        }
                        if (idComboExterno.Equals("3") && idComboInterno.Equals("PA"))
                        {
                            GrfClsNoRealizadaPorAsignatura(Session["idArchivo"].ToString());
                        }
                        if (idComboExterno.Equals("3") && idComboInterno.Equals("PJ"))
                        {
                            GrfClsNoRealizadaPorJornada(Session["idArchivo"].ToString());
                        }

                        //faltan 2 mas
                        if (idComboExterno.Equals("4") && idComboInterno.Equals("PD"))
                        {
                            GrfAtrasoRetiroLibroPorDocente(Session["idArchivo"].ToString());
                        }
                        if (idComboExterno.Equals("4") && idComboInterno.Equals("PF"))
                        {
                            GrfAtrasoRetiroLibroPorFecha(Session["idArchivo"].ToString());
                        }
                        if (idComboExterno.Equals("4") && idComboInterno.Equals("PA"))
                        {
                            GrfAtrasoRetiroLibroPorAsignatura(Session["idArchivo"].ToString());
                        }
                        if (idComboExterno.Equals("4") && idComboInterno.Equals("PJ"))
                        {
                            GrfAtrasoRetiroLibroPorJornada(Session["idArchivo"].ToString());
                        }

                        if (idComboExterno.Equals("5") && idComboInterno.Equals("PD"))
                        {
                            GrfLibroDevueltosAntesPorDocente(Session["idArchivo"].ToString());
                        }
                        if (idComboExterno.Equals("5") && idComboInterno.Equals("PF"))
                        {
                            GrfLibroDevueltosAntesPorFecha(Session["idArchivo"].ToString());
                        }
                        //if (idComboExterno.Equals("5") && idComboInterno.Equals("PA"))
                        //{
                        //    GrfLibroDevueltosAntesPorAsignatura(Session["idArchivo"].ToString());
                        //}
                        if (idComboExterno.Equals("5") && idComboInterno.Equals("PJ"))
                        {
                            GrfLibroDevueltosAntesPorJornada(Session["idArchivo"].ToString());
                        }

                        if (idComboExterno.Equals("9") && idComboInterno.Equals("PD"))
                        {
                            GrfLibroNoMarcadoPorDocente(Session["idArchivo"].ToString());
                        }
                        if (idComboExterno.Equals("9") && idComboInterno.Equals("PF"))
                        {
                            GrfLibroNoMarcadoPorFecha(Session["idArchivo"].ToString());
                        }
                        if (idComboExterno.Equals("9") && idComboInterno.Equals("PA"))
                        {
                            GrfLibroNoMarcadoPorAsignatura(Session["idArchivo"].ToString());
                        }
                        if (idComboExterno.Equals("9") && idComboInterno.Equals("PJ"))
                        {
                            GrfLibroNoMarcadoPorJornada(Session["idArchivo"].ToString());
                        }

                        if (idComboExterno.Equals("10") && idComboInterno.Equals("PA"))
                        {
                            GrfPorcentajealumnosExistentesPorAsignatura(Session["idArchivo"].ToString());
                        }
                        if (idComboExterno.Equals("10") && idComboInterno.Equals("PF"))
                        {
                            GrfPorcentajealumnosExistentesPorFecha(Session["idArchivo"].ToString());
                        }
                        if (idComboExterno.Equals("10") && idComboInterno.Equals("PJ"))
                        {
                            GrfPorcentajealumnosExistentesPorJornada(Session["idArchivo"].ToString());
                        }
                    }
                    else
                    {
                        if (idComboExterno.Equals("2") && idComboInterno.Equals("PD"))
                        {
                            GrfClsRealizadaPorDocente(txtidArchivo.Text);
                        }
                        if (idComboExterno.Equals("2") && idComboInterno.Equals("PF"))
                        {
                            GrfClsRealizadaPorFecha(txtidArchivo.Text);
                        }
                        if (idComboExterno.Equals("2") && idComboInterno.Equals("PA"))
                        {
                            GrfClsRealizadaPorAsignatura(txtidArchivo.Text);
                        }
                        if (idComboExterno.Equals("2") && idComboInterno.Equals("PJ"))
                        {
                            GrfClsRealizadaPorJornada(txtidArchivo.Text);
                        }

                        if (idComboExterno.Equals("3") && idComboInterno.Equals("PD"))
                        {
                            GrfClsNoRealizadaPorDocente(txtidArchivo.Text);
                        }
                        if (idComboExterno.Equals("3") && idComboInterno.Equals("PF"))
                        {
                            GrfClsNoRealizadaPorFecha(txtidArchivo.Text);
                        }
                        if (idComboExterno.Equals("3") && idComboInterno.Equals("PA"))
                        {
                            GrfClsNoRealizadaPorAsignatura(txtidArchivo.Text);
                        }
                        if (idComboExterno.Equals("3") && idComboInterno.Equals("PJ"))
                        {
                            GrfClsNoRealizadaPorJornada(txtidArchivo.Text);
                        }

                        //faltan 2 mas

                        if (idComboExterno.Equals("4") && idComboInterno.Equals("PD"))
                        {
                            GrfAtrasoRetiroLibroPorDocente(txtidArchivo.Text);
                        }
                        if (idComboExterno.Equals("4") && idComboInterno.Equals("PF"))
                        {
                            GrfAtrasoRetiroLibroPorFecha(txtidArchivo.Text);
                        }
                        if (idComboExterno.Equals("4") && idComboInterno.Equals("PA"))
                        {
                            GrfAtrasoRetiroLibroPorAsignatura(txtidArchivo.Text);
                        }
                        if (idComboExterno.Equals("4") && idComboInterno.Equals("PJ"))
                        {
                            GrfAtrasoRetiroLibroPorJornada(txtidArchivo.Text);
                        }

                        if (idComboExterno.Equals("5") && idComboInterno.Equals("PD"))
                        {
                            GrfLibroDevueltosAntesPorDocente(txtidArchivo.Text);
                        }
                        if (idComboExterno.Equals("5") && idComboInterno.Equals("PF"))
                        {
                            GrfLibroDevueltosAntesPorFecha(txtidArchivo.Text);
                        }
                        //if (idComboExterno.Equals("5") && idComboInterno.Equals("PA"))
                        //{
                        //    GrfLibroDevueltosAntesPorAsignatura(txtidArchivo.Text);
                        //}
                        if (idComboExterno.Equals("5") && idComboInterno.Equals("PJ"))
                        {
                            GrfLibroDevueltosAntesPorJornada(txtidArchivo.Text);
                        }

                        if (idComboExterno.Equals("9") && idComboInterno.Equals("PD"))
                        {
                            GrfLibroNoMarcadoPorDocente(txtidArchivo.Text);
                        }
                        if (idComboExterno.Equals("9") && idComboInterno.Equals("PF"))
                        {
                            GrfLibroNoMarcadoPorFecha(txtidArchivo.Text);
                        }
                        if (idComboExterno.Equals("9") && idComboInterno.Equals("PA"))
                        {
                            GrfLibroNoMarcadoPorAsignatura(txtidArchivo.Text);
                        }
                        if (idComboExterno.Equals("9") && idComboInterno.Equals("PJ"))
                        {
                            GrfLibroNoMarcadoPorJornada(txtidArchivo.Text);
                        }


                        if (idComboExterno.Equals("10") && idComboInterno.Equals("PA"))
                        {
                            GrfPorcentajealumnosExistentesPorAsignatura(txtidArchivo.Text);
                        }
                        if (idComboExterno.Equals("10") && idComboInterno.Equals("PF"))
                        {
                            GrfPorcentajealumnosExistentesPorFecha(txtidArchivo.Text);
                        }
                        if (idComboExterno.Equals("10") && idComboInterno.Equals("PJ"))
                        {
                            GrfPorcentajealumnosExistentesPorJornada(txtidArchivo.Text);
                        }
                    }
                }
                if (Session["idArchivo"] == null && txtidArchivo.Text != string.Empty)
                {
                    if (idComboExterno.Equals("2") && idComboInterno.Equals("PD"))
                    {
                        GrfClsRealizadaPorDocente(txtidArchivo.Text);
                    }
                    if (idComboExterno.Equals("2") && idComboInterno.Equals("PF"))
                    {
                        GrfClsRealizadaPorFecha(txtidArchivo.Text);
                    }
                    if (idComboExterno.Equals("2") && idComboInterno.Equals("PA"))
                    {
                        GrfClsRealizadaPorAsignatura(txtidArchivo.Text);
                    }
                    if (idComboExterno.Equals("2") && idComboInterno.Equals("PJ"))
                    {
                        GrfClsRealizadaPorJornada(txtidArchivo.Text);
                    }

                    if (idComboExterno.Equals("3") && idComboInterno.Equals("PD"))
                    {
                        GrfClsNoRealizadaPorDocente(txtidArchivo.Text);
                    }
                    if (idComboExterno.Equals("3") && idComboInterno.Equals("PF"))
                    {
                        GrfClsNoRealizadaPorFecha(txtidArchivo.Text);
                    }
                    if (idComboExterno.Equals("3") && idComboInterno.Equals("PA"))
                    {
                        GrfClsNoRealizadaPorAsignatura(txtidArchivo.Text);
                    }
                    if (idComboExterno.Equals("3") && idComboInterno.Equals("PJ"))
                    {
                        GrfClsNoRealizadaPorJornada(txtidArchivo.Text);
                    }

                    //faltan 2 mas

                    if (idComboExterno.Equals("4") && idComboInterno.Equals("PD"))
                    {
                        GrfAtrasoRetiroLibroPorDocente(txtidArchivo.Text);
                    }
                    if (idComboExterno.Equals("4") && idComboInterno.Equals("PF"))
                    {
                        GrfAtrasoRetiroLibroPorFecha(txtidArchivo.Text);
                    }
                    if (idComboExterno.Equals("4") && idComboInterno.Equals("PA"))
                    {
                        GrfAtrasoRetiroLibroPorAsignatura(txtidArchivo.Text);
                    }
                    if (idComboExterno.Equals("4") && idComboInterno.Equals("PJ"))
                    {
                        GrfAtrasoRetiroLibroPorJornada(txtidArchivo.Text);
                    }

                    if (idComboExterno.Equals("5") && idComboInterno.Equals("PD"))
                    {
                        GrfLibroDevueltosAntesPorDocente(txtidArchivo.Text);
                    }
                    if (idComboExterno.Equals("5") && idComboInterno.Equals("PF"))
                    {
                        GrfLibroDevueltosAntesPorFecha(txtidArchivo.Text);
                    }
                    //if (idComboExterno.Equals("5") && idComboInterno.Equals("PA"))
                    //{
                    //    GrfLibroDevueltosAntesPorAsignatura(txtidArchivo.Text);
                    //}
                    if (idComboExterno.Equals("5") && idComboInterno.Equals("PJ"))
                    {
                        GrfLibroDevueltosAntesPorJornada(txtidArchivo.Text);
                    }

                    if (idComboExterno.Equals("9") && idComboInterno.Equals("PD"))
                    {
                        GrfLibroNoMarcadoPorDocente(txtidArchivo.Text);
                    }
                    if (idComboExterno.Equals("9") && idComboInterno.Equals("PF"))
                    {
                        GrfLibroNoMarcadoPorFecha(txtidArchivo.Text);
                    }
                    if (idComboExterno.Equals("9") && idComboInterno.Equals("PA"))
                    {
                        GrfLibroNoMarcadoPorAsignatura(txtidArchivo.Text);
                    }
                    if (idComboExterno.Equals("9") && idComboInterno.Equals("PJ"))
                    {
                        GrfLibroNoMarcadoPorJornada(txtidArchivo.Text);
                    }


                    if (idComboExterno.Equals("10") && idComboInterno.Equals("PA"))
                    {
                        GrfPorcentajealumnosExistentesPorAsignatura(txtidArchivo.Text);
                    }
                    if (idComboExterno.Equals("10") && idComboInterno.Equals("PF"))
                    {
                        GrfPorcentajealumnosExistentesPorFecha(txtidArchivo.Text);
                    }
                    if (idComboExterno.Equals("10") && idComboInterno.Equals("PJ"))
                    {
                        GrfPorcentajealumnosExistentesPorJornada(txtidArchivo.Text);
                    }
                }               
                if(txtidArchivo.Text.Equals(string.Empty) && Session["idArchivo"] == null)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Debe cargar archivo previamente";
                    divGrafico.Visible = false;
                }
                if (lblmsg.Text.Equals(""))
                {
                    checkTop.Enabled = true;
                }
            }
            else
            {
                LimpiaCheck();
                divGrafico.Visible = false;
            }
        }

        private void CargaImagenes(Chart chartBarra, int cant)
        {          
            ChartBarra.SaveImage(Server.MapPath(string.Format("~/Imagenes/{0}", "Grafico_Barra_" + cant + ".jpeg")) , ChartImageFormat.Jpeg);
        }

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            VerificaCarpetaReporte();
            divBtnPDF.Visible = false;
        }

        private void VerificaCarpetaReporte()
        {
            string ruta = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)+ @"\Reportes Graficos Inacap";
            int cantGraficos = 0;
            try
            {
                if (!Directory.Exists(ruta))
                {
                    Directory.CreateDirectory(ruta);
                    cantGraficos = CuentaGraficos();
                    GeneraPDF(cantGraficos);
                }
                else
                {
                    cantGraficos = CuentaGraficos();
                    GeneraPDF(cantGraficos);
                }
                lblmsg.ForeColor = Color.Green;
                lblmsg.Text = "PDF generado correctamente";
            }
            catch (Exception)
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "No se pudo generar el archivo";
                throw;
            }
            
        }

        private int CuentaGraficos()
        {
            string[] dirs = Directory.GetFiles(Server.MapPath("~/Imagenes/"));

            int cantidad = dirs.Length;

            return cantidad;
        }

        private void GeneraPDF(int cantGraficos)
        {

            using (var ms = new MemoryStream())
            {
                using (var document = new Document(PageSize.LETTER, 50, 50, 40, 15))
                {
                    string valorComboExterno = cmbexterno.SelectedItem.Text;
                    string valorComboInterno = cmbinterno.SelectedItem.Text;
                    decimal cantPag = 0;
                    int index = 1;

                    cantPag = Math.Ceiling((Convert.ToDecimal(cantGraficos) / Convert.ToDecimal(2)));

                    PdfWriter.GetInstance(document, ms);
                    document.Open();

                    for (int i = 0; i < cantPag; i++)
                    {
                        //Encabezado con Logo
                        iTextSharp.text.Image imagenLogoUni = iTextSharp.text.Image.GetInstance(Server.MapPath(string.Format("~/Images/{0}", "logo_uni.jpg")));
                        document.AddCreator("Inacap - Santiago de Chile");
                        var titulo = new Paragraph("             UNIVERSIDAD TECNOLÓGICA DE CHILE");
                        titulo.SpacingBefore = 200;
                        titulo.SpacingAfter = 0;
                        titulo.Alignment = 1; //0-Left, 1 middle,2 Right
                        document.Add(titulo);
                        document.Add(Chunk.NEWLINE);
                        imagenLogoUni.BorderWidth = 0;
                        imagenLogoUni.Alignment = Element.ALIGN_TOP;
                        imagenLogoUni.SetAbsolutePosition(60, 730);
                        imagenLogoUni.ScalePercent(10f);
                        document.Add(imagenLogoUni);

                        //Se le setea la fecha al archivo
                        var fechaPdf = new Paragraph("" + DateTime.Now);
                        fechaPdf.SpacingBefore = 5;
                        fechaPdf.SpacingAfter = 0;
                        fechaPdf.Alignment = 2; //0-Left, 1 middle,2 Right
                        document.Add(fechaPdf);

                        //Filtro seleccionado para la gráfica
                        var parrafo2 = new Paragraph("             " + valorComboExterno + " " + valorComboInterno);
                        parrafo2.SpacingAfter = 0;
                        parrafo2.Alignment = 1; //0-Left, 1 middle,2 Right
                        document.Add(parrafo2);

                        //Cargar imagenes
                        for (int j = 0; j < 2; j++)
                        {
                            if (index <= cantGraficos)
                            {
                                iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(Server.MapPath(string.Format("~/Imagenes/{0}", "Grafico_Barra_" + index + ".jpeg")));
                                imagen.BorderWidth = 0;
                                imagen.Alignment = Element.ALIGN_CENTER;
                                imagen.ScalePercent(75f);
                                document.Add(imagen);
                                index++;
                            }
                        }
                        // Se le pone paginador
                        var nroPag = new Paragraph("Página " + Convert.ToInt32(i + 1) + "/" + cantPag);
                        nroPag.SpacingBefore = 20;
                        nroPag.SpacingAfter = 0;
                        nroPag.Alignment = 2; //0-Left, 1 middle,2 Right
                        document.Add(nroPag);

                        //Agregamos otra página
                        document.NewPage();
                    }
                    // Cerramos el documento                 
                    document.Close();
                }
                Response.Clear();
                //Response.ContentType = "application/pdf";
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("content-disposition", "attachment;filename= Reporte.pdf");
                Response.Buffer = true;
                Response.Clear();
                var bytes = ms.ToArray();
                Response.OutputStream.Write(bytes, 0, bytes.Length);
                Response.OutputStream.Flush();
            }
        }

        private void VaciaCarpeta()
        {
            foreach (var item in Directory.GetFiles(Server.MapPath(string.Format("~/Imagenes")), "*.*"))
            {
                File.SetAttributes(item, FileAttributes.Normal);
                File.Delete(item);
            }
        }

        protected void checkTop_CheckedChanged(object sender, EventArgs e)
        {
            if (Session["idArchivo"] != null)
            {
                if (txtidArchivo.Text.Equals(string.Empty))
                {
                    SeteaGrafico(Session["idArchivo"].ToString(), Convert.ToInt32(txtIndex.Value), txtSp.Value + "_Top");

                }
                else
                {
                    SeteaGrafico(txtidArchivo.Text, Convert.ToInt32(txtIndex.Value), txtSp.Value + "_Top");
                }
                   
            }
            if (Session["idArchivo"] == null && txtidArchivo.Text != string.Empty)
            {
                SeteaGrafico(txtidArchivo.Text, Convert.ToInt32(txtIndex.Value), txtSp.Value + "_Top");
            }
                //SeteaGrafico(, int index, string spTop);
        }

        private void LimpiaCheck()
        {
            checkTop.Enabled = false;
            checkTop.Checked = false;
        }      
    }
}