using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FormularioAsistenciaWeb.Helper
{
    public static class VerificaAutorizacion
    {
        public static bool VerAutorizacion(string rol, string pagina)
        {
            if (rol.Contains("Director"))
            {
                if (!pagina.Contains("cambContrasena") && !pagina.Contains("verDash") && !pagina.Contains("pagAdmin") && !pagina.Contains("verDatos"))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            if (rol.Contains("Coordinador"))
            {
                if (!pagina.Contains("cambContrasena") && !pagina.Contains("verDash") && !pagina.Contains("cargaArchivo") && !pagina.Contains("verDatos") && !pagina.Contains("pagAdmin"))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return true;   
        }
    }
}