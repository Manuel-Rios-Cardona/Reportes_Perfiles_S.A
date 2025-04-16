using System;
using System.Web.UI;

namespace PerfilesSA
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetupNavigation();
            }
        }

        private void SetupNavigation()
        {
            string currentPage = System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToLower();

            // Configure navigation links with absolute paths
            lnkEmpleados.NavigateUrl = "~/Default.aspx";
            lnkDepartamentos.NavigateUrl = "~/Departments.aspx";
            lnkReportes.NavigateUrl = "~/Reports.aspx";

            // Set active states based on current page
            lnkEmpleados.CssClass = "nav-link" + (currentPage == "default.aspx" ? " active" : "");
            lnkDepartamentos.CssClass = "nav-link" + (currentPage == "departments.aspx" ? " active" : "");
            lnkReportes.CssClass = "nav-link" + (currentPage == "reports.aspx" ? " active" : "");
        }
    }
}