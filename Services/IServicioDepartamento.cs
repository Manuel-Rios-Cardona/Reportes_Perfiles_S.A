using System.Data;
using PerfilesSA.Models;

namespace PerfilesSA.Services
{
    public interface IServicioDepartamento
    {
        DataTable ObtenerTodosDepartamentos();
        DataTable ObtenerDepartamentosActivos();
        Department ObtenerDepartamentoPorId(int idDepartamento);
        void InsertarDepartamento(Department departamento);
        void ActualizarDepartamento(Department departamento);
        void EliminarDepartamento(int idDepartamento);
        void CambiarEstadoDepartamento(int idDepartamento, bool estaActivo);
    }
}