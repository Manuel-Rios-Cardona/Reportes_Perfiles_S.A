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

-- Crear tablas principales
-- Tabla de Departamentoss
CREATE TABLE Departamentos (
    IdDepartamento INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    EstaActivo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME NULL
);
GO

-- Tabla de Empleados
CREATE TABLE Empleados (
    IdEmpleado INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL,
    Apellido NVARCHAR(50) NOT NULL,
    DPI NVARCHAR(13) NOT NULL UNIQUE,
    FechaNacimiento DATE NOT NULL,
    Genero NVARCHAR(1) NOT NULL CHECK (Genero IN ('M', 'F')),
    FechaIngreso DATE NOT NULL,
    Direccion NVARCHAR(200) NULL,
    NIT NVARCHAR(20) NULL,
    IdDepartamento INT NOT NULL,
    EstaActivo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME NULL,
    CONSTRAINT FK_Empleados_Departamentos FOREIGN KEY (IdDepartamento) 
        REFERENCES Departamentos(IdDepartamento)
);
GO

-- Procedimientos para gestión de empleados
-- Obtener lista completa de empleados
CREATE PROCEDURE sp_ObtenerTodosEmpleados
AS
BEGIN
    SELECT 
        e.IdEmpleado,
        e.Nombre,
        e.Apellido,
        e.DPI,
        e.FechaNacimiento,
        e.Genero,
        e.FechaIngreso,
        e.Direccion,
        e.NIT,
        e.IdDepartamento,
        d.Nombre AS NombreDepartamento,
        d.EstaActivo AS DepartamentoActivo,
        e.EstaActivo
    FROM Empleados e
    INNER JOIN Departamentos d ON e.IdDepartamento = d.IdDepartamento
    ORDER BY e.Apellido, e.Nombre;
END;
GO

-- Filtrar empleados por departamento
CREATE PROCEDURE sp_ObtenerEmpleadosPorDepartamento
    @IdDepartamento INT
AS
BEGIN
    SELECT 
        e.IdEmpleado,
        e.Nombre,
        e.Apellido,
        e.DPI,
        e.FechaNacimiento,
        e.Genero,
        e.FechaIngreso,
        e.Direccion,
        e.NIT,
        e.IdDepartamento,
        d.Nombre AS NombreDepartamento,
        d.EstaActivo AS DepartamentoActivo,
        e.EstaActivo
    FROM Empleados e
    INNER JOIN Departamentos d ON e.IdDepartamento = d.IdDepartamento
    WHERE e.IdDepartamento = @IdDepartamento
    ORDER BY e.Apellido, e.Nombre;
END;
GO

-- Filtrar empleados por fecha de ingreso
CREATE PROCEDURE sp_ObtenerEmpleadosPorRangoFechas
    @FechaInicio DATE,
    @FechaFin DATE
AS
BEGIN
    SELECT 
        e.IdEmpleado,
        e.Nombre,
        e.Apellido,
        e.DPI,
        e.FechaNacimiento,
        e.Genero,
        e.FechaIngreso,
        e.Direccion,
        e.NIT,
        e.IdDepartamento,
        d.Nombre AS NombreDepartamento,
        d.EstaActivo AS DepartamentoActivo,
        e.EstaActivo
    FROM Empleados e
    INNER JOIN Departamentos d ON e.IdDepartamento = d.IdDepartamento
    WHERE e.FechaIngreso BETWEEN @FechaInicio AND @FechaFin
    ORDER BY e.Apellido, e.Nombre;
END;
GO

-- Agregar nuevo empleado
CREATE PROCEDURE sp_InsertarEmpleado
    @Nombre NVARCHAR(50),
    @Apellido NVARCHAR(50),
    @DPI NVARCHAR(13),
    @FechaNacimiento DATE,
    @Genero NVARCHAR(1),
    @FechaIngreso DATE,
    @Direccion NVARCHAR(200),
    @NIT NVARCHAR(20),
    @IdDepartamento INT
AS
BEGIN
    BEGIN TRY
        INSERT INTO Empleados (
            Nombre, Apellido, DPI, FechaNacimiento, 
            Genero, FechaIngreso, Direccion, NIT, IdDepartamento
        )
        VALUES (
            @Nombre, @Apellido, @DPI, @FechaNacimiento,
            @Genero, @FechaIngreso, @Direccion, @NIT, @IdDepartamento
        );

        SELECT SCOPE_IDENTITY() AS IdEmpleado;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Modificar datos de empleado
