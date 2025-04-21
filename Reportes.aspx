<%@ Page Title="Reportes" Language="C#" MasterPageFile="~/PlantillaMaestra.aspx" AutoEventWireup="true" CodeBehind="Reportes.aspx.cs" Inherits="PerfilesSA.Reportes" %>

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
    <div class="container-fluid">
        <h1 class="h3 mb-4 text-gray-800">Reportes de Empleados</h1>

        <div class="card shadow mb-4">
            <div class="card-header py-3">
                <h6 class="m-0 font-weight-bold text-primary">Filtros</h6>
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="ddlDepartamento">Departamento</label>
                            <asp:DropDownList ID="ddlDepartamento" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="txtFechaInicio">Fecha Inicio</label>
                            <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="txtFechaFin">Fecha Fin</label>
                            <asp:TextBox ID="txtFechaFin" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-md-12">
                        <asp:Button ID="btnGenerarReporte" runat="server" Text="Generar Reporte" CssClass="btn btn-primary" OnClick="btnGenerarReporte_Click" />
                        <asp:Button ID="btnExportarExcel" runat="server" Text="Exportar a Excel" CssClass="btn btn-success" OnClick="btnExportarExcel_Click" />
                    </div>
                </div>
            </div>
        </div>

        <asp:Label ID="lblMensaje" runat="server" CssClass="alert" Visible="false"></asp:Label>

        <div class="card shadow mb-4">
            <div class="card-header py-3">
                <h6 class="m-0 font-weight-bold text-primary">Resultados</h6>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <asp:GridView ID="gvReporte" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false"
                        OnRowDataBound="gvReporte_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="IdEmpleado" HeaderText="ID" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                            <asp:BoundField DataField="DPI" HeaderText="DPI" />
                            <asp:BoundField DataField="FechaNacimiento" HeaderText="Fecha de Nacimiento" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="FechaIngreso" HeaderText="Fecha de Ingreso" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="Departamento" HeaderText="Departamento" />
                            <asp:TemplateField HeaderText="Estado">
                                <ItemTemplate>
                                    <asp:Label ID="lblEstado" runat="server" CssClass="badge"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content> 