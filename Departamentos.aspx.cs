using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using PerfilesSA.Modelos;
using PerfilesSA.Services;

namespace PerfilesSA
{
    public partial class Departamentos : Page
    {
        private readonly IServicioDepartamento _servicioDepartamento;

        public Departamentos()
        {
            _servicioDepartamento = Global.ServicioDepartamento;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDepartamentos();
            }
        }

        private void CargarDepartamentos()
        {
            try
            {
                var departamentos = _servicioDepartamento.ObtenerTodosDepartamentos();
                gvDepartamentos.DataSource = departamentos;
                gvDepartamentos.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al cargar los departamentos: {ex.Message}", "danger");
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                var departamento = new Departamento
                {
                    Nombre = txtNombre.Text.Trim(),
                    Descripcion = string.IsNullOrEmpty(txtDescripcion.Text) ? null : txtDescripcion.Text.Trim(),
                    EstaActivo = chkEstaActivo.Checked
                };

                if (ViewState["ModoEdicion"] != null && (bool)ViewState["ModoEdicion"] && ViewState["IdDepartamento"] != null)
                {
                    departamento.IdDepartamento = Convert.ToInt32(ViewState["IdDepartamento"]);
                    _servicioDepartamento.ActualizarDepartamento(departamento);
                    MostrarMensaje("Departamento actualizado exitosamente.", "success");
                }
                else
                {
                    _servicioDepartamento.InsertarDepartamento(departamento);
                    MostrarMensaje("Departamento creado exitosamente.", "success");
                }

                LimpiarFormulario();
                CargarDepartamentos();
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error: {ex.Message}", "danger");
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            txtNombre.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            chkEstaActivo.Checked = true;
            ViewState["ModoEdicion"] = false;
            ViewState["IdDepartamento"] = null;
            btnGuardar.Text = "Guardar";
            if (lblMensaje != null)
            {
                lblMensaje.Visible = false;
                lblMensaje.Text = string.Empty;
            }
        }

        protected void gvDepartamentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int idDepartamento = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "EditarDepartamento")
                {
                    var departamento = _servicioDepartamento.ObtenerDepartamentoPorId(idDepartamento);
                    if (departamento != null)
                    {
                        txtNombre.Text = departamento.Nombre;
                        txtDescripcion.Text = departamento.Descripcion;
                        chkEstaActivo.Checked = departamento.EstaActivo;
                        ViewState["ModoEdicion"] = true;
                        ViewState["IdDepartamento"] = departamento.IdDepartamento;
                        btnGuardar.Text = "Actualizar";
                    }
                }
                else if (e.CommandName == "EliminarDepartamento")
                {
                    try
                    {
                        _servicioDepartamento.EliminarDepartamento(idDepartamento);
                        MostrarMensaje("Departamento eliminado exitosamente.", "success");
                        CargarDepartamentos();
                    }
                    catch (Exception ex)
                    {
                        MostrarMensaje($"Error al eliminar departamento: {ex.Message}", "danger");
                    }
                }
                else if (e.CommandName == "CambiarEstado")
                {
                    var departamento = _servicioDepartamento.ObtenerDepartamentoPorId(idDepartamento);
                    if (departamento != null)
                    {
                        _servicioDepartamento.CambiarEstadoDepartamento(idDepartamento, !departamento.EstaActivo);
                        CargarDepartamentos();
                        MostrarMensaje($"Estado del departamento actualizado exitosamente.", "success");
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error: {ex.Message}", "danger");
            }
        }

        private void MostrarMensaje(string mensaje, string tipo)
        {
            if (lblMensaje != null)
            {
                lblMensaje.CssClass = $"alert alert-{tipo} alert-dismissible fade show mb-3";
                lblMensaje.Text = $"{mensaje}<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>";
                lblMensaje.Visible = true;
            }
        }
    }
}