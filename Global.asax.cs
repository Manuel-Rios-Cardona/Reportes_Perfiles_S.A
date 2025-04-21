using System;
using System.Web;
using System.Web.UI;
using PerfilesSA.Services;

namespace PerfilesSA
{
    public class Global : HttpApplication
    {
        private static IServicioEmpleado _servicioEmpleado;
        private static IServicioDepartamento _servicioDepartamento;

        public static IServicioEmpleado ServicioEmpleado
        {
            get
            {
                if (_servicioEmpleado == null)
                {
                    _servicioEmpleado = new ServicioEmpleado();
                }
                return _servicioEmpleado;
            }
        }

        public static IServicioDepartamento ServicioDepartamento
        {
            get
            {
                if (_servicioDepartamento == null)
                {
                    _servicioDepartamento = new ServicioDepartamento();
                }
                return _servicioDepartamento;
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            // Inicializar servicios
            _servicioEmpleado = new ServicioEmpleado();
            _servicioDepartamento = new ServicioDepartamento();

            // Desactivar la validación no intrusiva
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string path = Request.Url.AbsolutePath.ToLower();
            if (path == "/" || path == "/default" || string.IsNullOrEmpty(path))
            {
                Response.Redirect("~/Default.aspx", true);
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {
            //  Código que se ejecuta al cerrar la aplicación
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex != null)
            {
                // Log the error
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}