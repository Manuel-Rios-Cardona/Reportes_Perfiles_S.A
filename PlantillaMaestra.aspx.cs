using System;
using System.Web.UI;

namespace PerfilesSA
{
    public partial class PlantillaMaestra : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ConfigurarNavegacion();
            }
        }

        private void ConfigurarNavegacion()
        {
            string paginaActual = System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToLower();

            // Configurar enlaces de navegación con rutas absolutas
            lnkEmpleados.NavigateUrl = "~/Inicio.aspx";
            lnkDepartamentos.NavigateUrl = "~/Departamentos.aspx";
            lnkReportes.NavigateUrl = "~/Reportes.aspx";

            // Establecer estados activos según la página actual
            lnkEmpleados.CssClass = "nav-link" + (paginaActual == "inicio.aspx" ? " active" : "");
            lnkDepartamentos.CssClass = "nav-link" + (paginaActual == "departamentos.aspx" ? " active" : "");
            lnkReportes.CssClass = "nav-link" + (paginaActual == "reportes.aspx" ? " active" : "");
        }
    }
}