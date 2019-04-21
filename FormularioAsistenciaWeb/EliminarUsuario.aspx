<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EliminarUsuario.aspx.cs" Inherits="FormularioAsistenciaWeb.EliminarUsuario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<%--<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>--%>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
         <div class="section__content section__content--p30">
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-12">
                    <div class="card">
                        <div class="card-header">
                            <strong>Panel de Búsqueda</strong>
                        </div>
                        <div class="card-body card-block">
                            <div class="row form-group">
                                <div class="col col-md-12">
                                    <div class="input-group">
                                        <div style="width: 50%; margin-right: 1%;">
                                            <asp:TextBox class="au-input au-input--full" ID="txtRut" placeholder="RUT  Ej: 12345678-9" runat="server" oninput="checkRut(this)" required></asp:TextBox>
                                        </div>
                                        <div class="input-group-btn">
                                            <asp:Button runat="server" class="btn btn-primary" Text="Buscar" ID="btnBusqueda" OnClick="btnBusqueda_Click"/>
                                        </div>
                                        <div>
                                            <asp:Label ID="lblmsg" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-lg-13" runat="server" id="frmEditar">
                <div class="card">
                    <div class="card-header">
                        <strong>Eliminación de Usuario</strong>
                    </div>
                    <div class="card-body card-block">
                        <div class="row form-group">
                            <div class="col col-md-3">
                                <label for="select" class=" form-control-label">Rol</label>
                            </div>
                            <div class="col-12 col-md-9">
                                <asp:DropDownList class="form-control" ID="tipoUsuario" runat="server" required="required" disabled="">
                                    <asp:ListItem Selected="false" Text="Seleccione" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Administrador" Value="Administrador"></asp:ListItem>
                                    <asp:ListItem Text="Coordinador" Value="Coordinador"></asp:ListItem>
                                    <asp:ListItem Text="Director" Value="Director"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col col-md-3">
                                <label for="text-input" class=" form-control-label">RUT</label>
                            </div>
                            <div class="col-12 col-md-9">
                                <asp:TextBox class="form-control" ID="txtRutFrm" ReadOnly="true" placeholder="RUT  Ej: 12345678-9" runat="server" required="Required"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col col-md-3">
                                <label for="text-input" class=" form-control-label">Nombre</label>
                            </div>
                            <div class="col-12 col-md-9">
                                <asp:TextBox class="form-control" ID="txtNombre" placeholder="Ingrese nombre" runat="server" ReadOnly="true" required></asp:TextBox>
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col col-md-3">
                                <label for="text-input" class=" form-control-label">Apellido</label>
                            </div>
                            <div class="col-12 col-md-9">
                                <asp:TextBox class="form-control" ID="txtApellido" placeholder="Ingrese apellido" runat="server" ReadOnly="true" required></asp:TextBox>
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col col-md-3">
                                <label for="email-input" class=" form-control-label">Email </label>
                            </div>
                            <div class="col-12 col-md-9">
                                <asp:TextBox class="form-control" ID="txtEmail" ReadOnly="true" placeholder="Ingrese email" runat="server" TextMode="Email" required="Required"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="card-footer">
                        <asp:Button class="btn btn-primary btn-sm" ID="btnEliminar" runat="server" Text="Eliminar Usuario" OnClick="btnEliminar_Click" OnClientClick="return confirm('¿Seguro desea eliminar este usuario?');" />
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
