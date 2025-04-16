using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using PerfilesSA.Models;

namespace PerfilesSA.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["PerfilesDB"].ConnectionString;

        public DataTable GetAllEmployees()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand("sp_GetAllEmployees", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var adapter = new SqlDataAdapter(cmd);
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetEmployeesByDateRange(DateTime startDate, DateTime endDate)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand("sp_GetEmployeesByDateRange", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);
                    var adapter = new SqlDataAdapter(cmd);
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetEmployeesByDepartment(int departmentId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand("sp_GetEmployeesByDepartment", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DepartmentId", departmentId);
                    var adapter = new SqlDataAdapter(cmd);
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public Employee GetEmployeeById(int employeeId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand("sp_GetEmployeeById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Employee
                            {
                                EmployeeId = (int)reader["EmployeeId"],
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                DPI = reader["DPI"].ToString(),
                                BirthDate = (DateTime)reader["BirthDate"],
                                Gender = reader["Gender"].ToString(),
                                HireDate = (DateTime)reader["HireDate"],
                                Address = reader["Address"] as string,
                                NIT = reader["NIT"] as string,
                                DepartmentId = (int)reader["DepartmentId"],
                                DepartmentName = reader["DepartmentName"].ToString()
                            };
                        }
                        return null;
                    }
                }
            }
        }

        public void InsertEmployee(Employee employee)
        {
            if (!employee.IsValidDPI())
            {
                throw new ArgumentException("El DPI debe contener exactamente 13 números, sin letras ni caracteres especiales.");
            }

            if (IsEmployeeInAnyDepartment(employee.DPI))
            {
                throw new Exception("El empleado con este DPI ya está asignado a un departamento.");
            }

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    using (var cmd = new SqlCommand("sp_InsertEmployee", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        AddEmployeeParameters(cmd, employee);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("String or binary data would be truncated"))
                {
                    throw new Exception("El DPI ingresado excede la longitud permitida. Debe tener exactamente 13 dígitos.");
                }
                throw;
            }
        }

        public void UpdateEmployee(Employee employee)
        {
            if (!employee.IsValidDPI())
            {
                throw new ArgumentException("El DPI debe contener exactamente 13 números, sin letras ni caracteres especiales.");
            }

            if (IsEmployeeInAnyDepartment(employee.DPI, employee.EmployeeId))
            {
                throw new Exception("Ya existe otro empleado con este DPI asignado a un departamento.");
            }

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    using (var cmd = new SqlCommand("sp_UpdateEmployee", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                        AddEmployeeParameters(cmd, employee);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("String or binary data would be truncated"))
                {
                    throw new Exception("El DPI ingresado excede la longitud permitida. Debe tener exactamente 13 dígitos.");
                }
                throw;
            }
        }

        public void DeleteEmployee(int employeeId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand("sp_DeleteEmployee", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool IsEmployeeInAnyDepartment(string dpi, int? excludeEmployeeId = null)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand("sp_CheckEmployeeDepartment", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DPI", dpi);
                    cmd.Parameters.AddWithValue("@ExcludeEmployeeId", (object)excludeEmployeeId ?? DBNull.Value);
                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        public int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age))
                age--;
            return age;
        }

        public int CalculateYearsOfService(DateTime hireDate)
        {
            var today = DateTime.Today;
            var years = today.Year - hireDate.Year;
            if (hireDate > today.AddYears(-years))
                years--;
            return years;
        }

        private void AddEmployeeParameters(SqlCommand cmd, Employee employee)
        {
            cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
            cmd.Parameters.AddWithValue("@LastName", employee.LastName);
            cmd.Parameters.AddWithValue("@DPI", employee.DPI?.Trim());
            cmd.Parameters.AddWithValue("@BirthDate", employee.BirthDate);
            cmd.Parameters.AddWithValue("@Gender", employee.Gender);
            cmd.Parameters.AddWithValue("@HireDate", employee.HireDate);
            cmd.Parameters.AddWithValue("@Address", (object)employee.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@NIT", (object)employee.NIT ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DepartmentId", employee.DepartmentId);
            cmd.Parameters.AddWithValue("@IsActive", employee.IsActive);
        }
    }
}