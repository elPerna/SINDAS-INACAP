using FormularioAsistenciaWeb.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace FormularioAsistenciaWeb
{
    public partial class DashboardHistorial : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            divGrafico.Visible = false;
            if (!IsPostBack)
            {
                LlenaComboExterno();
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

        private void LlenaComboExterno()
        {
            try
            {
                lblmsg.Text = string.Empty;
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
                lblmsg.Text = string.Empty;
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
            LlenaComboInterno();
        }

        protected void cmbinterno_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idComboExterno = cmbexterno.SelectedValue.ToString();
            string idComboInterno = cmbinterno.SelectedValue.ToString();

            if (!idComboInterno.Equals(""))
            {
                divGrafico.Visible = true;
                if (!txtidArchivo.Text.Equals(string.Empty) && !txtidArchivo2.Text.Equals(string.Empty))
                {                 
                    lblmsg.Text = string.Empty;

                    if (idComboExterno.Equals("2") && idComboInterno.Equals("PD"))
                    {
                        GrfClsRealizadaPorDocente(txtidArchivo.Text, txtidArchivo2.Text);
                    }
                    if (idComboExterno.Equals("2") && idComboInterno.Equals("PF"))
                    {
                        GrfClsRealizadaPorFecha(txtidArchivo.Text, txtidArchivo2.Text);
                    }
                    if (idComboExterno.Equals("2") && idComboInterno.Equals("PA"))
                    {
                        GrfClsRealizadaPorAsignatura(txtidArchivo.Text, txtidArchivo2.Text);
                    }
                    if (idComboExterno.Equals("2") && idComboInterno.Equals("PJ"))
                    {
                        GrfClsRealizadaPorJornada(txtidArchivo.Text, txtidArchivo2.Text);
                    }

                    if (idComboExterno.Equals("3") && idComboInterno.Equals("PD"))
                    {
                        GrfClsNoRealizadaPorDocente(txtidArchivo.Text, txtidArchivo2.Text);
                    }
                    if (idComboExterno.Equals("3") && idComboInterno.Equals("PF"))
                    {
                        GrfClsNoRealizadaPorFecha(txtidArchivo.Text, txtidArchivo2.Text);
                    }
                    if (idComboExterno.Equals("3") && idComboInterno.Equals("PA"))
                    {
                        GrfClsNoRealizadaPorAsignatura(txtidArchivo.Text, txtidArchivo2.Text);
                    }
                    if (idComboExterno.Equals("3") && idComboInterno.Equals("PJ"))
                    {
                        GrfClsNoRealizadaPorJornada(txtidArchivo.Text, txtidArchivo2.Text);
                    }

                    if (idComboExterno.Equals("4") && idComboInterno.Equals("PD"))
                    {
                        GrfAtrasoRetiroLibroPorDocente(txtidArchivo.Text, txtidArchivo2.Text);
                    }
                    if (idComboExterno.Equals("4") && idComboInterno.Equals("PF"))
                    {
                        GrfAtrasoRetiroLibroPorFecha(txtidArchivo.Text, txtidArchivo2.Text);
                    }
                    if (idComboExterno.Equals("4") && idComboInterno.Equals("PA"))
                    {
                        GrfAtrasoRetiroLibroPorAsignatura(txtidArchivo.Text, txtidArchivo2.Text);
                    }
                    if (idComboExterno.Equals("4") && idComboInterno.Equals("PJ"))
                    {
                        GrfAtrasoRetiroLibroPorJornada(txtidArchivo.Text, txtidArchivo2.Text);
                    }

                    if (idComboExterno.Equals("5") && idComboInterno.Equals("PD"))
                    {
                        GrfLibroDevueltosAntesPorDocente(txtidArchivo.Text, txtidArchivo2.Text);
                    }
                    if (idComboExterno.Equals("5") && idComboInterno.Equals("PF"))
                    {
                        GrfLibroDevueltosAntesPorFecha(txtidArchivo.Text, txtidArchivo2.Text);
                    }
                    if (idComboExterno.Equals("5") && idComboInterno.Equals("PA"))
                    {
                        GrfLibroDevueltosAntesPorAsignatura(txtidArchivo.Text, txtidArchivo2.Text);
                    }
                    if (idComboExterno.Equals("5") && idComboInterno.Equals("PJ"))
                    {
                        GrfLibroDevueltosAntesPorJornada(txtidArchivo.Text, txtidArchivo2.Text);
                    }

                    if (idComboExterno.Equals("9") && idComboInterno.Equals("PD"))
                    {
                        GrfLibroNoMarcadoPorDocente(txtidArchivo.Text, txtidArchivo2.Text);
                    }
                    if (idComboExterno.Equals("9") && idComboInterno.Equals("PF"))
                    {
                        GrfLibroNoMarcadoPorFecha(txtidArchivo.Text, txtidArchivo2.Text);
                    }
                    if (idComboExterno.Equals("9") && idComboInterno.Equals("PA"))
                    {
                        GrfLibroNoMarcadoPorAsignatura(txtidArchivo.Text, txtidArchivo2.Text);
                    }
                    if (idComboExterno.Equals("9") && idComboInterno.Equals("PJ"))
                    {
                        GrfLibroNoMarcadoPorJornada(txtidArchivo.Text, txtidArchivo2.Text);
                    }

                    if (idComboExterno.Equals("10") && idComboInterno.Equals("PA"))
                    {
                        GrfPorcentajealumnosExistentesPorAsignatura(txtidArchivo.Text, txtidArchivo2.Text);
                    }
                    if (idComboExterno.Equals("10") && idComboInterno.Equals("PF"))
                    {
                        GrfPorcentajealumnosExistentesPorFecha(txtidArchivo.Text, txtidArchivo2.Text);
                    }
                    if (idComboExterno.Equals("10") && idComboInterno.Equals("PJ"))
                    {
                        GrfPorcentajealumnosExistentesPorJornada(txtidArchivo.Text, txtidArchivo2.Text);
                    }
                }
                if (txtidArchivo.Text.Equals(txtidArchivo2.Text) && !txtidArchivo.Text.Equals(string.Empty))
                {
                    divGrafico.Visible = false;
                    lblmsg.Text = string.Empty;
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Está comparando los mismos archivos";
                }
                if (txtidArchivo.Text.Equals(string.Empty) || txtidArchivo2.Text.Equals(string.Empty))
                {
                    divGrafico.Visible = false;
                    lblmsg.Text = string.Empty;
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Debe ingresar los ids de los archivos";
                }
            }
            else
            {
                divGrafico.Visible = false;
            }
        }

        private void SeteaGrafico(string idArchivo, int index, string idArchivo2, int index2, string sp)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            txtSp.Value = string.Empty;
            txtSp.Value = sp;

            var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

            if (!txtcantGraficos.Value.Equals("-1"))
            {
                if (index > 0 || index <= Convert.ToInt32(txtcantGraficos.Value))
                {
                    using (SqlConnection sql = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(sp, sql))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                            cmd.Parameters.AddWithValue("@Pag", index);
                            cmd.Parameters.AddWithValue("@TamPag", 20);
                            sql.Open();
                            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                            cant.RemoveRange(0, cant.Count);
                            docente.RemoveRange(0, docente.Count);
                            while (rdr.Read())
                            {
                                // cant = int, decimal docente = datetime, string
                                //doc datetime + int; doc datime + decimal;  doc string + decimal; doc string + int 

                                if (sp.Contains("_T1_"))
                                {
                                    docente.Add(rdr.GetDateTime(1));
                                    cant.Add(rdr.GetInt32(0));
                                }
                                if (sp.Contains("_T2_"))
                                {
                                    docente.Add(rdr.GetDateTime(1));
                                    cant.Add(rdr.GetDecimal(0));
                                }
                                if (sp.Contains("_T3_"))
                                {
                                    docente.Add(rdr.GetString(1));
                                    cant.Add(rdr.GetDecimal(0));
                                }
                                if (sp.Contains("_T4_"))
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
                        }
                    }
                }
            }
            //else
            //{
            //    lblmsg.ForeColor = Color.Red;
            //    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo.Text;
            //    //divGrafico.Visible = false;
            //}


            // segundo grafico
            if (!txtcantGraficos2.Value.Equals("-1"))
            {
                if (index2 > 0 || index2 <= Convert.ToInt32(txtcantGraficos2.Value))
                {
                    using (SqlConnection sql = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(sp, sql))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                            cmd.Parameters.AddWithValue("@Pag", index2);
                            cmd.Parameters.AddWithValue("@TamPag", 20);
                            sql.Open();
                            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                            cant.RemoveRange(0, cant.Count);
                            docente.RemoveRange(0, docente.Count);
                            while (rdr.Read())
                            {
                                // cant = int, decimal docente = datetime, string
                                //doc datetime + int; doc datime + decimal;  doc string + decimal; doc string + int 

                                if (sp.Contains("_T1_"))
                                {
                                    docente.Add(rdr.GetDateTime(1));
                                    cant.Add(rdr.GetInt32(0));
                                }
                                if (sp.Contains("_T2_"))
                                {
                                    docente.Add(rdr.GetDateTime(1));
                                    cant.Add(rdr.GetDecimal(0));
                                }
                                if (sp.Contains("_T3_"))
                                {
                                    docente.Add(rdr.GetString(1));
                                    cant.Add(rdr.GetDecimal(0));
                                }
                                if (sp.Contains("_T4_"))
                                {
                                    docente.Add(rdr.GetString(1));
                                    cant.Add(rdr.GetInt32(0));
                                }
                            }
                            rdr.Close();
                            sql.Close();

                            ActualizaContador2();
                            if (txtcantGraficos2.Value.Equals(txtIndex2.Value))
                            {
                                btnSiguiente2.Enabled = false;
                            }
                            else
                            {
                                btnSiguiente2.Enabled = true;
                            }

                            ChartBarra2.Series[0].Points.DataBindXY(docente, cant);
                            divGrafico.Visible = true;
                            lblmsg.Text = string.Empty;
                        }
                    }
                }
            }
            //else
            //{
            //    lblmsg.ForeColor = Color.Red;
            //    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo2.Text;
            //    //divGrafico.Visible = false;
            //}
            if (txtcantGraficos.Value.Equals("-1") && txtcantGraficos2.Value.Equals("-1"))
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar los gráficos";
            }
            if (Convert.ToInt32(txtcantGraficos.Value) > 0 && txtcantGraficos2.Value.Equals("-1"))
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo2.Text;
            }
            if (txtcantGraficos.Value.Equals("-1") && Convert.ToInt32(txtcantGraficos2.Value) > 0)
            {
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo.Text;
            }

        }

        private void ActualizaContador()
        {
            lblContador.Text = txtIndex.Value + "/" + txtcantGraficos.Value;
        }

        private void ActualizaContador2()
        {
            lblContador2.Text = txtIndex2.Value + "/" + txtcantGraficos2.Value;
        }

        #region Clase realizada 
        private void GrfClsRealizadaPorDocente(string idArchivo, string idArchivo2)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;
            decimal cantGraficos2;
            string sp = "sp_DB_T4_ClsRealizadaPorDocente_Paginado";
            txtSp.Value = sp;
            lblmsg.Text = string.Empty;
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
                        txtcantGraficos.Value = Convert.ToInt32(-1).ToString();
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

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex.Value) > 0 || Convert.ToInt32(txtIndex.Value) <= Convert.ToInt32(txtcantGraficos.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
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
                            }
                        }
                    }
                    divGrafico.Visible = true;
                    divGrafica1.Visible = true;
                    btnAnterior.Enabled = false;
                }
                else
                {
                    txtIndex.Value = Convert.ToInt32(-1).ToString();
                    divGrafica1.Visible = false;
                    divBtnGrafica.Visible = false;
                }


                // grafico 2
                cant.RemoveRange(0, cant.Count);
                docente.RemoveRange(0, docente.Count);
                cantGraficos2 = 0;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DB_ClsRealizadaPorDocente", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos2 = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos2.Value = Convert.ToInt32(-1).ToString();
                        txtIndex2.Value = string.Empty;
                    }
                }

                if (cantGraficos2 > 0)
                {
                    if (cantGraficos2.ToString().Equals("1"))
                    {
                        divBtnGrafica2.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica2.Visible = true;
                    }

                    txtcantGraficos2.Value = cantGraficos2.ToString();
                    txtIndex2.Value = 1.ToString();

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex2.Value) > 0 || Convert.ToInt32(txtIndex2.Value) <= Convert.ToInt32(txtcantGraficos2.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex2.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {

                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                }
                                rdr.Close();
                                sql.Close();

                                ActualizaContador2();
                                if (txtcantGraficos2.Value.Equals(txtIndex2.Value))
                                {
                                    btnSiguiente2.Enabled = false;
                                }
                                else
                                {
                                    btnSiguiente2.Enabled = true;
                                }

                                ChartBarra2.Series[0].Points.DataBindXY(docente, cant);
                                divGrafico.Visible = true;
                            }
                        }
                    }

                    divGrafico.Visible = true;
                    divGrafica2.Visible = true;
                    btnAnterior2.Enabled = false;
                }
                else
                {
                    txtIndex2.Value = Convert.ToInt32(-1).ToString();
                    divGrafica2.Visible = false;
                    divBtnGrafica2.Visible = false;
                }

                if (cantGraficos == 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = " ";
                }
                if (cantGraficos > 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo2.Text;
                }
                if (cantGraficos == 0 && cantGraficos2 > 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo.Text;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfClsRealizadaPorFecha(string idArchivo, string idArchivo2)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;
            decimal cantGraficos2;
            string sp = "sp_DB_T1_ClsRealizadaPorFecha_Paginado";
            txtSp.Value = sp;
            lblmsg.Text = string.Empty;
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
                        txtcantGraficos.Value = Convert.ToInt32(-1).ToString();
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

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex.Value) > 0 || Convert.ToInt32(txtIndex.Value) <= Convert.ToInt32(txtcantGraficos.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
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
                            }
                        }
                    }
                    divGrafico.Visible = true;
                    divGrafica1.Visible = true;
                    btnAnterior.Enabled = false;
                }
                else
                {
                    txtIndex.Value = Convert.ToInt32(-1).ToString();
                    divGrafica1.Visible = false;
                    divBtnGrafica.Visible = false;
                }


                // grafico 2
                cant.RemoveRange(0, cant.Count);
                docente.RemoveRange(0, docente.Count);
                cantGraficos2 = 0;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DB_ClsRealizadaPorFecha", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos2 = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos2.Value = Convert.ToInt32(-1).ToString();
                        txtIndex2.Value = string.Empty;
                    }
                }

                if (cantGraficos2 > 0)
                {
                    if (cantGraficos2.ToString().Equals("1"))
                    {
                        divBtnGrafica2.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica2.Visible = true;
                    }

                    txtcantGraficos2.Value = cantGraficos2.ToString();
                    txtIndex2.Value = 1.ToString();

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex2.Value) > 0 || Convert.ToInt32(txtIndex2.Value) <= Convert.ToInt32(txtcantGraficos2.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex2.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {

                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                }
                                rdr.Close();
                                sql.Close();

                                ActualizaContador2();
                                if (txtcantGraficos2.Value.Equals(txtIndex2.Value))
                                {
                                    btnSiguiente2.Enabled = false;
                                }
                                else
                                {
                                    btnSiguiente2.Enabled = true;
                                }

                                ChartBarra2.Series[0].Points.DataBindXY(docente, cant);
                                divGrafico.Visible = true;
                            }
                        }
                    }

                    divGrafico.Visible = true;
                    divGrafica2.Visible = true;
                    btnAnterior2.Enabled = false;
                }
                else
                {
                    txtIndex2.Value = Convert.ToInt32(-1).ToString();
                    divGrafica2.Visible = false;
                    divBtnGrafica2.Visible = false;
                }

                if (cantGraficos == 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar los gráficos";
                }
                if (cantGraficos > 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo2.Text;
                }
                if (cantGraficos == 0 && cantGraficos2 > 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo.Text;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfClsRealizadaPorAsignatura(string idArchivo, string idArchivo2)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;
            decimal cantGraficos2;
            string sp = "sp_DB_T4_ClsRealizadaPorAsignatura_Paginado";
            txtSp.Value = sp;
            lblmsg.Text = string.Empty;
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
                        txtcantGraficos.Value = Convert.ToInt32(-1).ToString();
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

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex.Value) > 0 || Convert.ToInt32(txtIndex.Value) <= Convert.ToInt32(txtcantGraficos.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
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
                            }
                        }
                    }
                    divGrafico.Visible = true;
                    divGrafica1.Visible = true;
                    btnAnterior.Enabled = false;
                }
                else
                {
                    txtIndex.Value = Convert.ToInt32(-1).ToString();
                    divGrafica1.Visible = false;
                    divBtnGrafica.Visible = false;
                }


                // grafico 2
                cant.RemoveRange(0, cant.Count);
                docente.RemoveRange(0, docente.Count);
                cantGraficos2 = 0;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DB_ClsRealizadaPorAsignatura", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos2 = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos2.Value = Convert.ToInt32(-1).ToString();
                        txtIndex2.Value = string.Empty;
                    }
                }

                if (cantGraficos2 > 0)
                {
                    if (cantGraficos2.ToString().Equals("1"))
                    {
                        divBtnGrafica2.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica2.Visible = true;
                    }

                    txtcantGraficos2.Value = cantGraficos2.ToString();
                    txtIndex2.Value = 1.ToString();

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex2.Value) > 0 || Convert.ToInt32(txtIndex2.Value) <= Convert.ToInt32(txtcantGraficos2.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex2.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {

                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                }
                                rdr.Close();
                                sql.Close();

                                ActualizaContador2();
                                if (txtcantGraficos2.Value.Equals(txtIndex2.Value))
                                {
                                    btnSiguiente2.Enabled = false;
                                }
                                else
                                {
                                    btnSiguiente2.Enabled = true;
                                }

                                ChartBarra2.Series[0].Points.DataBindXY(docente, cant);
                                divGrafico.Visible = true;
                            }
                        }
                    }

                    divGrafico.Visible = true;
                    divGrafica2.Visible = true;
                    btnAnterior2.Enabled = false;
                }
                else
                {
                    txtIndex2.Value = Convert.ToInt32(-1).ToString();
                    divGrafica2.Visible = false;
                    divBtnGrafica2.Visible = false;
                }

                if (cantGraficos == 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar los gráficos";
                }
                if (cantGraficos > 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo2.Text;
                }
                if (cantGraficos == 0 && cantGraficos2 > 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo.Text;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfClsRealizadaPorJornada(string idArchivo, string idArchivo2)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;
            decimal cantGraficos2;
            string sp = "sp_DB_T4_ClsRealizadaPorJornada_Paginado";
            txtSp.Value = sp;
            lblmsg.Text = string.Empty;
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
                        txtcantGraficos.Value = Convert.ToInt32(-1).ToString();
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

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex.Value) > 0 || Convert.ToInt32(txtIndex.Value) <= Convert.ToInt32(txtcantGraficos.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
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
                            }
                        }
                    }
                    divGrafico.Visible = true;
                    divGrafica1.Visible = true;
                    btnAnterior.Enabled = false;
                }
                else
                {
                    txtIndex.Value = Convert.ToInt32(-1).ToString();
                    divGrafica1.Visible = false;
                    divBtnGrafica.Visible = false;
                }


                // grafico 2
                cant.RemoveRange(0, cant.Count);
                docente.RemoveRange(0, docente.Count);
                cantGraficos2 = 0;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DB_ClsRealizadaPorJornada", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos2 = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos2.Value = Convert.ToInt32(-1).ToString();
                        txtIndex2.Value = string.Empty;
                    }
                }

                if (cantGraficos2 > 0)
                {
                    if (cantGraficos2.ToString().Equals("1"))
                    {
                        divBtnGrafica2.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica2.Visible = true;
                    }

                    txtcantGraficos2.Value = cantGraficos2.ToString();
                    txtIndex2.Value = 1.ToString();

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex2.Value) > 0 || Convert.ToInt32(txtIndex2.Value) <= Convert.ToInt32(txtcantGraficos2.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex2.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {

                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                }
                                rdr.Close();
                                sql.Close();

                                ActualizaContador2();
                                if (txtcantGraficos2.Value.Equals(txtIndex2.Value))
                                {
                                    btnSiguiente2.Enabled = false;
                                }
                                else
                                {
                                    btnSiguiente2.Enabled = true;
                                }

                                ChartBarra2.Series[0].Points.DataBindXY(docente, cant);
                                divGrafico.Visible = true;
                            }
                        }
                    }

                    divGrafico.Visible = true;
                    divGrafica2.Visible = true;
                    btnAnterior2.Enabled = false;
                }
                else
                {
                    txtIndex2.Value = Convert.ToInt32(-1).ToString();
                    divGrafica2.Visible = false;
                    divBtnGrafica2.Visible = false;
                }

                if (cantGraficos == 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar los gráficos";
                }
                if (cantGraficos > 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo2.Text;
                }
                if (cantGraficos == 0 && cantGraficos2 > 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo.Text;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Clase no realizada 
        private void GrfClsNoRealizadaPorDocente(string idArchivo, string idArchivo2)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;
            decimal cantGraficos2;
            string sp = "sp_DB_T4_ClsNoRealizadaPorDocente_Paginado";
            txtSp.Value = sp;
            lblmsg.Text = string.Empty;
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
                        txtcantGraficos.Value = Convert.ToInt32(-1).ToString();
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

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex.Value) > 0 || Convert.ToInt32(txtIndex.Value) <= Convert.ToInt32(txtcantGraficos.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
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
                            }
                        }
                    }
                    divGrafico.Visible = true;
                    divGrafica1.Visible = true;
                    btnAnterior.Enabled = false;
                }
                else
                {
                    txtIndex.Value = Convert.ToInt32(-1).ToString();
                    divGrafica1.Visible = false;
                    divBtnGrafica.Visible = false;
                }


                // grafico 2
                cant.RemoveRange(0, cant.Count);
                docente.RemoveRange(0, docente.Count);
                cantGraficos2 = 0;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DB_ClsNoRealizadaPorDocente", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos2 = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos2.Value = Convert.ToInt32(-1).ToString();
                        txtIndex2.Value = string.Empty;
                    }
                }

                if (cantGraficos2 > 0)
                {
                    if (cantGraficos2.ToString().Equals("1"))
                    {
                        divBtnGrafica2.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica2.Visible = true;
                    }

                    txtcantGraficos2.Value = cantGraficos2.ToString();
                    txtIndex2.Value = 1.ToString();

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex2.Value) > 0 || Convert.ToInt32(txtIndex2.Value) <= Convert.ToInt32(txtcantGraficos2.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex2.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {

                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                }
                                rdr.Close();
                                sql.Close();

                                ActualizaContador2();
                                if (txtcantGraficos2.Value.Equals(txtIndex2.Value))
                                {
                                    btnSiguiente2.Enabled = false;
                                }
                                else
                                {
                                    btnSiguiente2.Enabled = true;
                                }

                                ChartBarra2.Series[0].Points.DataBindXY(docente, cant);
                                divGrafico.Visible = true;
                            }
                        }
                    }

                    divGrafico.Visible = true;
                    divGrafica2.Visible = true;
                    btnAnterior2.Enabled = false;
                }
                else
                {
                    txtIndex2.Value = Convert.ToInt32(-1).ToString();
                    divGrafica2.Visible = false;              
                    divBtnGrafica2.Visible = false;
                }

                if(cantGraficos == 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar los gráficos";
                }
                if (cantGraficos > 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo2.Text;
                }
                if (cantGraficos == 0 && cantGraficos2 > 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo.Text;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfClsNoRealizadaPorFecha(string idArchivo, string idArchivo2)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;
            decimal cantGraficos2;
            string sp = "sp_DB_T1_ClsNoRealizadaPorFecha_Paginado";
            txtSp.Value = sp;
            lblmsg.Text = string.Empty;
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
                        txtcantGraficos.Value = Convert.ToInt32(-1).ToString();
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

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex.Value) > 0 || Convert.ToInt32(txtIndex.Value) <= Convert.ToInt32(txtcantGraficos.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
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
                            }
                        }
                    }
                    divGrafico.Visible = true;
                    divGrafica1.Visible = true;
                    btnAnterior.Enabled = false;
                }
                else
                {
                    txtIndex.Value = Convert.ToInt32(-1).ToString();
                    divGrafica1.Visible = false;
                    divBtnGrafica.Visible = false;
                }


                // grafico 2
                cant.RemoveRange(0, cant.Count);
                docente.RemoveRange(0, docente.Count);
                cantGraficos2 = 0;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DB_ClsNoRealizadaPorFecha", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos2 = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos2.Value = Convert.ToInt32(-1).ToString();
                        txtIndex2.Value = string.Empty;
                    }
                }

                if (cantGraficos2 > 0)
                {
                    if (cantGraficos2.ToString().Equals("1"))
                    {
                        divBtnGrafica2.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica2.Visible = true;
                    }

                    txtcantGraficos2.Value = cantGraficos2.ToString();
                    txtIndex2.Value = 1.ToString();

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex2.Value) > 0 || Convert.ToInt32(txtIndex2.Value) <= Convert.ToInt32(txtcantGraficos2.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex2.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {

                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                }
                                rdr.Close();
                                sql.Close();

                                ActualizaContador2();
                                if (txtcantGraficos2.Value.Equals(txtIndex2.Value))
                                {
                                    btnSiguiente2.Enabled = false;
                                }
                                else
                                {
                                    btnSiguiente2.Enabled = true;
                                }

                                ChartBarra2.Series[0].Points.DataBindXY(docente, cant);
                                divGrafico.Visible = true;
                            }
                        }
                    }

                    divGrafico.Visible = true;
                    divGrafica2.Visible = true;
                    btnAnterior2.Enabled = false;
                }
                else
                {
                    txtIndex2.Value = Convert.ToInt32(-1).ToString();
                    divGrafica2.Visible = false;
                    divBtnGrafica2.Visible = false;
                }

                if (cantGraficos == 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar los gráficos";
                }
                if (cantGraficos > 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo2.Text;
                }
                if (cantGraficos == 0 && cantGraficos2 > 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo.Text;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfClsNoRealizadaPorAsignatura(string idArchivo, string idArchivo2)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;
            decimal cantGraficos2;
            string sp = "sp_DB_T4_ClsNoRealizadaPorAsignatura_Paginado";
            txtSp.Value = sp;
            lblmsg.Text = string.Empty;
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
                        txtcantGraficos.Value = Convert.ToInt32(-1).ToString();
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

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex.Value) > 0 || Convert.ToInt32(txtIndex.Value) <= Convert.ToInt32(txtcantGraficos.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
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
                            }
                        }
                    }
                    divGrafico.Visible = true;
                    divGrafica1.Visible = true;
                    btnAnterior.Enabled = false;
                }
                else
                {
                    txtIndex.Value = Convert.ToInt32(-1).ToString();
                    divGrafica1.Visible = false;
                    divBtnGrafica.Visible = false;
                }


                // grafico 2
                cant.RemoveRange(0, cant.Count);
                docente.RemoveRange(0, docente.Count);
                cantGraficos2 = 0;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DB_ClsNoRealizadaPorAsignatura", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos2 = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos2.Value = Convert.ToInt32(-1).ToString();
                        txtIndex2.Value = string.Empty;
                    }
                }

                if (cantGraficos2 > 0)
                {
                    if (cantGraficos2.ToString().Equals("1"))
                    {
                        divBtnGrafica2.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica2.Visible = true;
                    }

                    txtcantGraficos2.Value = cantGraficos2.ToString();
                    txtIndex2.Value = 1.ToString();

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex2.Value) > 0 || Convert.ToInt32(txtIndex2.Value) <= Convert.ToInt32(txtcantGraficos2.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex2.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {

                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                }
                                rdr.Close();
                                sql.Close();

                                ActualizaContador2();
                                if (txtcantGraficos2.Value.Equals(txtIndex2.Value))
                                {
                                    btnSiguiente2.Enabled = false;
                                }
                                else
                                {
                                    btnSiguiente2.Enabled = true;
                                }

                                ChartBarra2.Series[0].Points.DataBindXY(docente, cant);
                                divGrafico.Visible = true;
                            }
                        }
                    }

                    divGrafico.Visible = true;
                    divGrafica2.Visible = true;
                    btnAnterior2.Enabled = false;
                }
                else
                {
                    txtIndex2.Value = Convert.ToInt32(-1).ToString();
                    divGrafica2.Visible = false;
                    divBtnGrafica2.Visible = false;
                }

                if (cantGraficos == 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar los gráficos";
                }
                if (cantGraficos > 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo2.Text;
                }
                if (cantGraficos == 0 && cantGraficos2 > 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo.Text;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfClsNoRealizadaPorJornada(string idArchivo, string idArchivo2)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;
            decimal cantGraficos2;
            string sp = "sp_DB_T4_ClsNoRealizadaPorJornada_Paginado";
            txtSp.Value = sp;
            lblmsg.Text = string.Empty;
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
                        txtcantGraficos.Value = Convert.ToInt32(-1).ToString();
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

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex.Value) > 0 || Convert.ToInt32(txtIndex.Value) <= Convert.ToInt32(txtcantGraficos.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
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
                            }
                        }
                    }
                    divGrafico.Visible = true;
                    divGrafica1.Visible = true;
                    btnAnterior.Enabled = false;
                }
                else
                {
                    txtIndex.Value = Convert.ToInt32(-1).ToString();
                    divGrafica1.Visible = false;
                    divBtnGrafica.Visible = false;
                }


                // grafico 2
                cant.RemoveRange(0, cant.Count);
                docente.RemoveRange(0, docente.Count);
                cantGraficos2 = 0;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DB_ClsNoRealizadaPorJornada", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos2 = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos2.Value = Convert.ToInt32(-1).ToString();
                        txtIndex2.Value = string.Empty;
                    }
                }

                if (cantGraficos2 > 0)
                {
                    if (cantGraficos2.ToString().Equals("1"))
                    {
                        divBtnGrafica2.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica2.Visible = true;
                    }

                    txtcantGraficos2.Value = cantGraficos2.ToString();
                    txtIndex2.Value = 1.ToString();

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex2.Value) > 0 || Convert.ToInt32(txtIndex2.Value) <= Convert.ToInt32(txtcantGraficos2.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex2.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {

                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                }
                                rdr.Close();
                                sql.Close();

                                ActualizaContador2();
                                if (txtcantGraficos2.Value.Equals(txtIndex2.Value))
                                {
                                    btnSiguiente2.Enabled = false;
                                }
                                else
                                {
                                    btnSiguiente2.Enabled = true;
                                }

                                ChartBarra2.Series[0].Points.DataBindXY(docente, cant);
                                divGrafico.Visible = true;
                            }
                        }
                    }

                    divGrafico.Visible = true;
                    divGrafica2.Visible = true;
                    btnAnterior2.Enabled = false;
                }
                else
                {
                    txtIndex2.Value = Convert.ToInt32(-1).ToString();
                    divGrafica2.Visible = false;
                    divBtnGrafica2.Visible = false;
                }

                if (cantGraficos == 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar los gráficos";
                }
                if (cantGraficos > 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo2.Text;
                }
                if (cantGraficos == 0 && cantGraficos2 > 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo.Text;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Libro no marcado al devolverlo
        private void GrfLibroNoMarcadoPorDocente(string idArchivo, string idArchivo2)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;
            decimal cantGraficos2;
            string sp = "sp_DB_T4_LibroNoMarcadoPorDocente_Paginado";
            txtSp.Value = sp;
            lblmsg.Text = string.Empty;
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
                        txtcantGraficos.Value = Convert.ToInt32(-1).ToString();
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

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex.Value) > 0 || Convert.ToInt32(txtIndex.Value) <= Convert.ToInt32(txtcantGraficos.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
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
                            }
                        }
                    }
                    divGrafico.Visible = true;
                    divGrafica1.Visible = true;
                    btnAnterior.Enabled = false;
                }
                else
                {
                    txtIndex.Value = Convert.ToInt32(-1).ToString();
                    divGrafica1.Visible = false;
                    divBtnGrafica.Visible = false;
                }


                // grafico 2
                cant.RemoveRange(0, cant.Count);
                docente.RemoveRange(0, docente.Count);
                cantGraficos2 = 0;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DB_LibroNoMarcadoPorDocente", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos2 = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos2.Value = Convert.ToInt32(-1).ToString();
                        txtIndex2.Value = string.Empty;
                    }
                }

                if (cantGraficos2 > 0)
                {
                    if (cantGraficos2.ToString().Equals("1"))
                    {
                        divBtnGrafica2.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica2.Visible = true;
                    }

                    txtcantGraficos2.Value = cantGraficos2.ToString();
                    txtIndex2.Value = 1.ToString();

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex2.Value) > 0 || Convert.ToInt32(txtIndex2.Value) <= Convert.ToInt32(txtcantGraficos2.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex2.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {

                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                }
                                rdr.Close();
                                sql.Close();

                                ActualizaContador2();
                                if (txtcantGraficos2.Value.Equals(txtIndex2.Value))
                                {
                                    btnSiguiente2.Enabled = false;
                                }
                                else
                                {
                                    btnSiguiente2.Enabled = true;
                                }

                                ChartBarra2.Series[0].Points.DataBindXY(docente, cant);
                                divGrafico.Visible = true;
                            }
                        }
                    }

                    divGrafico.Visible = true;
                    divGrafica2.Visible = true;
                    btnAnterior2.Enabled = false;
                }
                else
                {
                    txtIndex2.Value = Convert.ToInt32(-1).ToString();
                    divGrafica2.Visible = false;
                    divBtnGrafica2.Visible = false;
                }

                if (cantGraficos == 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar los gráficos";
                }
                if (cantGraficos > 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo2.Text;
                }
                if (cantGraficos == 0 && cantGraficos2 > 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo.Text;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfLibroNoMarcadoPorFecha(string idArchivo, string idArchivo2)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;
            decimal cantGraficos2;
            string sp = "sp_DB_T1_LibroNoMarcadoPorFecha_Paginado";
            txtSp.Value = sp;
            lblmsg.Text = string.Empty;
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
                        txtcantGraficos.Value = Convert.ToInt32(-1).ToString();
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

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex.Value) > 0 || Convert.ToInt32(txtIndex.Value) <= Convert.ToInt32(txtcantGraficos.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
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
                            }
                        }
                    }
                    divGrafico.Visible = true;
                    divGrafica1.Visible = true;
                    btnAnterior.Enabled = false;
                }
                else
                {
                    txtIndex.Value = Convert.ToInt32(-1).ToString();
                    divGrafica1.Visible = false;
                    divBtnGrafica.Visible = false;
                }


                // grafico 2
                cant.RemoveRange(0, cant.Count);
                docente.RemoveRange(0, docente.Count);
                cantGraficos2 = 0;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DB_LibroNoMarcadoPorFecha", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos2 = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos2.Value = Convert.ToInt32(-1).ToString();
                        txtIndex2.Value = string.Empty;
                    }
                }

                if (cantGraficos2 > 0)
                {
                    if (cantGraficos2.ToString().Equals("1"))
                    {
                        divBtnGrafica2.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica2.Visible = true;
                    }

                    txtcantGraficos2.Value = cantGraficos2.ToString();
                    txtIndex2.Value = 1.ToString();

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex2.Value) > 0 || Convert.ToInt32(txtIndex2.Value) <= Convert.ToInt32(txtcantGraficos2.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex2.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {

                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                }
                                rdr.Close();
                                sql.Close();

                                ActualizaContador2();
                                if (txtcantGraficos2.Value.Equals(txtIndex2.Value))
                                {
                                    btnSiguiente2.Enabled = false;
                                }
                                else
                                {
                                    btnSiguiente2.Enabled = true;
                                }

                                ChartBarra2.Series[0].Points.DataBindXY(docente, cant);
                                divGrafico.Visible = true;
                            }
                        }
                    }

                    divGrafico.Visible = true;
                    divGrafica2.Visible = true;
                    btnAnterior2.Enabled = false;
                }
                else
                {
                    txtIndex2.Value = Convert.ToInt32(-1).ToString();
                    divGrafica2.Visible = false;
                    divBtnGrafica2.Visible = false;
                }

                if (cantGraficos == 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar los gráficos";
                }
                if (cantGraficos > 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo2.Text;
                }
                if (cantGraficos == 0 && cantGraficos2 > 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo.Text;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfLibroNoMarcadoPorAsignatura(string idArchivo, string idArchivo2)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;
            decimal cantGraficos2;
            string sp = "sp_DB_T4_LibroNoMarcadoPorAsignatura_Paginado";
            txtSp.Value = sp;
            lblmsg.Text = string.Empty;
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
                        txtcantGraficos.Value = Convert.ToInt32(-1).ToString();
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

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex.Value) > 0 || Convert.ToInt32(txtIndex.Value) <= Convert.ToInt32(txtcantGraficos.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
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
                            }
                        }
                    }
                    divGrafico.Visible = true;
                    divGrafica1.Visible = true;
                    btnAnterior.Enabled = false;
                }
                else
                {
                    txtIndex.Value = Convert.ToInt32(-1).ToString();
                    divGrafica1.Visible = false;
                    divBtnGrafica.Visible = false;
                }


                // grafico 2
                cant.RemoveRange(0, cant.Count);
                docente.RemoveRange(0, docente.Count);
                cantGraficos2 = 0;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DB_LibroNoMarcadoPorAsignatura", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos2 = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos2.Value = Convert.ToInt32(-1).ToString();
                        txtIndex2.Value = string.Empty;
                    }
                }

                if (cantGraficos2 > 0)
                {
                    if (cantGraficos2.ToString().Equals("1"))
                    {
                        divBtnGrafica2.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica2.Visible = true;
                    }

                    txtcantGraficos2.Value = cantGraficos2.ToString();
                    txtIndex2.Value = 1.ToString();

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex2.Value) > 0 || Convert.ToInt32(txtIndex2.Value) <= Convert.ToInt32(txtcantGraficos2.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex2.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {

                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                }
                                rdr.Close();
                                sql.Close();

                                ActualizaContador2();
                                if (txtcantGraficos2.Value.Equals(txtIndex2.Value))
                                {
                                    btnSiguiente2.Enabled = false;
                                }
                                else
                                {
                                    btnSiguiente2.Enabled = true;
                                }

                                ChartBarra2.Series[0].Points.DataBindXY(docente, cant);
                                divGrafico.Visible = true;
                            }
                        }
                    }

                    divGrafico.Visible = true;
                    divGrafica2.Visible = true;
                    btnAnterior2.Enabled = false;
                }
                else
                {
                    txtIndex2.Value = Convert.ToInt32(-1).ToString();
                    divGrafica2.Visible = false;
                    divBtnGrafica2.Visible = false;
                }

                if (cantGraficos == 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar los gráficos";
                }
                if (cantGraficos > 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo2.Text;
                }
                if (cantGraficos == 0 && cantGraficos2 > 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo.Text;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfLibroNoMarcadoPorJornada(string idArchivo, string idArchivo2)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;
            decimal cantGraficos2;
            string sp = "sp_DB_T4_LibroNoMarcadoPorJornada_Paginado";
            txtSp.Value = sp;
            lblmsg.Text = string.Empty;
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
                        txtcantGraficos.Value = Convert.ToInt32(-1).ToString();
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

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex.Value) > 0 || Convert.ToInt32(txtIndex.Value) <= Convert.ToInt32(txtcantGraficos.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
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
                            }
                        }
                    }
                    divGrafico.Visible = true;
                    divGrafica1.Visible = true;
                    btnAnterior.Enabled = false;
                }
                else
                {
                    txtIndex.Value = Convert.ToInt32(-1).ToString();
                    divGrafica1.Visible = false;
                    divBtnGrafica.Visible = false;
                }


                // grafico 2
                cant.RemoveRange(0, cant.Count);
                docente.RemoveRange(0, docente.Count);
                cantGraficos2 = 0;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DB_LibroNoMarcadoPorJornada", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos2 = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos2.Value = Convert.ToInt32(-1).ToString();
                        txtIndex2.Value = string.Empty;
                    }
                }

                if (cantGraficos2 > 0)
                {
                    if (cantGraficos2.ToString().Equals("1"))
                    {
                        divBtnGrafica2.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica2.Visible = true;
                    }

                    txtcantGraficos2.Value = cantGraficos2.ToString();
                    txtIndex2.Value = 1.ToString();

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex2.Value) > 0 || Convert.ToInt32(txtIndex2.Value) <= Convert.ToInt32(txtcantGraficos2.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex2.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {

                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                }
                                rdr.Close();
                                sql.Close();

                                ActualizaContador2();
                                if (txtcantGraficos2.Value.Equals(txtIndex2.Value))
                                {
                                    btnSiguiente2.Enabled = false;
                                }
                                else
                                {
                                    btnSiguiente2.Enabled = true;
                                }

                                ChartBarra2.Series[0].Points.DataBindXY(docente, cant);
                                divGrafico.Visible = true;
                            }
                        }
                    }

                    divGrafico.Visible = true;
                    divGrafica2.Visible = true;
                    btnAnterior2.Enabled = false;
                }
                else
                {
                    txtIndex2.Value = Convert.ToInt32(-1).ToString();
                    divGrafica2.Visible = false;
                    divBtnGrafica2.Visible = false;
                }

                if (cantGraficos == 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar los gráficos";
                }
                if (cantGraficos > 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo2.Text;
                }
                if (cantGraficos == 0 && cantGraficos2 > 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo.Text;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Referencia porcentual de alumnos asistentes
        private void GrfPorcentajealumnosExistentesPorAsignatura(string idArchivo, string idArchivo2)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;
            decimal cantGraficos2;
            string sp = "sp_DB_T3_PorcentajePorAsignatura_Paginado";
            txtSp.Value = sp;
            lblmsg.Text = string.Empty;
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
                        txtcantGraficos.Value = Convert.ToInt32(-1).ToString();
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

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex.Value) > 0 || Convert.ToInt32(txtIndex.Value) <= Convert.ToInt32(txtcantGraficos.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
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
                            }
                        }
                    }
                    divGrafico.Visible = true;
                    divGrafica1.Visible = true;
                    btnAnterior.Enabled = false;
                }
                else
                {
                    txtIndex.Value = Convert.ToInt32(-1).ToString();
                    divGrafica1.Visible = false;
                    divBtnGrafica.Visible = false;
                }


                // grafico 2
                cant.RemoveRange(0, cant.Count);
                docente.RemoveRange(0, docente.Count);
                cantGraficos2 = 0;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_PorcentajePorAsignatura", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos2 = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos2.Value = Convert.ToInt32(-1).ToString();
                        txtIndex2.Value = string.Empty;
                    }
                }

                if (cantGraficos2 > 0)
                {
                    if (cantGraficos2.ToString().Equals("1"))
                    {
                        divBtnGrafica2.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica2.Visible = true;
                    }

                    txtcantGraficos2.Value = cantGraficos2.ToString();
                    txtIndex2.Value = 1.ToString();

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex2.Value) > 0 || Convert.ToInt32(txtIndex2.Value) <= Convert.ToInt32(txtcantGraficos2.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex2.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {

                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                }
                                rdr.Close();
                                sql.Close();

                                ActualizaContador2();
                                if (txtcantGraficos2.Value.Equals(txtIndex2.Value))
                                {
                                    btnSiguiente2.Enabled = false;
                                }
                                else
                                {
                                    btnSiguiente2.Enabled = true;
                                }

                                ChartBarra2.Series[0].Points.DataBindXY(docente, cant);
                                divGrafico.Visible = true;
                            }
                        }
                    }

                    divGrafico.Visible = true;
                    divGrafica2.Visible = true;
                    btnAnterior2.Enabled = false;
                }
                else
                {
                    txtIndex2.Value = Convert.ToInt32(-1).ToString();
                    divGrafica2.Visible = false;
                    divBtnGrafica2.Visible = false;
                }

                if (cantGraficos == 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar los gráficos";
                }
                if (cantGraficos > 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo2.Text;
                }
                if (cantGraficos == 0 && cantGraficos2 > 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo.Text;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfPorcentajealumnosExistentesPorFecha(string idArchivo, string idArchivo2)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;
            decimal cantGraficos2;
            string sp = "sp_DB_T2_PorcentajePorFecha_Paginado";
            txtSp.Value = sp;
            lblmsg.Text = string.Empty;
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
                        txtcantGraficos.Value = Convert.ToInt32(-1).ToString();
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

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex.Value) > 0 || Convert.ToInt32(txtIndex.Value) <= Convert.ToInt32(txtcantGraficos.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
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
                            }
                        }
                    }
                    divGrafico.Visible = true;
                    divGrafica1.Visible = true;
                    btnAnterior.Enabled = false;
                }
                else
                {
                    txtIndex.Value = Convert.ToInt32(-1).ToString();
                    divGrafica1.Visible = false;
                    divBtnGrafica.Visible = false;
                }


                // grafico 2
                cant.RemoveRange(0, cant.Count);
                docente.RemoveRange(0, docente.Count);
                cantGraficos2 = 0;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_PorcentajePorFecha", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos2 = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos2.Value = Convert.ToInt32(-1).ToString();
                        txtIndex2.Value = string.Empty;
                    }
                }

                if (cantGraficos2 > 0)
                {
                    if (cantGraficos2.ToString().Equals("1"))
                    {
                        divBtnGrafica2.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica2.Visible = true;
                    }

                    txtcantGraficos2.Value = cantGraficos2.ToString();
                    txtIndex2.Value = 1.ToString();

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex2.Value) > 0 || Convert.ToInt32(txtIndex2.Value) <= Convert.ToInt32(txtcantGraficos2.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex2.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {

                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                }
                                rdr.Close();
                                sql.Close();

                                ActualizaContador2();
                                if (txtcantGraficos2.Value.Equals(txtIndex2.Value))
                                {
                                    btnSiguiente2.Enabled = false;
                                }
                                else
                                {
                                    btnSiguiente2.Enabled = true;
                                }

                                ChartBarra2.Series[0].Points.DataBindXY(docente, cant);
                                divGrafico.Visible = true;
                            }
                        }
                    }

                    divGrafico.Visible = true;
                    divGrafica2.Visible = true;
                    btnAnterior2.Enabled = false;
                }
                else
                {
                    txtIndex2.Value = Convert.ToInt32(-1).ToString();
                    divGrafica2.Visible = false;
                    divBtnGrafica2.Visible = false;
                }

                if (cantGraficos == 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar los gráficos";
                }
                if (cantGraficos > 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo2.Text;
                }
                if (cantGraficos == 0 && cantGraficos2 > 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo.Text;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfPorcentajealumnosExistentesPorJornada(string idArchivo, string idArchivo2)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;
            decimal cantGraficos2;
            string sp = "sp_DB_T3_PorcentajePorJornada_Paginado";
            txtSp.Value = sp;
            lblmsg.Text = string.Empty;
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
                        txtcantGraficos.Value = Convert.ToInt32(-1).ToString();
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

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex.Value) > 0 || Convert.ToInt32(txtIndex.Value) <= Convert.ToInt32(txtcantGraficos.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
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
                            }
                        }
                    }
                    divGrafico.Visible = true;
                    divGrafica1.Visible = true;
                    btnAnterior.Enabled = false;
                }
                else
                {
                    txtIndex.Value = Convert.ToInt32(-1).ToString();
                    divGrafica1.Visible = false;
                    divBtnGrafica.Visible = false;
                }


                // grafico 2
                cant.RemoveRange(0, cant.Count);
                docente.RemoveRange(0, docente.Count);
                cantGraficos2 = 0;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_PorcentajePorJornada", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos2 = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos2.Value = Convert.ToInt32(-1).ToString();
                        txtIndex2.Value = string.Empty;
                    }
                }

                if (cantGraficos2 > 0)
                {
                    if (cantGraficos2.ToString().Equals("1"))
                    {
                        divBtnGrafica2.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica2.Visible = true;
                    }

                    txtcantGraficos2.Value = cantGraficos2.ToString();
                    txtIndex2.Value = 1.ToString();

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex2.Value) > 0 || Convert.ToInt32(txtIndex2.Value) <= Convert.ToInt32(txtcantGraficos2.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex2.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {

                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                }
                                rdr.Close();
                                sql.Close();

                                ActualizaContador2();
                                if (txtcantGraficos2.Value.Equals(txtIndex2.Value))
                                {
                                    btnSiguiente2.Enabled = false;
                                }
                                else
                                {
                                    btnSiguiente2.Enabled = true;
                                }

                                ChartBarra2.Series[0].Points.DataBindXY(docente, cant);
                                divGrafico.Visible = true;
                            }
                        }
                    }

                    divGrafico.Visible = true;
                    divGrafica2.Visible = true;
                    btnAnterior2.Enabled = false;
                }
                else
                {
                    txtIndex2.Value = Convert.ToInt32(-1).ToString();
                    divGrafica2.Visible = false;
                    divBtnGrafica2.Visible = false;
                }

                if (cantGraficos == 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar los gráficos";
                }
                if (cantGraficos > 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo2.Text;
                }
                if (cantGraficos == 0 && cantGraficos2 > 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo.Text;
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

        private void GrfAtrasoRetiroLibroPorDocente(string idArchivo, string idArchivo2)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;
            decimal cantGraficos2;
            string sp = "sp_DB_T4_AtrasoRetiroLibroPorDocente_Paginado";
            txtSp.Value = sp;
            lblmsg.Text = string.Empty;
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
                        txtcantGraficos.Value = Convert.ToInt32(-1).ToString();
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

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex.Value) > 0 || Convert.ToInt32(txtIndex.Value) <= Convert.ToInt32(txtcantGraficos.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
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
                            }
                        }
                    }
                    divGrafico.Visible = true;
                    divGrafica1.Visible = true;
                    btnAnterior.Enabled = false;
                }
                else
                {
                    txtIndex.Value = Convert.ToInt32(-1).ToString();
                    divGrafica1.Visible = false;
                    divBtnGrafica.Visible = false;
                }


                // grafico 2
                cant.RemoveRange(0, cant.Count);
                docente.RemoveRange(0, docente.Count);
                cantGraficos2 = 0;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DatosAtrasoLibroClasesPorDocente", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos2 = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos2.Value = Convert.ToInt32(-1).ToString();
                        txtIndex2.Value = string.Empty;
                    }
                }

                if (cantGraficos2 > 0)
                {
                    if (cantGraficos2.ToString().Equals("1"))
                    {
                        divBtnGrafica2.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica2.Visible = true;
                    }

                    txtcantGraficos2.Value = cantGraficos2.ToString();
                    txtIndex2.Value = 1.ToString();

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex2.Value) > 0 || Convert.ToInt32(txtIndex2.Value) <= Convert.ToInt32(txtcantGraficos2.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex2.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {

                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                }
                                rdr.Close();
                                sql.Close();

                                ActualizaContador2();
                                if (txtcantGraficos2.Value.Equals(txtIndex2.Value))
                                {
                                    btnSiguiente2.Enabled = false;
                                }
                                else
                                {
                                    btnSiguiente2.Enabled = true;
                                }

                                ChartBarra2.Series[0].Points.DataBindXY(docente, cant);
                                divGrafico.Visible = true;
                            }
                        }
                    }

                    divGrafico.Visible = true;
                    divGrafica2.Visible = true;
                    btnAnterior2.Enabled = false;
                }
                else
                {
                    txtIndex2.Value = Convert.ToInt32(-1).ToString();
                    divGrafica2.Visible = false;
                    divBtnGrafica2.Visible = false;
                }

                if (cantGraficos == 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar los gráficos";
                }
                if (cantGraficos > 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo2.Text;
                }
                if (cantGraficos == 0 && cantGraficos2 > 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo.Text;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfAtrasoRetiroLibroPorFecha(string idArchivo, string idArchivo2)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;
            decimal cantGraficos2;
            string sp = "sp_DB_T1_AtrasoRetiroLibroPorFecha_Paginado";
            txtSp.Value = sp;
            lblmsg.Text = string.Empty;
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
                        txtcantGraficos.Value = Convert.ToInt32(-1).ToString();
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

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex.Value) > 0 || Convert.ToInt32(txtIndex.Value) <= Convert.ToInt32(txtcantGraficos.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
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
                            }
                        }
                    }
                    divGrafico.Visible = true;
                    divGrafica1.Visible = true;
                    btnAnterior.Enabled = false;
                }
                else
                {
                    txtIndex.Value = Convert.ToInt32(-1).ToString();
                    divGrafica1.Visible = false;
                    divBtnGrafica.Visible = false;
                }


                // grafico 2
                cant.RemoveRange(0, cant.Count);
                docente.RemoveRange(0, docente.Count);
                cantGraficos2 = 0;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DatosAtrasoLibroClasesPorFecha", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos2 = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos2.Value = Convert.ToInt32(-1).ToString();
                        txtIndex2.Value = string.Empty;
                    }
                }

                if (cantGraficos2 > 0)
                {
                    if (cantGraficos2.ToString().Equals("1"))
                    {
                        divBtnGrafica2.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica2.Visible = true;
                    }

                    txtcantGraficos2.Value = cantGraficos2.ToString();
                    txtIndex2.Value = 1.ToString();

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex2.Value) > 0 || Convert.ToInt32(txtIndex2.Value) <= Convert.ToInt32(txtcantGraficos2.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex2.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {

                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                }
                                rdr.Close();
                                sql.Close();

                                ActualizaContador2();
                                if (txtcantGraficos2.Value.Equals(txtIndex2.Value))
                                {
                                    btnSiguiente2.Enabled = false;
                                }
                                else
                                {
                                    btnSiguiente2.Enabled = true;
                                }

                                ChartBarra2.Series[0].Points.DataBindXY(docente, cant);
                                divGrafico.Visible = true;
                            }
                        }
                    }

                    divGrafico.Visible = true;
                    divGrafica2.Visible = true;
                    btnAnterior2.Enabled = false;
                }
                else
                {
                    txtIndex2.Value = Convert.ToInt32(-1).ToString();
                    divGrafica2.Visible = false;
                    divBtnGrafica2.Visible = false;
                }

                if (cantGraficos == 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar los gráficos";
                }
                if (cantGraficos > 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo2.Text;
                }
                if (cantGraficos == 0 && cantGraficos2 > 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo.Text;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfAtrasoRetiroLibroPorAsignatura(string idArchivo, string idArchivo2)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;
            decimal cantGraficos2;
            string sp = "sp_DB_T4_AtrasoRetiroLibroPorAsignatura_Paginado";
            txtSp.Value = sp;
            lblmsg.Text = string.Empty;
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DatosAtrasoLibroClasesPorAsignatura", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos.Value = Convert.ToInt32(-1).ToString();
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

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex.Value) > 0 || Convert.ToInt32(txtIndex.Value) <= Convert.ToInt32(txtcantGraficos.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
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
                            }
                        }
                    }
                    divGrafico.Visible = true;
                    divGrafica1.Visible = true;
                    btnAnterior.Enabled = false;
                }
                else
                {
                    txtIndex.Value = Convert.ToInt32(-1).ToString();
                    divGrafica1.Visible = false;
                    divBtnGrafica.Visible = false;
                }


                // grafico 2
                cant.RemoveRange(0, cant.Count);
                docente.RemoveRange(0, docente.Count);
                cantGraficos2 = 0;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DatosAtrasoLibroClasesPorAsignatura", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos2 = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos2.Value = Convert.ToInt32(-1).ToString();
                        txtIndex2.Value = string.Empty;
                    }
                }

                if (cantGraficos2 > 0)
                {
                    if (cantGraficos2.ToString().Equals("1"))
                    {
                        divBtnGrafica2.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica2.Visible = true;
                    }

                    txtcantGraficos2.Value = cantGraficos2.ToString();
                    txtIndex2.Value = 1.ToString();

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex2.Value) > 0 || Convert.ToInt32(txtIndex2.Value) <= Convert.ToInt32(txtcantGraficos2.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex2.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {

                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                }
                                rdr.Close();
                                sql.Close();

                                ActualizaContador2();
                                if (txtcantGraficos2.Value.Equals(txtIndex2.Value))
                                {
                                    btnSiguiente2.Enabled = false;
                                }
                                else
                                {
                                    btnSiguiente2.Enabled = true;
                                }

                                ChartBarra2.Series[0].Points.DataBindXY(docente, cant);
                                divGrafico.Visible = true;
                            }
                        }
                    }

                    divGrafico.Visible = true;
                    divGrafica2.Visible = true;
                    btnAnterior2.Enabled = false;
                }
                else
                {
                    txtIndex2.Value = Convert.ToInt32(-1).ToString();
                    divGrafica2.Visible = false;
                    divBtnGrafica2.Visible = false;
                }

                if (cantGraficos == 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar los gráficos";
                }
                if (cantGraficos > 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo2.Text;
                }
                if (cantGraficos == 0 && cantGraficos2 > 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo.Text;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfAtrasoRetiroLibroPorJornada(string idArchivo, string idArchivo2)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;
            decimal cantGraficos2;
            string sp = "sp_DB_T4_AtrasoRetiroLibroPorJornada_Paginado";
            txtSp.Value = sp;
            lblmsg.Text = string.Empty;
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
                        txtcantGraficos.Value = Convert.ToInt32(-1).ToString();
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

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex.Value) > 0 || Convert.ToInt32(txtIndex.Value) <= Convert.ToInt32(txtcantGraficos.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
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
                            }
                        }
                    }
                    divGrafico.Visible = true;
                    divGrafica1.Visible = true;
                    btnAnterior.Enabled = false;
                }
                else
                {
                    txtIndex.Value = Convert.ToInt32(-1).ToString();
                    divGrafica1.Visible = false;
                    divBtnGrafica.Visible = false;
                }


                // grafico 2
                cant.RemoveRange(0, cant.Count);
                docente.RemoveRange(0, docente.Count);
                cantGraficos2 = 0;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DatosAtrasoLibroClasesPorJornada", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos2 = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos2.Value = Convert.ToInt32(-1).ToString();
                        txtIndex2.Value = string.Empty;
                    }
                }

                if (cantGraficos2 > 0)
                {
                    if (cantGraficos2.ToString().Equals("1"))
                    {
                        divBtnGrafica2.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica2.Visible = true;
                    }

                    txtcantGraficos2.Value = cantGraficos2.ToString();
                    txtIndex2.Value = 1.ToString();

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex2.Value) > 0 || Convert.ToInt32(txtIndex2.Value) <= Convert.ToInt32(txtcantGraficos2.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex2.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {

                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                }
                                rdr.Close();
                                sql.Close();

                                ActualizaContador2();
                                if (txtcantGraficos2.Value.Equals(txtIndex2.Value))
                                {
                                    btnSiguiente2.Enabled = false;
                                }
                                else
                                {
                                    btnSiguiente2.Enabled = true;
                                }

                                ChartBarra2.Series[0].Points.DataBindXY(docente, cant);
                                divGrafico.Visible = true;
                            }
                        }
                    }

                    divGrafico.Visible = true;
                    divGrafica2.Visible = true;
                    btnAnterior2.Enabled = false;
                }
                else
                {
                    txtIndex2.Value = Convert.ToInt32(-1).ToString();
                    divGrafica2.Visible = false;
                    divBtnGrafica2.Visible = false;
                }

                if (cantGraficos == 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar los gráficos";
                }
                if (cantGraficos > 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo2.Text;
                }
                if (cantGraficos == 0 && cantGraficos2 > 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo.Text;
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
        private void GrfLibroDevueltosAntesPorDocente(string idArchivo, string idArchivo2)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;
            decimal cantGraficos2;
            string sp = "sp_DB_T4_LibroClasesDevueltoAntesPorDocente_Paginado";
            txtSp.Value = sp;
            lblmsg.Text = string.Empty;
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
                        txtcantGraficos.Value = Convert.ToInt32(-1).ToString();
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

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex.Value) > 0 || Convert.ToInt32(txtIndex.Value) <= Convert.ToInt32(txtcantGraficos.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
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
                            }
                        }
                    }
                    divGrafico.Visible = true;
                    divGrafica1.Visible = true;
                    btnAnterior.Enabled = false;
                }
                else
                {
                    txtIndex.Value = Convert.ToInt32(-1).ToString();
                    divGrafica1.Visible = false;
                    divBtnGrafica.Visible = false;
                }


                // grafico 2
                cant.RemoveRange(0, cant.Count);
                docente.RemoveRange(0, docente.Count);
                cantGraficos2 = 0;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DatosLibroClasesDevueltoAntesPorDocente", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos2 = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos2.Value = Convert.ToInt32(-1).ToString();
                        txtIndex2.Value = string.Empty;
                    }
                }

                if (cantGraficos2 > 0)
                {
                    if (cantGraficos2.ToString().Equals("1"))
                    {
                        divBtnGrafica2.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica2.Visible = true;
                    }

                    txtcantGraficos2.Value = cantGraficos2.ToString();
                    txtIndex2.Value = 1.ToString();

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex2.Value) > 0 || Convert.ToInt32(txtIndex2.Value) <= Convert.ToInt32(txtcantGraficos2.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex2.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {

                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                }
                                rdr.Close();
                                sql.Close();

                                ActualizaContador2();
                                if (txtcantGraficos2.Value.Equals(txtIndex2.Value))
                                {
                                    btnSiguiente2.Enabled = false;
                                }
                                else
                                {
                                    btnSiguiente2.Enabled = true;
                                }

                                ChartBarra2.Series[0].Points.DataBindXY(docente, cant);
                                divGrafico.Visible = true;
                            }
                        }
                    }

                    divGrafico.Visible = true;
                    divGrafica2.Visible = true;
                    btnAnterior2.Enabled = false;
                }
                else
                {
                    txtIndex2.Value = Convert.ToInt32(-1).ToString();
                    divGrafica2.Visible = false;
                    divBtnGrafica2.Visible = false;
                }

                if (cantGraficos == 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar los gráficos";
                }
                if (cantGraficos > 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo2.Text;
                }
                if (cantGraficos == 0 && cantGraficos2 > 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo.Text;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfLibroDevueltosAntesPorFecha(string idArchivo, string idArchivo2)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;
            decimal cantGraficos2;
            string sp = "sp_DB_T1_LibroClasesDevueltoAntesPorFecha_Paginado";
            txtSp.Value = sp;
            lblmsg.Text = string.Empty;
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
                        txtcantGraficos.Value = Convert.ToInt32(-1).ToString();
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

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex.Value) > 0 || Convert.ToInt32(txtIndex.Value) <= Convert.ToInt32(txtcantGraficos.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
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
                            }
                        }
                    }
                    divGrafico.Visible = true;
                    divGrafica1.Visible = true;
                    btnAnterior.Enabled = false;
                }
                else
                {
                    txtIndex.Value = Convert.ToInt32(-1).ToString();
                    divGrafica1.Visible = false;
                    divBtnGrafica.Visible = false;
                }


                // grafico 2
                cant.RemoveRange(0, cant.Count);
                docente.RemoveRange(0, docente.Count);
                cantGraficos2 = 0;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DatosLibroClasesDevueltoAntesPorFecha", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos2 = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos2.Value = Convert.ToInt32(-1).ToString();
                        txtIndex2.Value = string.Empty;
                    }
                }

                if (cantGraficos2 > 0)
                {
                    if (cantGraficos2.ToString().Equals("1"))
                    {
                        divBtnGrafica2.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica2.Visible = true;
                    }

                    txtcantGraficos2.Value = cantGraficos2.ToString();
                    txtIndex2.Value = 1.ToString();

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex2.Value) > 0 || Convert.ToInt32(txtIndex2.Value) <= Convert.ToInt32(txtcantGraficos2.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex2.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {

                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                }
                                rdr.Close();
                                sql.Close();

                                ActualizaContador2();
                                if (txtcantGraficos2.Value.Equals(txtIndex2.Value))
                                {
                                    btnSiguiente2.Enabled = false;
                                }
                                else
                                {
                                    btnSiguiente2.Enabled = true;
                                }

                                ChartBarra2.Series[0].Points.DataBindXY(docente, cant);
                                divGrafico.Visible = true;
                            }
                        }
                    }

                    divGrafico.Visible = true;
                    divGrafica2.Visible = true;
                    btnAnterior2.Enabled = false;
                }
                else
                {
                    txtIndex2.Value = Convert.ToInt32(-1).ToString();
                    divGrafica2.Visible = false;
                    divBtnGrafica2.Visible = false;
                }

                if (cantGraficos == 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar los gráficos";
                }
                if (cantGraficos > 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo2.Text;
                }
                if (cantGraficos == 0 && cantGraficos2 > 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo.Text;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfLibroDevueltosAntesPorAsignatura(string idArchivo, string idArchivo2)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;
            decimal cantGraficos2;
            string sp = "sp_DB_T4_LibroClasesDevueltoAntesPorAsignatura_Paginado";
            txtSp.Value = sp;
            lblmsg.Text = string.Empty;
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DatosLibroClasesDevueltoAntesPorAsignatura", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos.Value = Convert.ToInt32(-1).ToString();
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

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex.Value) > 0 || Convert.ToInt32(txtIndex.Value) <= Convert.ToInt32(txtcantGraficos.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
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
                            }
                        }
                    }
                    divGrafico.Visible = true;
                    divGrafica1.Visible = true;
                    btnAnterior.Enabled = false;
                }
                else
                {
                    txtIndex.Value = Convert.ToInt32(-1).ToString();
                    divGrafica1.Visible = false;
                    divBtnGrafica.Visible = false;
                }


                // grafico 2
                cant.RemoveRange(0, cant.Count);
                docente.RemoveRange(0, docente.Count);
                cantGraficos2 = 0;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DatosLibroClasesDevueltoAntesPorAsignatura", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos2 = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos2.Value = Convert.ToInt32(-1).ToString();
                        txtIndex2.Value = string.Empty;
                    }
                }

                if (cantGraficos2 > 0)
                {
                    if (cantGraficos2.ToString().Equals("1"))
                    {
                        divBtnGrafica2.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica2.Visible = true;
                    }

                    txtcantGraficos2.Value = cantGraficos2.ToString();
                    txtIndex2.Value = 1.ToString();

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex2.Value) > 0 || Convert.ToInt32(txtIndex2.Value) <= Convert.ToInt32(txtcantGraficos2.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex2.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {

                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                }
                                rdr.Close();
                                sql.Close();

                                ActualizaContador2();
                                if (txtcantGraficos2.Value.Equals(txtIndex2.Value))
                                {
                                    btnSiguiente2.Enabled = false;
                                }
                                else
                                {
                                    btnSiguiente2.Enabled = true;
                                }

                                ChartBarra2.Series[0].Points.DataBindXY(docente, cant);
                                divGrafico.Visible = true;
                            }
                        }
                    }

                    divGrafico.Visible = true;
                    divGrafica2.Visible = true;
                    btnAnterior2.Enabled = false;
                }
                else
                {
                    txtIndex2.Value = Convert.ToInt32(-1).ToString();
                    divGrafica2.Visible = false;
                    divBtnGrafica2.Visible = false;
                }

                if (cantGraficos == 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar los gráficos";
                }
                if (cantGraficos > 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo2.Text;
                }
                if (cantGraficos == 0 && cantGraficos2 > 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo.Text;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GrfLibroDevueltosAntesPorJornada(string idArchivo, string idArchivo2)
        {
            ArrayList cant = new ArrayList();
            ArrayList docente = new ArrayList();
            decimal cantGraficos;
            decimal cantGraficos2;
            string sp = "sp_DB_T4_LibroClasesDevueltoAntesPorJornada_Paginado";
            txtSp.Value = sp;
            lblmsg.Text = string.Empty;
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
                        txtcantGraficos.Value = Convert.ToInt32(-1).ToString();
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

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex.Value) > 0 || Convert.ToInt32(txtIndex.Value) <= Convert.ToInt32(txtcantGraficos.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {
                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
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
                            }
                        }
                    }
                    divGrafico.Visible = true;
                    divGrafica1.Visible = true;
                    btnAnterior.Enabled = false;
                }
                else
                {
                    txtIndex.Value = Convert.ToInt32(-1).ToString();
                    divGrafica1.Visible = false;
                    divBtnGrafica.Visible = false;
                }


                // grafico 2
                cant.RemoveRange(0, cant.Count);
                docente.RemoveRange(0, docente.Count);
                cantGraficos2 = 0;

                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DatosLibroClasesDevueltoAntesPorJornada", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        sql.Open();
                        da.Fill(dt);
                        sql.Close();

                        cantGraficos2 = Math.Ceiling((Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));
                        txtcantGraficos2.Value = Convert.ToInt32(-1).ToString();
                        txtIndex2.Value = string.Empty;
                    }
                }

                if (cantGraficos2 > 0)
                {
                    if (cantGraficos2.ToString().Equals("1"))
                    {
                        divBtnGrafica2.Visible = false;
                    }
                    else
                    {
                        divBtnGrafica2.Visible = true;
                    }

                    txtcantGraficos2.Value = cantGraficos2.ToString();
                    txtIndex2.Value = 1.ToString();

                    //Setea el primer grafico                  

                    if (Convert.ToInt32(txtIndex2.Value) > 0 || Convert.ToInt32(txtIndex2.Value) <= Convert.ToInt32(txtcantGraficos2.Value))
                    {
                        using (SqlConnection sql = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(sp, sql))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idArchivo", idArchivo2);
                                cmd.Parameters.AddWithValue("@Pag", Convert.ToInt32(txtIndex2.Value));
                                cmd.Parameters.AddWithValue("@TamPag", 20);
                                sql.Open();
                                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                                cant.RemoveRange(0, cant.Count);
                                docente.RemoveRange(0, docente.Count);
                                while (rdr.Read())
                                {

                                    if (sp.Contains("_T1_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                    if (sp.Contains("_T2_"))
                                    {
                                        docente.Add(rdr.GetDateTime(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T3_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetDecimal(0));
                                    }
                                    if (sp.Contains("_T4_"))
                                    {
                                        docente.Add(rdr.GetString(1));
                                        cant.Add(rdr.GetInt32(0));
                                    }
                                }
                                rdr.Close();
                                sql.Close();

                                ActualizaContador2();
                                if (txtcantGraficos2.Value.Equals(txtIndex2.Value))
                                {
                                    btnSiguiente2.Enabled = false;
                                }
                                else
                                {
                                    btnSiguiente2.Enabled = true;
                                }

                                ChartBarra2.Series[0].Points.DataBindXY(docente, cant);
                                divGrafico.Visible = true;
                            }
                        }
                    }

                    divGrafico.Visible = true;
                    divGrafica2.Visible = true;
                    btnAnterior2.Enabled = false;
                }
                else
                {
                    txtIndex2.Value = Convert.ToInt32(-1).ToString();
                    divGrafica2.Visible = false;
                    divBtnGrafica2.Visible = false;
                }

                if (cantGraficos == 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar los gráficos";
                }
                if (cantGraficos > 0 && cantGraficos2 == 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo2.Text;
                }
                if (cantGraficos == 0 && cantGraficos2 > 0)
                {
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Filtros seleccionados no obtuvieron datos para generar gráfico del Id archivo: " + txtidArchivo.Text;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

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

            SeteaGrafico(txtidArchivo.Text, Convert.ToInt32(txtIndex.Value), txtidArchivo2.Text, Convert.ToInt32(txtIndex2.Value), txtSp.Value);
        }

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

            SeteaGrafico(txtidArchivo.Text, Convert.ToInt32(txtIndex.Value), txtidArchivo2.Text, Convert.ToInt32(txtIndex2.Value), txtSp.Value);
        }

        protected void btnAnterior2_Click(object sender, EventArgs e)
        {
            txtIndex2.Value = Convert.ToString(Convert.ToInt32(txtIndex2.Value) - Convert.ToInt32(1));
            ActualizaContador2();
            if (txtIndex2.Value.Equals("1"))
            {
                btnAnterior2.Enabled = false;
            }
            if (Convert.ToInt32(txtIndex2.Value) < Convert.ToInt32(txtcantGraficos2.Value))
            {
                btnSiguiente2.Enabled = true;
            }
            else
            {
                btnAnterior2.Enabled = true;
            }

            SeteaGrafico(txtidArchivo.Text, Convert.ToInt32(txtIndex.Value), txtidArchivo2.Text, Convert.ToInt32(txtIndex2.Value), txtSp.Value);
        }

        protected void btnSiguiente2_Click(object sender, EventArgs e)
        {
            txtIndex2.Value = Convert.ToString(Convert.ToInt32(txtIndex2.Value) + Convert.ToInt32(1));
            ActualizaContador2();
            if (txtIndex2.Value.Equals(txtcantGraficos2.Value))
            {
                btnSiguiente2.Enabled = false;
            }
            if (Convert.ToInt32(txtIndex2.Value) > 1)
            {
                btnAnterior2.Enabled = true;
            }
            else
            {
                btnSiguiente2.Enabled = true;
            }

              SeteaGrafico(txtidArchivo.Text, Convert.ToInt32(txtIndex.Value), txtidArchivo2.Text, Convert.ToInt32(txtIndex2.Value), txtSp.Value);          
        }
    }
}