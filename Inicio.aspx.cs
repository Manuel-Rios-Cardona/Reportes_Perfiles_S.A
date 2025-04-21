using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using PerfilesSA.Modelos;
using PerfilesSA.Services;

namespace PerfilesSA
{
    public partial class Inicio : Page
    {
        private readonly IServicioEmpleado _servicioEmpleado;
        private readonly IServicioDepartamento _servicioDepartamento;

        public Inicio()
        {
            _servicioEmpleado = Global.ServicioEmpleado;
            _servicioDepartamento = Global.ServicioDepartamento;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDepartamentos();
                CargarEmpleados();
                LimpiarFormulario();
            }
        }

        private void CargarDepartamentos()
        {
            try
            {
                var departamentos = _servicioDepartamento.ObtenerTodosDepartamentos();
                ddlDepartamento.DataSource = departamentos;
                ddlDepartamento.DataTextField = "Nombre";
                ddlDepartamento.DataValueField = "IdDepartamento";
                ddlDepartamento.DataBind();
                ddlDepartamento.Items.Insert(0, new ListItem("Seleccione...", ""));
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al cargar departamentos: {ex.Message}", "danger");
            }
        }

        private void CargarEmpleados()
        {
            try
            {
                var empleados = _servicioEmpleado.ObtenerTodosEmpleados();
                gvEmpleados.DataSource = empleados;
                gvEmpleados.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al cargar empleados: {ex.Message}", "danger");
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                var empleado = new Empleado
                {
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    DPI = txtDPI.Text,
                    FechaNacimiento = DateTime.Parse(txtFechaNacimiento.Text),
                    Genero = ddlGenero.SelectedValue,
                    FechaIngreso = DateTime.Parse(txtFechaIngreso.Text),
                    Direccion = txtDireccion.Text,
                    NIT = txtNIT.Text,
                    IdDepartamento = int.Parse(ddlDepartamento.SelectedValue),
                    EstaActivo = chkEstaActivo.Checked
                };

                if (ViewState["ModoEdicion"] != null && (bool)ViewState["ModoEdicion"])
                {
                    empleado.IdEmpleado = Convert.ToInt32(ViewState["IdEmpleado"]);
                    _servicioEmpleado.ActualizarEmpleado(empleado);
                    MostrarMensaje("Empleado actualizado exitosamente.", "success");
                }
                else
                {
                    _servicioEmpleado.InsertarEmpleado(empleado);
                    MostrarMensaje("Empleado registrado exitosamente.", "success");
                }

                LimpiarFormulario();
                CargarEmpleados();
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
            txtApellido.Text = string.Empty;
            txtDPI.Text = string.Empty;
            txtFechaNacimiento.Text = string.Empty;
            ddlGenero.SelectedIndex = 0;
            txtFechaIngreso.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            txtNIT.Text = string.Empty;
            ddlDepartamento.SelectedIndex = 0;
            chkEstaActivo.Checked = true;

            ViewState["ModoEdicion"] = false;
            ViewState["IdEmpleado"] = null;
            btnGuardar.Text = "Guardar";

            // Limpiar los campos calculados
            lblEdad.Text = string.Empty;
            lblAniosServicio.Text = string.Empty;
        }

        protected void gvEmpleados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditarEmpleado")
            {
                int idEmpleado = Convert.ToInt32(e.CommandArgument);
                CargarEmpleadoParaEditar(idEmpleado);
            }
            else if (e.CommandName == "EliminarEmpleado")
            {
                try
                {
                    int idEmpleado = Convert.ToInt32(e.CommandArgument);
                    _servicioEmpleado.EliminarEmpleado(idEmpleado);
                    MostrarMensaje("Empleado eliminado exitosamente.", "success");
                    CargarEmpleados();
                }
                catch (Exception ex)
                {
                    MostrarMensaje($"Error al eliminar empleado: {ex.Message}", "danger");
                }
            }
        }

        private void CargarEmpleadoParaEditar(int idEmpleado)
        {
            try
            {
                var empleado = _servicioEmpleado.ObtenerEmpleadoPorId(idEmpleado);
                if (empleado != null)
                {
                    txtNombre.Text = empleado.Nombre;
                    txtApellido.Text = empleado.Apellido;
                    txtDPI.Text = empleado.DPI;
                    txtFechaNacimiento.Text = empleado.FechaNacimiento.ToString("yyyy-MM-dd");
                    ddlGenero.SelectedValue = empleado.Genero;
                    txtFechaIngreso.Text = empleado.FechaIngreso.ToString("yyyy-MM-dd");
                    txtDireccion.Text = empleado.Direccion;
                    txtNIT.Text = empleado.NIT;
                    ddlDepartamento.SelectedValue = empleado.IdDepartamento.ToString();
                    chkEstaActivo.Checked = empleado.EstaActivo;

                    ViewState["ModoEdicion"] = true;
                    ViewState["IdEmpleado"] = empleado.IdEmpleado;
                    btnGuardar.Text = "Actualizar";

                    // Actualizar campos calculados
                    lblEdad.Text = empleado.Edad.ToString();
                    lblAniosServicio.Text = empleado.AniosServicio.ToString();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al cargar empleado: {ex.Message}", "danger");
            }
        }

        protected void txtFechaNacimiento_TextChanged(object sender, EventArgs e)
        {
            if (DateTime.TryParse(txtFechaNacimiento.Text, out DateTime fechaNacimiento))
            {
                int edad = _servicioEmpleado.CalcularEdad(fechaNacimiento);
                lblEdad.Text = edad.ToString();
            }
        }

        protected void txtFechaIngreso_TextChanged(object sender, EventArgs e)
        {
            if (DateTime.TryParse(txtFechaIngreso.Text, out DateTime fechaIngreso))
            {
                int aniosServicio = _servicioEmpleado.CalcularAniosServicio(fechaIngreso);
                lblAniosServicio.Text = aniosServicio.ToString();
            }
        }

        private void MostrarMensaje(string mensaje, string tipo)
        {
            lblMensaje.CssClass = $"alert alert-{tipo} alert-dismissible fade show mb-3";
            lblMensaje.Text = $"{mensaje}<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>";
            lblMensaje.Visible = true;
        }
    }
}