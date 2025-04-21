using System;
using System.Data;
using PerfilesSA.Models;

namespace PerfilesSA.Services
{
    public interface IServicioEmpleado
    {
        DataTable ObtenerTodosEmpleados();
        DataTable ObtenerEmpleadosPorRangoFechas(DateTime fechaInicio, DateTime fechaFin);
        DataTable ObtenerEmpleadosPorDepartamento(int idDepartamento);
        Employee ObtenerEmpleadoPorId(int idEmpleado);
        void InsertarEmpleado(Employee empleado);
        void ActualizarEmpleado(Employee empleado);
        bool EmpleadoEstaEnDepartamento(string dpi, int? excluirIdEmpleado = null);
        void EliminarEmpleado(int idEmpleado);
        int CalcularEdad(DateTime fechaNacimiento);
        int CalcularAniosServicio(DateTime fechaContratacion);
    }
}