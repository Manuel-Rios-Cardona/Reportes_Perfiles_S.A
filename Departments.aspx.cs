using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using PerfilesSA.Models;
using PerfilesSA.Services;

namespace PerfilesSA
{
    public partial class Departments : Page
    {
        private readonly IDepartmentService _departmentService;

        public Departments()
        {
            _departmentService = Global.DepartmentService;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDepartments();
            }
        }

        private void LoadDepartments()
        {
            try
            {
                var departments = _departmentService.GetAllDepartments();
                gvDepartments.DataSource = departments;
                gvDepartments.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al cargar los departamentos: {ex.Message}", "danger");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                var department = new Department
                {
                    Name = txtName.Text.Trim(),
                    Description = string.IsNullOrEmpty(txtDescription.Text) ? null : txtDescription.Text.Trim(),
                    IsActive = chkIsActive.Checked
                };

                if (ViewState["EditMode"] != null && (bool)ViewState["EditMode"] && ViewState["DepartmentId"] != null)
                {
                    department.DepartmentId = Convert.ToInt32(ViewState["DepartmentId"]);
                    _departmentService.UpdateDepartment(department);
                    ShowMessage("Departamento actualizado exitosamente.", "success");
                }
                else
                {
                    _departmentService.InsertDepartment(department);
                    ShowMessage("Departamento creado exitosamente.", "success");
                }

                ClearForm();
                LoadDepartments();
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
            txtName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            chkIsActive.Checked = true;
            ViewState["EditMode"] = false;
            ViewState["DepartmentId"] = null;
            btnSave.Text = "Guardar";
            if (lblMessage != null)
            {
                lblMessage.Visible = false;
                lblMessage.Text = string.Empty;
            }
        }

        protected void gvDepartments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int departmentId = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "EditDepartment")
                {
                    var department = _departmentService.GetDepartmentById(departmentId);
                    if (department != null)
                    {
                        txtName.Text = department.Name;
                        txtDescription.Text = department.Description;
                        chkIsActive.Checked = department.IsActive;
                        ViewState["EditMode"] = true;
                        ViewState["DepartmentId"] = department.DepartmentId;
                        btnSave.Text = "Actualizar";
                    }
                }
                else if (e.CommandName == "DeleteDepartment")
                {
                    try
                    {
                        _departmentService.DeleteDepartment(departmentId);
                        ShowMessage("Departamento eliminado exitosamente.", "success");
                        LoadDepartments();
                    }
                    catch (Exception ex)
                    {
                        ShowMessage($"Error al eliminar departamento: {ex.Message}", "danger");
                    }
                }
                else if (e.CommandName == "ToggleStatus")
                {
                    var department = _departmentService.GetDepartmentById(departmentId);
                    if (department != null)
                    {
                        _departmentService.ToggleDepartmentStatus(departmentId, !department.IsActive);
                        LoadDepartments();
                        ShowMessage($"Estado del departamento actualizado exitosamente.", "success");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Error: {ex.Message}", "danger");
            }
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