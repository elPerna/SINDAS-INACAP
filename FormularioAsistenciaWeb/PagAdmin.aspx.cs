using FormularioAsistenciaWeb.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FormularioAsistenciaWeb
{
    public partial class PagAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                if (!VerificaAutorizacion.VerAutorizacion(Session["usuario"].ToString(), "pagAdmin"))
                {
                    Response.Redirect("PagAdmin.aspx");
                }
            }
        }

        protected void btnCrearUsr_Click(object sender, EventArgs e)
        {
            Response.Redirect("CrearUsuario.aspx");
        }

        protected void btnEditarUsr_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditarUsuario.aspx");
        }

        protected void btnEliminarUsr_Click(object sender, EventArgs e)
        {
            Response.Redirect("EliminarUsuario.aspx");
        }

        protected void btnCambContraseña_Click1(object sender, EventArgs e)
        {
            Response.Redirect("CambioContraseña.aspx");
        }

        protected void btnCargArchivo_Click(object sender, EventArgs e)
        {
            Response.Redirect("Excel.aspx");
        }

 

        protected void Dashboard_Click(object sender, EventArgs e)
        {
            Response.Redirect("DashboardDesigner.aspx");
        }

     
    }
}