CREATE PROCEDURE sp_ActualizarEmpleado
    @IdEmpleado INT,
    @Nombre NVARCHAR(50),
    @Apellido NVARCHAR(50),
    @DPI NVARCHAR(13),
    @FechaNacimiento DATE,
    @Genero NVARCHAR(1),
    @FechaIngreso DATE,
    @Direccion NVARCHAR(200),
    @NIT NVARCHAR(20),
    @IdDepartamento INT
AS
BEGIN
    BEGIN TRY
        UPDATE Empleados
        SET 
            Nombre = @Nombre,
            Apellido = @Apellido,
            DPI = @DPI,
            FechaNacimiento = @FechaNacimiento,
            Genero = @Genero,
            FechaIngreso = @FechaIngreso,
            Direccion = @Direccion,
            NIT = @NIT,
            IdDepartamento = @IdDepartamento,
            FechaActualizacion = GETDATE()
        WHERE IdEmpleado = @IdEmpleado;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Eliminar empleado
CREATE PROCEDURE sp_EliminarEmpleado
    @IdEmpleado INT
AS
BEGIN
    BEGIN TRY
        DELETE FROM Empleados
        WHERE IdEmpleado = @IdEmpleado;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Procedimientos para gestión de departamentos
-- Obtener lista de departamentos
CREATE PROCEDURE sp_ObtenerTodosDepartamentos
AS
BEGIN
    SELECT 
        IdDepartamento,
        Nombre,
        EstaActivo,
        FechaCreacion,
        FechaActualizacion
    FROM Departamentos
    ORDER BY Nombre;
END;
GO

-- Agregar nuevo departamento
CREATE PROCEDURE sp_InsertarDepartamento
    @Nombre NVARCHAR(100)
AS
BEGIN
    BEGIN TRY
        INSERT INTO Departamentos (Nombre)
        VALUES (@Nombre);

        SELECT SCOPE_IDENTITY() AS IdDepartamento;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Modificar departamento
CREATE PROCEDURE sp_ActualizarDepartamento
    @IdDepartamento INT,
    @Nombre NVARCHAR(100)
AS
BEGIN
    BEGIN TRY
        UPDATE Departamentos
        SET 
            Nombre = @Nombre,
            FechaActualizacion = GETDATE()
        WHERE IdDepartamento = @IdDepartamento;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Eliminar departamento
CREATE PROCEDURE sp_EliminarDepartamento
    @IdDepartamento INT
AS
BEGIN
    BEGIN TRY
        -- Verificar si hay empleados asignados
        IF EXISTS (SELECT 1 FROM Empleados WHERE IdDepartamento = @IdDepartamento)
        BEGIN
            THROW 50001, 'No se puede eliminar el departamento porque tiene empleados asignados.', 1;
        END

        DELETE FROM Departamentos
        WHERE IdDepartamento = @IdDepartamento;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Activar/Desactivar departamento
CREATE PROCEDURE sp_CambiarEstadoDepartamento
    @IdDepartamento INT
AS
BEGIN
    BEGIN TRY
        UPDATE Departamentos
        SET 
            EstaActivo = ~EstaActivo,
            FechaActualizacion = GETDATE()
        WHERE IdDepartamento = @IdDepartamento;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Datos de prueba
-- Departamentos iniciales
INSERT INTO Departamentos (Nombre) VALUES 
('Administración'),
('Contabilidad'),
('Recursos Humanos'),
('Ventas'),
('Operaciones');
GO

-- Índices para mejorar rendimiento
-- Índice para DPI (campo único)
CREATE NONCLUSTERED INDEX IX_Empleados_DPI
ON Empleados(DPI);
GO

-- Índice para relación con departamentos
CREATE NONCLUSTERED INDEX IX_Empleados_IdDepartamento
ON Empleados(IdDepartamento);
GO

-- Índice para búsqueda por nombre de departamento
CREATE NONCLUSTERED INDEX IX_Departamentos_Nombre
ON Departamentos(Nombre);
GO