using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using PerfilesSA.Models;
using PerfilesSA.Services;

namespace PerfilesSA
{
    public partial class Default : Page
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;

        public Default()
        {
            _employeeService = Global.EmployeeService;
            _departmentService = Global.DepartmentService;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDepartments();
                LoadEmployees();
                ClearForm();
            }
        }

        private void LoadDepartments()
        {
            try
            {
                var departments = _departmentService.GetAllDepartments();
                ddlDepartment.DataSource = departments;
                ddlDepartment.DataTextField = "Name";
                ddlDepartment.DataValueField = "DepartmentId";
                ddlDepartment.DataBind();
                ddlDepartment.Items.Insert(0, new ListItem("Seleccione...", ""));
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al cargar departamentos: {ex.Message}", "danger");
            }
        }

        private void LoadEmployees()
        {
            try
            {
                var employees = _employeeService.GetAllEmployees();
                gvEmployees.DataSource = employees;
                gvEmployees.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al cargar empleados: {ex.Message}", "danger");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var employee = new Employee
                {
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    DPI = txtDPI.Text,
                    BirthDate = DateTime.Parse(txtBirthDate.Text),
                    Gender = ddlGender.SelectedValue,
                    HireDate = DateTime.Parse(txtHireDate.Text),
                    Address = txtAddress.Text,
                    NIT = txtNIT.Text,
                    DepartmentId = int.Parse(ddlDepartment.SelectedValue),
                    IsActive = chkIsActive.Checked
                };

                if (ViewState["EditMode"] != null && (bool)ViewState["EditMode"])
                {
                    employee.EmployeeId = Convert.ToInt32(ViewState["EmployeeId"]);
                    _employeeService.UpdateEmployee(employee);
                    ShowMessage("Empleado actualizado exitosamente.", "success");
                }
                else
                {
                    _employeeService.InsertEmployee(employee);
                    ShowMessage("Empleado registrado exitosamente.", "success");
                }

                ClearForm();
                LoadEmployees();
            }
            catch (Exception ex)
            {
                ShowMessage($"Error: {ex.Message}", "danger");
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtDPI.Text = string.Empty;
            txtBirthDate.Text = string.Empty;
            ddlGender.SelectedIndex = 0;
            txtHireDate.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtNIT.Text = string.Empty;
            ddlDepartment.SelectedIndex = 0;
            chkIsActive.Checked = true;

            ViewState["EditMode"] = false;
            ViewState["EmployeeId"] = null;
            btnSave.Text = "Guardar";

            // Limpiar los campos calculados
            lblAgeValue.Text = string.Empty;
            lblYearsOfServiceValue.Text = string.Empty;
        }

        protected void gvEmployees_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditEmployee")
            {
                int employeeId = Convert.ToInt32(e.CommandArgument);
                LoadEmployeeForEdit(employeeId);
            }
            else if (e.CommandName == "DeleteEmployee")
            {
                try
                {
                    int employeeId = Convert.ToInt32(e.CommandArgument);
                    _employeeService.DeleteEmployee(employeeId);
                    ShowMessage("Empleado eliminado exitosamente.", "success");
                    LoadEmployees();
                }
                catch (Exception ex)
                {
                    ShowMessage($"Error al eliminar empleado: {ex.Message}", "danger");
                }
            }
        }

        private void LoadEmployeeForEdit(int employeeId)
        {
            try
            {
                var employee = _employeeService.GetEmployeeById(employeeId);
                if (employee != null)
                {
                    txtFirstName.Text = employee.FirstName;
                    txtLastName.Text = employee.LastName;
                    txtDPI.Text = employee.DPI;
                    txtBirthDate.Text = employee.BirthDate.ToString("yyyy-MM-dd");
                    ddlGender.SelectedValue = employee.Gender;
                    txtHireDate.Text = employee.HireDate.ToString("yyyy-MM-dd");
                    txtAddress.Text = employee.Address;
                    txtNIT.Text = employee.NIT;
                    ddlDepartment.SelectedValue = employee.DepartmentId.ToString();
                    chkIsActive.Checked = employee.IsActive;

                    ViewState["EditMode"] = true;
                    ViewState["EmployeeId"] = employee.EmployeeId;
                    btnSave.Text = "Actualizar";

                    // Actualizar campos calculados
                    lblAgeValue.Text = employee.Age.ToString();
                    lblYearsOfServiceValue.Text = employee.YearsOfService.ToString();
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al cargar empleado: {ex.Message}", "danger");
            }
        }

        protected void txtBirthDate_TextChanged(object sender, EventArgs e)
        {
            if (DateTime.TryParse(txtBirthDate.Text, out DateTime birthDate))
            {
                int age = _employeeService.CalculateAge(birthDate);
                lblAgeValue.Text = age.ToString();
            }
        }

        protected void txtHireDate_TextChanged(object sender, EventArgs e)
        {
            if (DateTime.TryParse(txtHireDate.Text, out DateTime hireDate))
            {
                int yearsOfService = _employeeService.CalculateYearsOfService(hireDate);
                lblYearsOfServiceValue.Text = yearsOfService.ToString();
            }
        }

        private void ShowMessage(string message, string type)
        {
            lblMessage.CssClass = $"alert alert-{type} alert-dismissible fade show mb-3";
            lblMessage.Text = $"{message}<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>";
            lblMessage.Visible = true;
        }
    }
}