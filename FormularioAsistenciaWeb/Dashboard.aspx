<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="FormularioAsistenciaWeb.DashBoard" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-12" id="divpanel">
                <div class="card">
                    <div class="card-header">
                        <strong>Panel generador de gráficos</strong>
                    </div>
                    <div class="card-body card-block">
                        <div class="row form-group">
                            <div class="col col-md-12">
                                <div class="input-group">
                                    <div class="col col-md-3" style="margin-bottom: 1%">
                                        <asp:TextBox class="au-input au-input--full" ID="txtidArchivo" placeholder="Nro identificador de archivo" runat="server" required onkeypress="return numbersOnly(event)"></asp:TextBox>
                                    </div>
                                    <div class="col-12 col-md-3" style="margin-bottom: 1%">
                                        <asp:DropDownList AutoPostBack="true" class="form-control" ID="cmbexterno" runat="server" required="required" OnSelectedIndexChanged="cmbexterno_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-12 col-md-3" style="margin-bottom: 1%">
                                        <asp:DropDownList AutoPostBack="true" class="form-control" ID="cmbinterno" runat="server" required="required" OnSelectedIndexChanged="cmbinterno_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col col-md-3" style="margin-bottom: 1%">
                                        <div class="col col-md-12">
                                            <label class=" form-control-label">Mostrar Top 10</label>
                                            <asp:CheckBox runat="server" AutoPostBack="true" class="form-check-input" Style="margin-left: 3%;" ID="checkTop" OnCheckedChanged="checkTop_CheckedChanged" />
                                        </div>
                                    </div>
                                    <div class="col-12 col-md-12" style="margin-left: 10px;">
                                        <asp:Label ID="lblmsg" Text="" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-12 col-md-12" style="float: none; text-align: end" id="divBtnPDF" runat="server">
                                        <asp:Button class="btn btn-success btn-md" ID="btnPDF" runat="server" Text="Generar PDF" OnClick="btnPDF_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="container-fluid" runat="server" id="divGrafico">
                <div class="row">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="au-card m-b-30">
                                    <div class="au-card-inner" style="text-align: center">
                                        <%--<div style="position: absolute; left: 0px; top: 0px; right: 0px; bottom: 0px; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;" class="chartjs-size-monitor">
                                            <div class="chartjs-size-monitor-expand" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                                <div style="position: absolute; width: 1000000px; height: 1000000px; left: 0; top: 0">
                                                </div>
                                            </div>
                                            <div class="chartjs-size-monitor-shrink" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                                <div style="position: absolute; width: 200%; height: 200%; left: 0; top: 0">
                                                </div>
                                            </div>
                                        </div>--%>
                                        <h3 class="title-2 m-b-40">Gráfico Barra</h3>
                                        <asp:Chart ID="ChartBarra" runat="server" Height="350px" Width="841px">
                                            <ChartAreas>
                                                <asp:ChartArea Name="ChartBarra">

                                                    <AxisX IntervalAutoMode="VariableCount">
                                                        <LabelStyle Interval="1" />
                                                    </AxisX>

                                                </asp:ChartArea>
                                            </ChartAreas>
                                            <Series>
                                                <asp:Series Name="Series3" ChartType="Column" IsValueShownAsLabel="True" Legend="Legend1" Font="Microsoft Sans Serif, 8pt, style=Bold" IsVisibleInLegend="False"></asp:Series>
                                            </Series>
                                            <Legends>
                                                <asp:Legend Docking="Bottom" Name="Legend1" MaximumAutoSize="100" Alignment="Center">
                                                </asp:Legend>
                                            </Legends>
                                        </asp:Chart>

                                        <div class="col-12 col-md-12" style="float: none; text-align: center" id="divBtnGrafica" runat="server">
                                            <div class="col-12 col-md-12" style="float: none; text-align: center">
                                                <asp:Label ID="lblContador" Text="" runat="server"></asp:Label>
                                            </div>
                                            <asp:Button class="btn btn-success btn-md" ID="btnAnterior" runat="server" Text="Anterior" OnClick="btnAnterior_Click" />
                                            <asp:Button class="btn btn-success btn-md" ID="btnSiguiente" runat="server" Text="Siguiente" OnClick="btnSiguiente_Click" />
                                        </div>
                                        <div>
                                            <asp:HiddenField ID="txtIndex" runat="server" />
                                            <asp:HiddenField ID="txtcantGraficos" runat="server" />
                                            <asp:HiddenField ID="txtSp" runat="server" />
                                        </div>

                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-12" id="GraficoTop" runat="server">
                                <div class="au-card m-b-30">
                                    <div class="au-card-inner" style="text-align: center">
                                        <div style="position: absolute; left: 0px; top: 0px; right: 0px; bottom: 0px; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;" class="chartjs-size-monitor">
                                            <div class="chartjs-size-monitor-expand" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                                <div style="position: absolute; width: 1000000px; height: 1000000px; left: 0; top: 0">
                                                </div>
                                            </div>
                                            <div class="chartjs-size-monitor-shrink" style="position: absolute; left: 0; top: 0; right: 0; bottom: 0; overflow: hidden; pointer-events: none; visibility: hidden; z-index: -1;">
                                                <div style="position: absolute; width: 200%; height: 200%; left: 0; top: 0">
                                                </div>
                                            </div>
                                        </div>
                                        <h3 class="title-2 m-b-40">Gráfico Top 10</h3>
                                        <asp:Chart ID="ColumnTop" runat="server" Height="350px" Width="841px">
                                            <ChartAreas>
                                                <asp:ChartArea Name="ColumnTop">

                                                    <AxisX IntervalAutoMode="VariableCount">
                                                        <LabelStyle Interval="1" />
                                                    </AxisX>

                                                </asp:ChartArea>
                                            </ChartAreas>
                                            <Series>
                                                <asp:Series Name="SeriesTop" ChartType="Column" IsValueShownAsLabel="True" Legend="Legend1" Font="Microsoft Sans Serif, 8pt, style=Bold" IsVisibleInLegend="False"></asp:Series>
                                            </Series>
                                            <Legends>
                                                <asp:Legend Docking="Bottom" Name="Legend1" MaximumAutoSize="100" Alignment="Center">
                                                </asp:Legend>
                                            </Legends>
                                        </asp:Chart>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
