<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CrearUsuario.aspx.cs" Inherits="FormularioAsistenciaWeb.CrearUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/Validaciones.js"></script>
<%--    <script>
        $("#btnRegistrar").click(function () {
            if (Fn.validaRut($("#txtRut").val())) {
                $("#lblmsg").html("El rut ingresado es válido :D");
            } else {
                $("#lblmsg").html("El Rut no es válido :'( ");
            }
        });
       </script>--%>
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
                            <strong>Registro de Usuario</strong>
                        </div>
                        <div class="card-body card-block">
                            <div class="row form-group">
                                <div class="col col-md-3">
                                    <label for="select" class=" form-control-label">Rol</label>
                                </div>
                                <div class="col-12 col-md-9">
                                    <asp:DropDownList class="form-control" ID="tipoUsuario" runat="server" required="required">
                                        <asp:ListItem Selected="false" Text="Seleccione" Value=""></asp:ListItem>
                                        <asp:ListItem Text="Administrador" Value="Administrador"></asp:ListItem>
                                        <asp:ListItem Text="Coordinador" Value="Coordinador"></asp:ListItem>
                                        <asp:ListItem Text="Director" Value="Director"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row form-group">
                                <div class="col col-md-3">
                                    <label for="text-input" class=" form-control-label">Nombre</label>
                                </div>
                                <div class="col-12 col-md-9">
                                    <asp:TextBox class="form-control" ID="txtNombre"  placeholder="Ingrese nombre" runat="server" required></asp:TextBox>
                                </div>
                            </div>
                            <div class="row form-group">
                                <div class="col col-md-3">
                                    <label for="text-input" class=" form-control-label">Apellido</label>
                                </div>
                                <div class="col-12 col-md-9">
                                    <asp:TextBox class="form-control" ID="txtApellido" placeholder="Ingrese apellido" runat="server" required></asp:TextBox>
                                </div>
                            </div>
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
                                    <label for="email-input" class=" form-control-label">Email </label>
                                </div>
                                <div class="col-12 col-md-9">
                                    <asp:TextBox class="form-control" ID="txtEmail" placeholder="Ingrese email" runat="server" TextMode="Email" required="Required"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row form-group">
                                <div class="col col-md-3">
                                    <label for="password-input" class=" form-control-label">Password</label>
                                </div>
                                <div class="col-12 col-md-9">
                                    <asp:TextBox class="form-control" ID="txtContTemp" runat="server" TextMode="Password" required="Required" placeholder="Entre 6 y 8 carácteres" pattern=".{6,8}"></asp:TextBox>
                                </div>
                            </div>
                            <div>
                                <asp:Label ID="lblmsg" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="card-footer">
                            <asp:button class="btns btns-dark d-inline-block d-lg-none ml-auto" id="btnRegistrar"  runat="server" Text="Registrar Usuario" OnClick="btnRegistrar_Click" />
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
