<%@ Page Title="Departamentos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Departments.aspx.cs" Inherits="PerfilesSA.Departments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-12">
            <h2 class="mb-4">Gestión de Departamentos</h2>

            <asp:HiddenField ID="hfDepartmentId" runat="server" />
            
            <div class="card mb-4">
                <div class="card-header">
                    <h4 class="mb-0">Registro de Departamento</h4>
                </div>
                <div class="card-body">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-danger" ValidationGroup="vgDepartment" />
                    <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
                    
                    <div class="row mb-3">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="<%= txtName.ClientID %>" class="form-label">Nombre:</label>
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="100" />
                                <asp:RequiredFieldValidator ID="rfvName" runat="server"
                                    ControlToValidate="txtName"
                                    ErrorMessage="El nombre es requerido"
                                    Display="Dynamic"
                                    CssClass="text-danger"
                                    ValidationGroup="vgDepartment">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="<%= txtDescription.ClientID %>" class="form-label">Descripción:</label>
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" MaxLength="200" />
                            </div>
                        </div>
                    </div>

                    <div class="row mb-3">
                        <div class="col-12">
                            <div class="form-check">
                                <asp:CheckBox ID="chkIsActive" runat="server" Checked="true" CssClass="form-check-input" />
                                <label class="form-check-label" for="<%= chkIsActive.ClientID %>">Activo</label>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-12">
                            <asp:Button ID="btnSave" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="vgDepartment" />
                            <asp:Button ID="btnClear" runat="server" Text="Limpiar" CssClass="btn btn-secondary" OnClick="btnClear_Click" CausesValidation="false" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="card">
                <div class="card-header">
                    <h4 class="mb-0">Lista de Departamentos</h4>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <asp:GridView ID="gvDepartments" runat="server" 
                            AutoGenerateColumns="False" 
                            CssClass="table table-striped table-bordered table-hover"
                            OnRowCommand="gvDepartments_RowCommand"
                            DataKeyNames="DepartmentId"
                            EnableViewState="true">
                            <Columns>
                                <asp:BoundField DataField="DepartmentId" HeaderText="ID" Visible="false" />
                                <asp:BoundField DataField="Name" HeaderText="Nombre" />
                                <asp:BoundField DataField="Description" HeaderText="Descripción" />
                                <asp:TemplateField HeaderText="Estado">
                                    <ItemTemplate>
                                        <span class='<%# (bool)Eval("IsActive") ? "badge bg-success" : "badge bg-danger" %>'>
                                            <%# (bool)Eval("IsActive") ? "Activo" : "Inactivo" %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Acciones">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" CssClass="btn btn-primary btn-sm"
                                            CommandName="EditDepartment" CommandArgument='<%# Eval("DepartmentId") %>'
                                            OnClientClick="return true;">
                                            <i class="fas fa-edit"></i> Editar
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CssClass="btn btn-danger btn-sm"
                                            CommandName="DeleteDepartment" CommandArgument='<%# Eval("DepartmentId") %>'
                                            OnClientClick="return confirm('¿Está seguro que desea eliminar este departamento? No se podrá eliminar si tiene empleados asignados.');">
                                            <i class="fas fa-trash"></i> Eliminar
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lnkToggle" runat="server" 
                                            CssClass='<%# (bool)Eval("IsActive") ? "btn btn-warning btn-sm" : "btn btn-success btn-sm" %>'
                                            CommandName="ToggleStatus" CommandArgument='<%# Eval("DepartmentId") %>'>
                                            <i class='<%# (bool)Eval("IsActive") ? "fas fa-ban" : "fas fa-check" %>'></i>
                                            <%# (bool)Eval("IsActive") ? "Desactivar" : "Activar" %>
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