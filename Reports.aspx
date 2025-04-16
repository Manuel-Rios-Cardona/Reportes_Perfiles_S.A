<%@ Page Title="Reportes" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="PerfilesSA.Reports" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
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

        function updateAges() {
            const rows = document.querySelectorAll('[data-birth-date]');
            rows.forEach(row => {
                const birthDate = row.getAttribute('data-birth-date');
                const age = calculateAge(birthDate);
                row.querySelector('.age-cell').textContent = age;
            });

            const serviceRows = document.querySelectorAll('[data-hire-date]');
            serviceRows.forEach(row => {
                const hireDate = row.getAttribute('data-hire-date');
                const years = calculateYearsOfService(hireDate);
                row.querySelector('.years-service-cell').textContent = years;
            });
        }

        document.addEventListener('DOMContentLoaded', updateAges);
    </script>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2 class="mb-4">Reportes de Empleados</h2>
        
        <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>

        <div class="card">
            <div class="card-header bg-primary text-white">
                <h3 class="card-title mb-0">Filtros de Reporte</h3>
            </div>
            <div class="card-body">
                <div class="row mb-3">
                    <div class="col-md-4">
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="ddlDepartment" CssClass="form-label">Departamento:</asp:Label>
                            <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-select">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="txtStartDate" CssClass="form-label">Fecha Inicio:</asp:Label>
                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="txtEndDate" CssClass="form-label">Fecha Fin:</asp:Label>
                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <asp:Button ID="btnGenerateReport" runat="server" Text="Generar Reporte" 
                            CssClass="btn btn-primary me-2" OnClick="btnGenerateReport_Click" />
                        <asp:Button ID="btnExportExcel" runat="server" Text="Exportar a Excel" 
                            CssClass="btn btn-success" OnClick="btnExportExcel_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div class="card mt-4">
            <div class="card-header bg-primary text-white">
                <h3 class="card-title mb-0">Resultados del Reporte</h3>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <asp:GridView ID="gvReport" runat="server" CssClass="table table-striped table-bordered table-hover" 
                        AutoGenerateColumns="False" GridLines="None" OnRowDataBound="gvReport_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="FirstName" HeaderText="Nombres" />
                            <asp:BoundField DataField="LastName" HeaderText="Apellidos" />
                            <asp:BoundField DataField="DPI" HeaderText="DPI" />
                            <asp:BoundField DataField="BirthDate" HeaderText="Fecha Nacimiento" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:TemplateField HeaderText="Edad">
                                <ItemTemplate>
                                    <span class="age-cell"></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Gender" HeaderText="Género" />
                            <asp:BoundField DataField="HireDate" HeaderText="Fecha Ingreso" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:TemplateField HeaderText="Años de Servicio">
                                <ItemTemplate>
                                    <span class="years-service-cell"></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Address" HeaderText="Dirección" />
                            <asp:BoundField DataField="NIT" HeaderText="NIT" />
                            <asp:BoundField DataField="DepartmentName" HeaderText="Departamento" />
                            <asp:TemplateField HeaderText="Estado">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" CssClass="badge"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="alert alert-info">No se encontraron registros.</div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content> 