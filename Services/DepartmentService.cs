using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using PerfilesSA.Models;

namespace PerfilesSA.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["PerfilesDB"].ConnectionString;

        public DataTable GetAllDepartments()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand("sp_GetAllDepartments", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var adapter = new SqlDataAdapter(cmd);
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetActiveDepartments()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand("sp_GetActiveDepartments", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var adapter = new SqlDataAdapter(cmd);
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public Department GetDepartmentById(int departmentId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand("sp_GetDepartmentById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DepartmentId", departmentId);
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Department
                            {
                                DepartmentId = (int)reader["DepartmentId"],
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"] as string,
                                IsActive = (bool)reader["IsActive"],
                                CreatedDate = (DateTime)reader["CreatedDate"]
                            };
                        }
                        return null;
                    }
                }
            }
        }

        public void InsertDepartment(Department department)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand("sp_InsertDepartment", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    AddDepartmentParameters(cmd, department);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateDepartment(Department department)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand("sp_UpdateDepartment", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DepartmentId", department.DepartmentId);
                    cmd.Parameters.AddWithValue("@Name", department.Name);
                    cmd.Parameters.AddWithValue("@Description", (object)department.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", department.IsActive);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteDepartment(int departmentId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand("sp_DeleteDepartment", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DepartmentId", departmentId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ToggleDepartmentStatus(int departmentId, bool isActive)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand("sp_ToggleDepartmentStatus", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DepartmentId", departmentId);
                    cmd.Parameters.AddWithValue("@IsActive", isActive);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void AddDepartmentParameters(SqlCommand cmd, Department department)
        {
            cmd.Parameters.AddWithValue("@Name", department.Name);
            cmd.Parameters.AddWithValue("@Description", (object)department.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsActive", department.IsActive);
        }
    }
}