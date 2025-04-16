using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using PerfilesSA.Models;
using PerfilesSA.Services;

namespace PerfilesSA
{
    public partial class Reports : Page
    {
        private IEmployeeService EmployeeService => Global.EmployeeService;
        private IDepartmentService DepartmentService => Global.DepartmentService;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDepartments();
                LoadAllEmployees();
            }
        }

        private void LoadDepartments()
        {
            try
            {
                var departments = DepartmentService.GetAllDepartments();
                ddlDepartment.DataSource = departments;
                ddlDepartment.DataTextField = "Name";
                ddlDepartment.DataValueField = "DepartmentId";
                ddlDepartment.DataBind();
                ddlDepartment.Items.Insert(0, new ListItem("Todos los Departamentos", ""));
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al cargar departamentos: {ex.Message}", "danger");
            }
        }

        private void LoadAllEmployees()
        {
            try
            {
                var employees = EmployeeService.GetAllEmployees();
                gvReport.DataSource = employees;
                gvReport.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al cargar empleados: {ex.Message}", "danger");
            }
        }

        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = (DataRowView)e.Row.DataItem;

                // Agregar atributos de datos para JavaScript
                if (dr["BirthDate"] != DBNull.Value)
                {
                    DateTime birthDate = Convert.ToDateTime(dr["BirthDate"]);
                    e.Row.Attributes["data-birth-date"] = birthDate.ToString("yyyy-MM-dd");
                }

                if (dr["HireDate"] != DBNull.Value)
                {
                    DateTime hireDate = Convert.ToDateTime(dr["HireDate"]);
                    e.Row.Attributes["data-hire-date"] = hireDate.ToString("yyyy-MM-dd");
                }

                // Configurar el estado del empleado
                if (dr["IsActive"] != DBNull.Value)
                {
                    bool isActive = Convert.ToBoolean(dr["IsActive"]);
                    Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                    if (lblStatus != null)
                    {
                        lblStatus.Text = isActive ? "Activo" : "Inactivo";
                        lblStatus.CssClass = $"badge {(isActive ? "bg-success" : "bg-danger")}";
                    }
                }
            }
        }

        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable reportData;

                if (!string.IsNullOrEmpty(ddlDepartment.SelectedValue))
                {
                    // Filtrar por departamento
                    int departmentId = int.Parse(ddlDepartment.SelectedValue);
                    reportData = EmployeeService.GetEmployeesByDepartment(departmentId);
                }
                else if (!string.IsNullOrEmpty(txtStartDate.Text) && !string.IsNullOrEmpty(txtEndDate.Text))
                {
                    // Filtrar por rango de fechas de ingreso
                    DateTime startDate = DateTime.Parse(txtStartDate.Text);
                    DateTime endDate = DateTime.Parse(txtEndDate.Text);
                    reportData = EmployeeService.GetEmployeesByDateRange(startDate, endDate);
                }
                else
                {
                    // Mostrar todos los empleados
                    reportData = EmployeeService.GetAllEmployees();
                }

                gvReport.DataSource = reportData;
                gvReport.DataBind();

                if (reportData.Rows.Count == 0)
                {
                    ShowMessage("No se encontraron registros con los filtros seleccionados.", "warning");
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al generar el reporte: {ex.Message}", "danger");
            }
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable reportData;

                if (!string.IsNullOrEmpty(ddlDepartment.SelectedValue))
                {
                    int departmentId = int.Parse(ddlDepartment.SelectedValue);
                    reportData = EmployeeService.GetEmployeesByDepartment(departmentId);
                }
                else if (!string.IsNullOrEmpty(txtStartDate.Text) && !string.IsNullOrEmpty(txtEndDate.Text))
                {
                    DateTime startDate = DateTime.Parse(txtStartDate.Text);
                    DateTime endDate = DateTime.Parse(txtEndDate.Text);
                    reportData = EmployeeService.GetEmployeesByDateRange(startDate, endDate);
                }
                else
                {
                    reportData = EmployeeService.GetAllEmployees();
                }

                if (reportData.Rows.Count > 0)
                {
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=ReporteEmpleados.xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.ContentEncoding = System.Text.Encoding.UTF8;

                    using (System.IO.StringWriter sw = new System.IO.StringWriter())
                    {
                        using (System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw))
                        {
                            gvReport.RenderControl(hw);
                            Response.Output.Write(sw.ToString());
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
                else
                {
                    ShowMessage("No hay datos para exportar.", "warning");
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al exportar a Excel: {ex.Message}", "danger");
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // Necesario para la exportación a Excel
        }

        private void ShowMessage(string message, string type)
        {
            if (lblMessage != null)
            {
                lblMessage.CssClass = $"alert alert-{type} alert-dismissible fade show mb-3";
                lblMessage.Text = $"{message}<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>";
                lblMessage.Visible = true;
            }
        }
    }
}