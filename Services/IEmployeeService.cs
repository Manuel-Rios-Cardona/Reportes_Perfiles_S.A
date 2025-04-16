
using System;
using System.Data;
using PerfilesSA.Models;

namespace PerfilesSA.Services
{
    public interface IEmployeeService
    {
        DataTable GetAllEmployees();
        DataTable GetEmployeesByDateRange(DateTime startDate, DateTime endDate);
        DataTable GetEmployeesByDepartment(int departmentId);
        Employee GetEmployeeById(int employeeId);
        void InsertEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        bool IsEmployeeInAnyDepartment(string dpi, int? excludeEmployeeId = null);
        void DeleteEmployee(int employeeId);
        int CalculateAge(DateTime birthDate);
        int CalculateYearsOfService(DateTime hireDate);
    }
}