using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using PerfilesSA.Models;

namespace PerfilesSA.Data
{
    public class PerfilesContext : IDisposable
    {
        private readonly string connectionString;

        public PerfilesContext()
        {
            connectionString = ConfigurationManager.ConnectionStrings["PerfilesDB"].ConnectionString;
        }

        #region Departments
        public DataTable GetAllDepartments()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAllDepartments", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                    return dt;
                }
            }
        }

        public DataTable GetActiveDepartments()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetActiveDepartments", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                    return dt;
                }
            }
        }

        public Department GetDepartmentById(int departmentId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT DepartmentId, Name, Description, IsActive FROM Departments WHERE DepartmentId = @DepartmentId", conn))
                {
                    cmd.Parameters.AddWithValue("@DepartmentId", departmentId);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Department
                            {
                                DepartmentId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                                IsActive = reader.GetBoolean(3)
                            };
                        }
                        return null;
                    }
                }
            }
        }

        public void InsertDepartment(string name, string description, bool isActive)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertDepartment", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Description", (object)description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", isActive);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateDepartment(int departmentId, string name, string description, bool isActive)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateDepartment", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DepartmentId", departmentId);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Description", (object)description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", isActive);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result == 0)
                    {
                        throw new Exception("No se encontró el departamento para actualizar.");
                    }
                }
            }
        }
        #endregion

        #region Employees
        public DataTable GetAllEmployees()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAllEmployees", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                    return dt;
                }
            }
        }

        public DataTable GetEmployeesByDateRange(DateTime startDate, DateTime endDate)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetEmployeesByDateRange", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);

                    DataTable dt = new DataTable();
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                    return dt;
                }
            }
        }

        public bool IsEmployeeInAnyDepartment(string dpi, int? excludeEmployeeId = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_CheckEmployeeDepartment", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DPI", dpi);
                    if (excludeEmployeeId.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@ExcludeEmployeeId", excludeEmployeeId.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ExcludeEmployeeId", DBNull.Value);
                    }

                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result) > 0;
                }
            }
        }

        public void InsertEmployee(Employee employee)
        {
            if (IsEmployeeInAnyDepartment(employee.DPI))
            {
                throw new Exception("El empleado con este DPI ya está asignado a un departamento.");
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertEmployee", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                    cmd.Parameters.AddWithValue("@DPI", employee.DPI);
                    cmd.Parameters.AddWithValue("@BirthDate", employee.BirthDate);
                    cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                    cmd.Parameters.AddWithValue("@HireDate", employee.HireDate);
                    cmd.Parameters.AddWithValue("@Address", (object)employee.Address ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NIT", (object)employee.NIT ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DepartmentId", employee.DepartmentId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateEmployee(Employee employee)
        {
            if (IsEmployeeInAnyDepartment(employee.DPI, employee.EmployeeId))
            {
                throw new Exception("Ya existe otro empleado con este DPI asignado a un departamento.");
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateEmployee", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                    cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                    cmd.Parameters.AddWithValue("@DPI", employee.DPI);
                    cmd.Parameters.AddWithValue("@BirthDate", employee.BirthDate);
                    cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                    cmd.Parameters.AddWithValue("@HireDate", employee.HireDate);
                    cmd.Parameters.AddWithValue("@Address", (object)employee.Address ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NIT", (object)employee.NIT ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DepartmentId", employee.DepartmentId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

        public void Dispose()
        {
            
        }
    }
}