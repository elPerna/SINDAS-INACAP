<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Excel.aspx.cs" Inherits="FormularioAsistenciaWeb.Contact" %>


<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">


    <div class="section__content section__content--p30">
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-11">
                    <div class="card">
                        <div class="card-header">
                            <strong>Carga de Excel</strong>
                        </div>
                        <div class="card-body card-block">
                            <div class="row form-group">
                                <div class="col col-xs-12 col-sm-12 col-md-3">
                                    <label for="text-input" class=" form-control-label">Seleccione un archivo</label>
                                </div>
                                <div class="col col-xs-12 col-sm-12  col-md-9">
                                    <asp:FileUpload ID="cargaexcel" runat="server" accept=".xls, .xlsx" Width="70%" />
                                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                                    <asp:RegularExpressionValidator ID="Validadorarchivos" ForeColor="Red" runat="server" ErrorMessage="Archivo No Permitido"
                                        ControlToValidate="cargaexcel" ValidationExpression="(.*).(.xls|.XLS|.xlsx|.XLSX)$" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                        ControlToValidate="cargaexcel" ForeColor="Red" ErrorMessage="Debe seleccionar un archivo."></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="card-footer">
                            <asp:Button class="btn btn-primary btn-sm" ID="enviar" runat="server" Text="Enviar" OnClick="enviar_Click1" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="copyright">
                        <p>Copyright © 2018 Inacap Santiago Sur. Todos los derechos reservados.</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--<header>
       <meta charset="utf-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <meta name="viewport" content="width=device-width, initial-scale=1">
       <link rel="stylesheet" href="Content/bootstrap.css" />
       <link rel="stylesheet" href="Content/bootstrap-theme.css" />
       <link rel="stylesheet" href="Content/bootstrap.min.css" />
   </header>


     <br />
     <hgroup class="title">
        
        <h1>Carga de Excel</h1>
          
    </hgroup>
     <br />

    <div style="width:500px">
        <asp:FileUpload ID="cargaexcel" runat="server" accept=".xls, .xlsx" Width="100%" />        
        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
        <asp:RegularExpressionValidator ID="Validadorarchivos" ForeColor="Red" runat="server" ErrorMessage="Archivo No Permitido" 
            ControlToValidate="cargaexcel" ValidationExpression= "(.*).(.xls|.XLS|.xlsx|.XLSX)$" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
      ControlToValidate="cargaexcel" ForeColor="Red" ErrorMessage="Debe seleccionar un archivo."></asp:RequiredFieldValidator>
    </div>
     <div style="width:25%">
            <asp:button class="btn-danger btn-lg btn-block" id="enviar"  runat="server" Text="Enviar" OnClick="enviar_Click1" ></asp:button>
            
     </div>--%>
</asp:Content>


