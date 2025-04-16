-- Script de Base de Datos - Sistema de Control Administrativo
-- PERFILES S.A.

-- Limpiar base de datos si ya existe
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'PerfilesDB')
BEGIN
    ALTER DATABASE PerfilesDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE PerfilesDB;
END
GO

CREATE DATABASE PerfilesDB;
GO

USE PerfilesDB;
GO

-- Crear tablas principaless
-- Tabla de Departamentos
CREATE TABLE Departments (
    DepartmentId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL
);
GO

-- Tabla de Empleados
CREATE TABLE Employees (
    EmployeeId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    DPI NVARCHAR(13) NOT NULL UNIQUE,
    BirthDate DATE NOT NULL,
    Gender NVARCHAR(1) NOT NULL CHECK (Gender IN ('M', 'F')),
    HireDate DATE NOT NULL,
    Address NVARCHAR(200) NULL,
    NIT NVARCHAR(20) NULL,
    DepartmentId INT NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,
    CONSTRAINT FK_Employees_Departments FOREIGN KEY (DepartmentId) 
        REFERENCES Departments(DepartmentId)
);
GO

-- Procedimientos para gestión de empleados
-- Obtener lista completa de empleados
CREATE PROCEDURE sp_GetAllEmployees
AS
BEGIN
    SELECT 
        e.EmployeeId,
        e.FirstName,
        e.LastName,
        e.DPI,
        e.BirthDate,
        e.Gender,
        e.HireDate,
        e.Address,
        e.NIT,
        e.DepartmentId,
        d.Name AS DepartmentName,
        d.IsActive AS DepartmentIsActive,
        e.IsActive
    FROM Employees e
    INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
    ORDER BY e.LastName, e.FirstName;
END;
GO

-- Filtrar empleados por departamento
CREATE PROCEDURE sp_GetEmployeesByDepartment
    @DepartmentId INT
AS
BEGIN
    SELECT 
        e.EmployeeId,
        e.FirstName,
        e.LastName,
        e.DPI,
        e.BirthDate,
        e.Gender,
        e.HireDate,
        e.Address,
        e.NIT,
        e.DepartmentId,
        d.Name AS DepartmentName,
        d.IsActive AS DepartmentIsActive,
        e.IsActive
    FROM Employees e
    INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
    WHERE e.DepartmentId = @DepartmentId
    ORDER BY e.LastName, e.FirstName;
END;
GO

-- Filtrar empleados por fecha de ingreso
CREATE PROCEDURE sp_GetEmployeesByDateRange
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT 
        e.EmployeeId,
        e.FirstName,
        e.LastName,
        e.DPI,
        e.BirthDate,
        e.Gender,
        e.HireDate,
        e.Address,
        e.NIT,
        e.DepartmentId,
        d.Name AS DepartmentName,
        d.IsActive AS DepartmentIsActive,
        e.IsActive
    FROM Employees e
    INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
    WHERE e.HireDate BETWEEN @StartDate AND @EndDate
    ORDER BY e.LastName, e.FirstName;
END;
GO

-- Agregar nuevo empleado
CREATE PROCEDURE sp_InsertEmployee
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @DPI NVARCHAR(13),
    @BirthDate DATE,
    @Gender NVARCHAR(1),
    @HireDate DATE,
    @Address NVARCHAR(200),
    @NIT NVARCHAR(20),
    @DepartmentId INT
AS
BEGIN
    BEGIN TRY
        INSERT INTO Employees (
            FirstName, LastName, DPI, BirthDate, 
            Gender, HireDate, Address, NIT, DepartmentId
        )
        VALUES (
            @FirstName, @LastName, @DPI, @BirthDate,
            @Gender, @HireDate, @Address, @NIT, @DepartmentId
        );

        SELECT SCOPE_IDENTITY() AS EmployeeId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Modificar datos de empleado
CREATE PROCEDURE sp_UpdateEmployee
    @EmployeeId INT,
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @DPI NVARCHAR(13),
    @BirthDate DATE,
    @Gender NVARCHAR(1),
    @HireDate DATE,
    @Address NVARCHAR(200),
    @NIT NVARCHAR(20),
    @DepartmentId INT
AS
BEGIN
    BEGIN TRY
        UPDATE Employees
        SET 
            FirstName = @FirstName,
            LastName = @LastName,
            DPI = @DPI,
            BirthDate = @BirthDate,
            Gender = @Gender,
            HireDate = @HireDate,
            Address = @Address,
            NIT = @NIT,
            DepartmentId = @DepartmentId,
            UpdatedAt = GETDATE()
        WHERE EmployeeId = @EmployeeId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Eliminar empleado
CREATE PROCEDURE sp_DeleteEmployee
    @EmployeeId INT
AS
BEGIN
    BEGIN TRY
        DELETE FROM Employees
        WHERE EmployeeId = @EmployeeId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Procedimientos para gestión de departamentos
-- Obtener lista de departamentos
CREATE PROCEDURE sp_GetAllDepartments
AS
BEGIN
    SELECT 
        DepartmentId,
        Name,
        IsActive,
        CreatedAt,
        UpdatedAt
    FROM Departments
    ORDER BY Name;
END;
GO

-- Agregar nuevo departamento
CREATE PROCEDURE sp_InsertDepartment
    @Name NVARCHAR(100)
AS
BEGIN
    BEGIN TRY
        INSERT INTO Departments (Name)
        VALUES (@Name);

        SELECT SCOPE_IDENTITY() AS DepartmentId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Modificar departamento
CREATE PROCEDURE sp_UpdateDepartment
    @DepartmentId INT,
    @Name NVARCHAR(100)
AS
BEGIN
    BEGIN TRY
        UPDATE Departments
        SET 
            Name = @Name,
            UpdatedAt = GETDATE()
        WHERE DepartmentId = @DepartmentId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Eliminar departamento
CREATE PROCEDURE sp_DeleteDepartment
    @DepartmentId INT
AS
BEGIN
    BEGIN TRY
        -- Verificar si hay empleados asignados
        IF EXISTS (SELECT 1 FROM Employees WHERE DepartmentId = @DepartmentId)
        BEGIN
            THROW 50001, 'No se puede eliminar el departamento porque tiene empleados asignados.', 1;
        END

        DELETE FROM Departments
        WHERE DepartmentId = @DepartmentId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Activar/Desactivar departamento
CREATE PROCEDURE sp_ToggleDepartmentStatus
    @DepartmentId INT
AS
BEGIN
    BEGIN TRY
        UPDATE Departments
        SET 
            IsActive = ~IsActive,
            UpdatedAt = GETDATE()
        WHERE DepartmentId = @DepartmentId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Datos de prueba
-- Departamentos iniciales
INSERT INTO Departments (Name) VALUES 
('Administración'),
('Contabilidad'),
('Recursos Humanos'),
('Ventas'),
('Operaciones');
GO

-- Índices para mejorar rendimiento
-- Índice para DPI (campo único)
CREATE NONCLUSTERED INDEX IX_Employees_DPI
ON Employees(DPI);
GO

-- Índice para relación con departamentos
CREATE NONCLUSTERED INDEX IX_Employees_DepartmentId
ON Employees(DepartmentId);
GO

-- Índice para búsqueda por nombre de departamento
CREATE NONCLUSTERED INDEX IX_Departments_Name
ON Departments(Name);
GO