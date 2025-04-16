<%@ Page Title="Empleados" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PerfilesSA.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function calculateAge(birthDate) {
            const today = new Date();
            const birth = new Date(birthDate);
            let age = today.getFullYear() - birth.getFullYear();
            const monthDiff = today.getMonth() - birth.getMonth();

            if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birth.getDate())) {
                age--;
            }
            return age;
        }

        function calculateYearsOfService(hireDate) {
            const today = new Date();
            const hire = new Date(hireDate);
            let years = today.getFullYear() - hire.getFullYear();
            const monthDiff = today.getMonth() - hire.getMonth();

            if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < hire.getDate())) {
                years--;
            }
            return years;
        }

        function updateCalculatedFields() {
            const birthDateInput = document.getElementById('<%= txtBirthDate.ClientID %>');
            const hireDateInput = document.getElementById('<%= txtHireDate.ClientID %>');
            const ageLabel = document.getElementById('<%= lblAgeValue.ClientID %>');
            const yearsLabel = document.getElementById('<%= lblYearsOfServiceValue.ClientID %>');

            if (birthDateInput && birthDateInput.value) {
                const age = calculateAge(birthDateInput.value);
                if (ageLabel) ageLabel.textContent = age;
            }

            if (hireDateInput && hireDateInput.value) {
                const years = calculateYearsOfService(hireDateInput.value);
                if (yearsLabel) yearsLabel.textContent = years;
            }
        }

        // Agregar listeners para actualización en tiempo real
        document.addEventListener('DOMContentLoaded', function () {
            const birthDateInput = document.getElementById('<%= txtBirthDate.ClientID %>');
            const hireDateInput = document.getElementById('<%= txtHireDate.ClientID %>');

            if (birthDateInput) {
                birthDateInput.addEventListener('change', updateCalculatedFields);
            }
            if (hireDateInput) {
                hireDateInput.addEventListener('change', updateCalculatedFields);
            }

            // Calcular valores iniciales
            updateCalculatedFields();
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-12">
            <h2 class="mb-4">Gestión de Empleados</h2>
            
            <asp:Label ID="lblMessage" runat="server" CssClass="d-none"></asp:Label>
            
            <asp:HiddenField ID="hfEmployeeId" runat="server" Value="" />
            
            <div class="card mb-4">
                <div class="card-header">
                    <h4 class="mb-0">Registro de Empleado</h4>
                </div>
                <div class="card-body">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-danger" ValidationGroup="vgEmployee" />
                    
                    <div class="row mb-3">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtFirstName" class="form-label">Nombres:</label>
                                <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" 
                                    ControlToValidate="txtFirstName" 
                                    ErrorMessage="El nombre es requerido" 
                                    Display="Dynamic" 
                                    CssClass="text-danger"
                                    ValidationGroup="vgEmployee">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtLastName" class="form-label">Apellidos:</label>
                                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="rfvLastName" runat="server" 
                                    ControlToValidate="txtLastName" 
                                    ErrorMessage="El apellido es requerido" 
                                    Display="Dynamic" 
                                    CssClass="validation-error"
                                    ValidationGroup="vgEmployee">*</asp:RequiredFieldValidator>
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
                                    ValidationGroup="vgEmployee">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtBirthDate" class="form-label">Fecha de Nacimiento:</label>
                                <asp:TextBox ID="txtBirthDate" runat="server" CssClass="form-control" TextMode="Date" AutoPostBack="true" OnTextChanged="txtBirthDate_TextChanged" />
                                <asp:RequiredFieldValidator ID="rfvBirthDate" runat="server" 
                                    ControlToValidate="txtBirthDate" 
                                    ErrorMessage="La fecha de nacimiento es requerida" 
                                    Display="Dynamic" 
                                    CssClass="validation-error"
                                    ValidationGroup="vgEmployee">*</asp:RequiredFieldValidator>
                                <div class="form-text">Edad: <asp:Label ID="lblAgeValue" runat="server" /></div>
                            </div>
                        </div>
                    </div>

                    <div class="row mb-3">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="ddlGender" class="form-label">Género:</label>
                                <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="Seleccione..." Value="" />
                                    <asp:ListItem Text="Masculino" Value="M" />
                                    <asp:ListItem Text="Femenino" Value="F" />
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvGender" runat="server" 
                                    ControlToValidate="ddlGender" 
                                    ErrorMessage="El género es requerido" 
                                    Display="Dynamic" 
                                    CssClass="validation-error"
                                    ValidationGroup="vgEmployee">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtHireDate" class="form-label">Fecha de Contratación:</label>
                                <asp:TextBox ID="txtHireDate" runat="server" CssClass="form-control" TextMode="Date" AutoPostBack="true" OnTextChanged="txtHireDate_TextChanged" />
                                <asp:RequiredFieldValidator ID="rfvHireDate" runat="server" 
                                    ControlToValidate="txtHireDate" 
                                    ErrorMessage="La fecha de contratación es requerida" 
                                    Display="Dynamic" 
                                    CssClass="validation-error"
                                    ValidationGroup="vgEmployee">*</asp:RequiredFieldValidator>
                                <div class="form-text">Años de servicio: <asp:Label ID="lblYearsOfServiceValue" runat="server" /></div>
                            </div>
                        </div>
                    </div>

                    <div class="row mb-3">
                        <div class="col-12">
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="txtAddress" CssClass="form-label">Dirección (Opcional)</asp:Label>
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control"></asp:TextBox>
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
                                <label for="ddlDepartment" class="form-label">Departamento:</label>
                                <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-select" />
                                <asp:RequiredFieldValidator ID="rfvDepartment" runat="server" 
                                    ControlToValidate="ddlDepartment" 
                                    ErrorMessage="El departamento es requerido" 
                                    Display="Dynamic" 
                                    CssClass="validation-error"
                                    ValidationGroup="vgEmployee">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group mt-4">
                                <div class="form-check">
                                    <asp:CheckBox ID="chkIsActive" runat="server" Checked="true" CssClass="form-check-input" />
                                    <asp:Label runat="server" AssociatedControlID="chkIsActive" CssClass="form-check-label">
                                        Empleado Activo
                                    </asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-12">
                            <asp:Button ID="btnSave" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="vgEmployee" UseSubmitBehavior="true" />
                            <asp:Button ID="btnClear" runat="server" Text="Limpiar" CssClass="btn btn-secondary" OnClick="btnClear_Click" CausesValidation="false" />
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
                        <asp:GridView ID="gvEmployees" runat="server" 
                            AutoGenerateColumns="False" 
                            CssClass="table table-striped table-bordered table-hover"
                            OnRowCommand="gvEmployees_RowCommand"
                            DataKeyNames="EmployeeId">
                            <Columns>
                                <asp:BoundField DataField="FirstName" HeaderText="Nombres" />
                                <asp:BoundField DataField="LastName" HeaderText="Apellidos" />
                                <asp:BoundField DataField="DPI" HeaderText="DPI" />
                                <asp:BoundField DataField="BirthDate" HeaderText="Fecha Nacimiento" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="Gender" HeaderText="Género" />
                                <asp:BoundField DataField="HireDate" HeaderText="Fecha Contratación" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="Address" HeaderText="Dirección" />
                                <asp:BoundField DataField="NIT" HeaderText="NIT" />
                                <asp:BoundField DataField="DepartmentName" HeaderText="Departamento" />
                                <asp:TemplateField HeaderText="Acciones">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" CssClass="btn btn-primary btn-sm"
                                            CommandName="EditEmployee" CommandArgument='<%# Eval("EmployeeId") %>'
                                            OnClientClick="return true;">
                                            <i class="fas fa-edit"></i> Editar
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CssClass="btn btn-danger btn-sm"
                                            CommandName="DeleteEmployee" CommandArgument='<%# Eval("EmployeeId") %>'
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