using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using PerfilesSA.Modelos;
using PerfilesSA.Services;

namespace PerfilesSA
{
    public partial class Reportes : Page
    {
        private IServicioEmpleado ServicioEmpleado => Global.ServicioEmpleado;
        private IServicioDepartamento ServicioDepartamento => Global.ServicioDepartamento;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDepartamentos();
                CargarTodosEmpleados();
            }
        }

        private void CargarDepartamentos()
        {
            try
            {
                var departamentos = ServicioDepartamento.ObtenerTodosDepartamentos();
                ddlDepartamento.DataSource = departamentos;
                ddlDepartamento.DataTextField = "Nombre";
                ddlDepartamento.DataValueField = "IdDepartamento";
                ddlDepartamento.DataBind();
                ddlDepartamento.Items.Insert(0, new ListItem("Todos los Departamentos", ""));
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al cargar departamentos: {ex.Message}", "danger");
            }
        }

        private void CargarTodosEmpleados()
        {
            try
            {
                var empleados = ServicioEmpleado.ObtenerTodosEmpleados();
                gvReporte.DataSource = empleados;
                gvReporte.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al cargar empleados: {ex.Message}", "danger");
            }
        }

        protected void gvReporte_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = (DataRowView)e.Row.DataItem;

                // Agregar atributos de datos para JavaScript
                if (dr["FechaNacimiento"] != DBNull.Value)
                {
                    DateTime fechaNacimiento = Convert.ToDateTime(dr["FechaNacimiento"]);
                    e.Row.Attributes["data-birth-date"] = fechaNacimiento.ToString("yyyy-MM-dd");
                }

                if (dr["FechaIngreso"] != DBNull.Value)
                {
                    DateTime fechaIngreso = Convert.ToDateTime(dr["FechaIngreso"]);
                    e.Row.Attributes["data-hire-date"] = fechaIngreso.ToString("yyyy-MM-dd");
                }

                // Configurar el estado del empleado
                if (dr["EstaActivo"] != DBNull.Value)
                {
                    bool estaActivo = Convert.ToBoolean(dr["EstaActivo"]);
                    Label lblEstado = (Label)e.Row.FindControl("lblEstado");
                    if (lblEstado != null)
                    {
                        lblEstado.Text = estaActivo ? "Activo" : "Inactivo";
                        lblEstado.CssClass = $"badge {(estaActivo ? "bg-success" : "bg-danger")}";
                    }
                }
            }
        }

        protected void btnGenerarReporte_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable datosReporte;

                if (!string.IsNullOrEmpty(ddlDepartamento.SelectedValue))
                {
                    // Filtrar por departamento
                    int idDepartamento = int.Parse(ddlDepartamento.SelectedValue);
                    datosReporte = ServicioEmpleado.ObtenerEmpleadosPorDepartamento(idDepartamento);
                }
                else if (!string.IsNullOrEmpty(txtFechaInicio.Text) && !string.IsNullOrEmpty(txtFechaFin.Text))
                {
                    // Filtrar por rango de fechas de ingreso
                    DateTime fechaInicio = DateTime.Parse(txtFechaInicio.Text);
                    DateTime fechaFin = DateTime.Parse(txtFechaFin.Text);
                    datosReporte = ServicioEmpleado.ObtenerEmpleadosPorRangoFechas(fechaInicio, fechaFin);
                }
                else
                {
                    // Mostrar todos los empleados
                    datosReporte = ServicioEmpleado.ObtenerTodosEmpleados();
                }

                gvReporte.DataSource = datosReporte;
                gvReporte.DataBind();

                if (datosReporte.Rows.Count == 0)
                {
                    MostrarMensaje("No se encontraron registros con los filtros seleccionados.", "warning");
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al generar el reporte: {ex.Message}", "danger");
            }
        }

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable datosReporte;

                if (!string.IsNullOrEmpty(ddlDepartamento.SelectedValue))
                {
                    int idDepartamento = int.Parse(ddlDepartamento.SelectedValue);
                    datosReporte = ServicioEmpleado.ObtenerEmpleadosPorDepartamento(idDepartamento);
                }
                else if (!string.IsNullOrEmpty(txtFechaInicio.Text) && !string.IsNullOrEmpty(txtFechaFin.Text))
                {
                    DateTime fechaInicio = DateTime.Parse(txtFechaInicio.Text);
                    DateTime fechaFin = DateTime.Parse(txtFechaFin.Text);
                    datosReporte = ServicioEmpleado.ObtenerEmpleadosPorRangoFechas(fechaInicio, fechaFin);
                }
                else
                {
                    datosReporte = ServicioEmpleado.ObtenerTodosEmpleados();
                }

                if (datosReporte.Rows.Count > 0)
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
                            gvReporte.RenderControl(hw);
                            Response.Output.Write(sw.ToString());
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
                else
                {
                    MostrarMensaje("No hay datos para exportar.", "warning");
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al exportar a Excel: {ex.Message}", "danger");
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // Necesario para la exportación a Excel
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