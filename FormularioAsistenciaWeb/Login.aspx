<%@ Page Title="" Language="C#" MasterPageFile="~/Login.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="FormularioAsistenciaWeb.Login" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-content--bge5">
            <div class="container">
                <div class="login-wrap">
                    <div class="login-content">
                        <div class="login-logo">
                            <a href="#">
                                <img src="images/icon/SINDAS.png" alt="Sindas">
                            </a>
                        </div>
                        <div class="login-form">
                            <form action="" method="post" runat="server">
                                <div class="form-group">
                                    <label>RUT</label>
                                    <asp:textbox class="au-input au-input--full" id="txtRut" placeholder="RUT  Ej: 12345678-9" runat="server"  oninput="checkRut(this)"  required ></asp:textbox>
                                </div>
                                <div class="form-group">
                                    <label>Password</label>
                                    <asp:textbox class="au-input au-input--full" id="txtPwd" placeholder="Contraseña (entre 6 y 8 carácteres)" runat="server" textmode="Password" pattern=".{6,8}" required/>
                                </div>
                              <%--  <div class="login-checkbox">
                                    <label>
                                        <input type="checkbox" name="remember">Recuerdame
                                    </label>
                                    <label>
                                        <a href="#">Olvidó la contraseña?</a>
                                    </label>
                                </div>--%>
                                <asp:Button runat="server" class="au-btn au-btn--block au-btn--green m-b-20" Text="Iniciar sesión" ID="btnIniciaSesion" OnClick="btnIniciaSesion_Click"  />                              
                            </form>
                           <%-- <div class="register-link">
                                <p>
                                    No tienes una cuenta?
                                    <a href="#">Registrate</a>
                                </p>
                            </div>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>