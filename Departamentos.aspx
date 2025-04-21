<%@ Page Title="Departamentos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Departamentos.aspx.cs" Inherits="PerfilesSA.Departamentos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-12">
            <h2 class="mb-4">Gestión de Departamentos</h2>

            <asp:HiddenField ID="hfIdDepartamento" runat="server" />
            
            <div class="card mb-4">
                <div class="card-header">
                    <h4 class="mb-0">Registro de Departamento</h4>
                </div>
                <div class="card-body">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-danger" ValidationGroup="vgDepartamento" />
                    <asp:Label ID="lblMensaje" runat="server" EnableViewState="false"></asp:Label>
                    
                    <div class="row mb-3">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="<%= txtNombre.ClientID %>" class="form-label">Nombre:</label>
                                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" MaxLength="100" />
                                <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                                    ControlToValidate="txtNombre"
                                    ErrorMessage="El nombre es requerido"
                                    Display="Dynamic"
                                    CssClass="text-danger"
                                    ValidationGroup="vgDepartamento">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="<%= txtDescripcion.ClientID %>" class="form-label">Descripción:</label>
                                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" MaxLength="200" />
                            </div>
                        </div>
                    </div>

                    <div class="row mb-3">
                        <div class="col-12">
                            <div class="form-check">
                                <asp:CheckBox ID="chkEstaActivo" runat="server" Checked="true" CssClass="form-check-input" />
                                <label class="form-check-label" for="<%= chkEstaActivo.ClientID %>">Activo</label>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-12">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" ValidationGroup="vgDepartamento" />
                            <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-secondary" OnClick="btnLimpiar_Click" CausesValidation="false" />
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
                        <asp:GridView ID="gvDepartamentos" runat="server" 
                            AutoGenerateColumns="False" 
                            CssClass="table table-striped table-bordered table-hover"
                            OnRowCommand="gvDepartamentos_RowCommand"
                            DataKeyNames="IdDepartamento"
                            EnableViewState="true">
                            <Columns>
                                <asp:BoundField DataField="IdDepartamento" HeaderText="ID" Visible="false" />
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                                <asp:TemplateField HeaderText="Estado">
                                    <ItemTemplate>
                                        <span class='<%# (bool)Eval("EstaActivo") ? "badge bg-success" : "badge bg-danger" %>'>
                                            <%# (bool)Eval("EstaActivo") ? "Activo" : "Inactivo" %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Acciones">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEditar" runat="server" CssClass="btn btn-primary btn-sm"
                                            CommandName="EditarDepartamento" CommandArgument='<%# Eval("IdDepartamento") %>'
                                            OnClientClick="return true;">
                                            <i class="fas fa-edit"></i> Editar
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lnkEliminar" runat="server" CssClass="btn btn-danger btn-sm"
                                            CommandName="EliminarDepartamento" CommandArgument='<%# Eval("IdDepartamento") %>'
                                            OnClientClick="return confirm('¿Está seguro que desea eliminar este departamento? No se podrá eliminar si tiene empleados asignados.');">
                                            <i class="fas fa-trash"></i> Eliminar
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lnkCambiarEstado" runat="server" 
                                            CssClass='<%# (bool)Eval("EstaActivo") ? "btn btn-warning btn-sm" : "btn btn-success btn-sm" %>'
                                            CommandName="CambiarEstado" CommandArgument='<%# Eval("IdDepartamento") %>'>
                                            <i class='<%# (bool)Eval("EstaActivo") ? "fas fa-ban" : "fas fa-check" %>'></i>
                                            <%# (bool)Eval("EstaActivo") ? "Desactivar" : "Activar" %>
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