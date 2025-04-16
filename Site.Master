<%@ Master Language="C#" AutoEventWireup="true" CodeBehind="Site.master.cs" Inherits="PerfilesSA.SiteMaster" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title><%: Page.Title %> - PERFILES S.A.</title>

    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Font Awesome -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    
    <asp:ContentPlaceHolder ID="head" runat="server">
    </asp:ContentPlaceHolder>

    <style>
        body {
            padding-top: 20px;
            padding-bottom: 20px;
            background-color: #f8f9fa;
        }
        
        .navbar {
            margin-bottom: 20px;
            box-shadow: 0 2px 4px rgba(0,0,0,.1);
        }

        .navbar-dark {
            background-color: #1a237e !important;
        }
        
        .navbar-dark .navbar-nav .nav-link {
            color: rgba(255,255,255,.8);
            padding: 1rem;
            transition: all 0.3s ease;
        }

        .navbar-dark .navbar-nav .nav-link:hover,
        .navbar-dark .navbar-nav .nav-link.active {
            color: white;
            background-color: rgba(255,255,255,.1);
        }
        
        .validation-error {
            color: #dc3545;
            font-size: 0.875em;
            margin-top: 0.25rem;
        }
        
        .grid-view {
            width: 100%;
            margin-top: 20px;
            background-color: white;
            border-radius: 0.375rem;
            box-shadow: 0 0.125rem 0.25rem rgba(0,0,0,.075);
        }
        
        .form-group {
            margin-bottom: 1rem;
        }
        
        .btn {
            margin-right: 5px;
        }

        .card {
            box-shadow: 0 0.125rem 0.25rem rgba(0,0,0,.075);
        }

        .card-header {
            background-color: #1a237e !important;
            color: white;
        }
        
        footer {
            margin-top: 30px;
            padding: 20px 0;
            border-top: 1px solid #dee2e6;
            background-color: white;
        }
    </style>
</head>
<body>
    <form runat="server">
        <nav class="navbar navbar-expand-lg navbar-dark">
            <div class="container">
                <a class="navbar-brand" href='<%: ResolveUrl("~/Default.aspx") %>'>
                    <i class="fas fa-building me-2"></i>PERFILES S.A.
                </a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarNav">
                    <ul class="navbar-nav">
                        <li class="nav-item">
                            <asp:HyperLink runat="server" ID="lnkEmpleados" CssClass="nav-link">
                                <i class="fas fa-users me-1"></i> Empleados
                            </asp:HyperLink>
                        </li>
                        <li class="nav-item">
                            <asp:HyperLink runat="server" ID="lnkDepartamentos" CssClass="nav-link">
                                <i class="fas fa-sitemap me-1"></i> Departamentos
                            </asp:HyperLink>
                        </li>
                        <li class="nav-item">
                            <asp:HyperLink runat="server" ID="lnkReportes" CssClass="nav-link">
                                <i class="fas fa-chart-bar me-1"></i> Reportes
                            </asp:HyperLink>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>

        <main class="container">
            <asp:ContentPlaceHolder ID="MainContent" runat="server">
            </asp:ContentPlaceHolder>
        </main>

        <div class="container">
            <footer>
                <p class="text-center text-muted">&copy; <%: DateTime.Now.Year %> - Sistema de Control Administrativo - PERFILES S.A.</p>
            </footer>
        </div>

        <!-- Bootstrap Bundle with Popper -->
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
        <!-- jQuery -->
        <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    </form>
</body>
</html> 