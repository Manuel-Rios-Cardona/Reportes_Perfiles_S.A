<%@ Page Title="Empleados" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="PerfilesSA.Inicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function calcularEdad(fechaNacimiento) {
            const today = new Date();
            const birth = new Date(fechaNacimiento);
            let edad = today.getFullYear() - birth.getFullYear();
            const monthDiff = today.getMonth() - birth.getMonth();

            if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birth.getDate())) {
                edad--;
            }
            return edad;
        }

        function calcularAniosServicio(fechaIngreso) {
            const today = new Date();
            const hire = new Date(fechaIngreso);
            let anios = today.getFullYear() - hire.getFullYear();
            const monthDiff = today.getMonth() - hire.getMonth();

            if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < hire.getDate())) {
                anios--;
            }
            return anios;
        }

        function actualizarCamposCalculados() {
            const fechaNacimientoInput = document.getElementById('<%= txtFechaNacimiento.ClientID %>');
            const fechaIngresoInput = document.getElementById('<%= txtFechaIngreso.ClientID %>');
            const edadLabel = document.getElementById('<%= lblEdad.ClientID %>');
            const aniosLabel = document.getElementById('<%= lblAniosServicio.ClientID %>');

            if (fechaNacimientoInput && fechaNacimientoInput.value) {
                const edad = calcularEdad(fechaNacimientoInput.value);
                if (edadLabel) edadLabel.textContent = edad;
            }

            if (fechaIngresoInput && fechaIngresoInput.value) {
                const anios = calcularAniosServicio(fechaIngresoInput.value);
                if (aniosLabel) aniosLabel.textContent = anios;
            }
        }

        // Agregar listeners para actualización en tiempo real
        document.addEventListener('DOMContentLoaded', function () {
            const fechaNacimientoInput = document.getElementById('<%= txtFechaNacimiento.ClientID %>');
            const fechaIngresoInput = document.getElementById('<%= txtFechaIngreso.ClientID %>');

            if (fechaNacimientoInput) {
                fechaNacimientoInput.addEventListener('change', actualizarCamposCalculados);
            }
            if (fechaIngresoInput) {
                fechaIngresoInput.addEventListener('change', actualizarCamposCalculados);
            }

            // Calcular valores iniciales
            actualizarCamposCalculados();
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-12">
            <h2 class="mb-4">Gestión de Empleados</h2>
            
            <asp:Label ID="lblMensaje" runat="server" CssClass="d-none"></asp:Label>
            
            <asp:HiddenField ID="hfIdEmpleado" runat="server" Value="" />
            
            <div class="card mb-4">
                <div class="card-header">
                    <h4 class="mb-0">Registro de Empleado</h4>
                </div>
                <div class="card-body">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-danger" ValidationGroup="vgEmpleado" />
                    
                    <div class="row mb-3">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtNombre" class="form-label">Nombres:</label>
                                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="rfvNombre" runat="server" 
                                    ControlToValidate="txtNombre" 
                                    ErrorMessage="El nombre es requerido" 
                                    Display="Dynamic" 
                                    CssClass="text-danger"
                                    ValidationGroup="vgEmpleado">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtApellido" class="form-label">Apellidos:</label>
                                <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="rfvApellido" runat="server" 
                                    ControlToValidate="txtApellido" 
                                    ErrorMessage="El apellido es requerido" 
                                    Display="Dynamic" 
                                    CssClass="validation-error"
                                    ValidationGroup="vgEmpleado">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>

                    <div class="row mb-3">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtDPI" class="form-label">DPI:</label>
                                <asp:TextBox ID="txtDPI" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="rfvDPI" runat="server" 
                                    ControlToValidate="txtDPI" 
                                    ErrorMessage="El DPI es requerido" 
                                    Display="Dynamic" 
                                    CssClass="validation-error"
                                    ValidationGroup="vgEmpleado">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtFechaNacimiento" class="form-label">Fecha de Nacimiento:</label>
                                <asp:TextBox ID="txtFechaNacimiento" runat="server" CssClass="form-control" TextMode="Date" AutoPostBack="true" OnTextChanged="txtFechaNacimiento_TextChanged" />
                                <asp:RequiredFieldValidator ID="rfvFechaNacimiento" runat="server" 
                                    ControlToValidate="txtFechaNacimiento" 
                                    ErrorMessage="La fecha de nacimiento es requerida" 
                                    Display="Dynamic" 
                                    CssClass="validation-error"
                                    ValidationGroup="vgEmpleado">*</asp:RequiredFieldValidator>
                                <div class="form-text">Edad: <asp:Label ID="lblEdad" runat="server" /></div>
                            </div>
                        </div>
                    </div>

                    <div class="row mb-3">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="ddlGenero" class="form-label">Género:</label>
                                <asp:DropDownList ID="ddlGenero" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="Seleccione..." Value="" />
                                    <asp:ListItem Text="Masculino" Value="M" />
                                    <asp:ListItem Text="Femenino" Value="F" />
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvGenero" runat="server" 
                                    ControlToValidate="ddlGenero" 
                                    ErrorMessage="El género es requerido" 
                                    Display="Dynamic" 
                                    CssClass="validation-error"
                                    ValidationGroup="vgEmpleado">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtFechaIngreso" class="form-label">Fecha de Contratación:</label>
                                <asp:TextBox ID="txtFechaIngreso" runat="server" CssClass="form-control" TextMode="Date" AutoPostBack="true" OnTextChanged="txtFechaIngreso_TextChanged" />
                                <asp:RequiredFieldValidator ID="rfvFechaIngreso" runat="server" 
                                    ControlToValidate="txtFechaIngreso" 
                                    ErrorMessage="La fecha de contratación es requerida" 
                                    Display="Dynamic" 
                                    CssClass="validation-error"
                                    ValidationGroup="vgEmpleado">*</asp:RequiredFieldValidator>
                                <div class="form-text">Años de servicio: <asp:Label ID="lblAniosServicio" runat="server" /></div>
                            </div>
                        </div>
                    </div>

                    <div class="row mb-3">
                        <div class="col-12">
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="txtDireccion" CssClass="form-label">Dirección (Opcional)</asp:Label>
                                <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row mb-3">
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="txtNIT" CssClass="form-label">NIT (Opcional)</asp:Label>
                                <asp:TextBox ID="txtNIT" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="ddlDepartamento" class="form-label">Departamento:</label>
                                <asp:DropDownList ID="ddlDepartamento" runat="server" CssClass="form-select" />
                                <asp:RequiredFieldValidator ID="rfvDepartamento" runat="server" 
                                    ControlToValidate="ddlDepartamento" 
                                    ErrorMessage="El departamento es requerido" 
                                    Display="Dynamic" 
                                    CssClass="validation-error"
                                    ValidationGroup="vgEmpleado">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group mt-4">
                                <div class="form-check">
                                    <asp:CheckBox ID="chkEstaActivo" runat="server" Checked="true" CssClass="form-check-input" />
                                    <asp:Label runat="server" AssociatedControlID="chkEstaActivo" CssClass="form-check-label">
                                        Empleado Activo
                                    </asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-12">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" ValidationGroup="vgEmpleado" UseSubmitBehavior="true" />
                            <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-secondary" OnClick="btnLimpiar_Click" CausesValidation="false" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="card">
                <div class="card-header">
                    <h4 class="mb-0">Lista de Empleados</h4>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <asp:GridView ID="gvEmpleados" runat="server" 
                            AutoGenerateColumns="False" 
                            CssClass="table table-striped table-bordered table-hover"
                            OnRowCommand="gvEmpleados_RowCommand"
                            DataKeyNames="IdEmpleado">
                            <Columns>
                                <asp:BoundField DataField="Nombre" HeaderText="Nombres" />
                                <asp:BoundField DataField="Apellido" HeaderText="Apellidos" />
                                <asp:BoundField DataField="DPI" HeaderText="DPI" />
                                <asp:BoundField DataField="FechaNacimiento" HeaderText="Fecha Nacimiento" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="Genero" HeaderText="Género" />
                                <asp:BoundField DataField="FechaIngreso" HeaderText="Fecha Contratación" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="Direccion" HeaderText="Dirección" />
                                <asp:BoundField DataField="NIT" HeaderText="NIT" />
                                <asp:BoundField DataField="NombreDepartamento" HeaderText="Departamento" />
                                <asp:TemplateField HeaderText="Acciones">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEditar" runat="server" CssClass="btn btn-primary btn-sm"
                                            CommandName="EditarEmpleado" CommandArgument='<%# Eval("IdEmpleado") %>'
                                            OnClientClick="return true;">
                                            <i class="fas fa-edit"></i> Editar
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lnkEliminar" runat="server" CssClass="btn btn-danger btn-sm"
                                            CommandName="EliminarEmpleado" CommandArgument='<%# Eval("IdEmpleado") %>'
                                            OnClientClick="return confirm('¿Está seguro que desea eliminar este empleado?');">
                                            <i class="fas fa-trash"></i> Eliminar
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content> 