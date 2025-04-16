using System;
using System.Web;
using System.Web.UI;
using PerfilesSA.Services;

namespace PerfilesSA
{
    public class Global : HttpApplication
    {
        private static IEmployeeService _employeeService;
        private static IDepartmentService _departmentService;

        public static IEmployeeService EmployeeService
        {
            get
            {
                if (_employeeService == null)
                {
                    _employeeService = new EmployeeService();
                }
                return _employeeService;
            }
        }

        public static IDepartmentService DepartmentService
        {
            get
            {
                if (_departmentService == null)
                {
                    _departmentService = new DepartmentService();
                }
                return _departmentService;
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            // Inicializar servicios
            _employeeService = new EmployeeService();
            _departmentService = new DepartmentService();

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