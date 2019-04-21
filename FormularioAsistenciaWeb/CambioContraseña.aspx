<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CambioContraseña.aspx.cs" Inherits="FormularioAsistenciaWeb.CambioContraseña" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<%--<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>--%>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="section__content section__content--p30">
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-11">
                    <div class="card">
                        <div class="card-header">
                            <strong>Cambiar Contraseña</strong>
                        </div>
                        <div class="card-body card-block">
                            <div class="row form-group">
                                <div class="col col-md-3">
                                    <label for="text-input" class=" form-control-label">RUT</label>
                                </div>
                                <div class="col-12 col-md-9">
                                    <asp:TextBox class="form-control" ID="txtRut" oninput="checkRut(this)" placeholder="RUT  Ej: 12345678-9" runat="server" required="Required"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row form-group">
                                <div class="col col-md-3">
                                    <label for="password-input" class=" form-control-label">Password Actual</label>
                                </div>
                                <div class="col-12 col-md-9">
                                    <asp:TextBox class="form-control" ID="txtContActual" runat="server" TextMode="Password" required="Required" placeholder="Entre 6 y 8 carácteres" pattern=".{6,8}"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row form-group">
                                <div class="col col-md-3">
                                    <label for="password-input" class=" form-control-label">Password Nueva</label>
                                </div>
                                <div class="col-12 col-md-9">
                                    <asp:TextBox class="form-control" ID="txtContNueva" runat="server" TextMode="Password" required="Required" placeholder="Entre 6 y 8 carácteres" pattern=".{6,8}"></asp:TextBox>
                                </div>
                            </div>
                            <div>
                                <asp:Label ID="lblmsg" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="card-footer">
                            <asp:Button class="btns btns-dark d-inline-block d-lg-none ml-auto" ID="btnCambiaPass" runat="server" Text="Cambiar Contraseña" OnClick="btnCambiaPass_Click" />
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
</asp:Content>

