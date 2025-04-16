using System.Data;
using PerfilesSA.Models;

namespace PerfilesSA.Services
{
    public interface IDepartmentService
    {
        DataTable GetAllDepartments();
        DataTable GetActiveDepartments();
        Department GetDepartmentById(int departmentId);
        void InsertDepartment(Department department);
        void UpdateDepartment(Department department);
        void DeleteDepartment(int departmentId);
        void ToggleDepartmentStatus(int departmentId, bool isActive);
    }
}