using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using PerfilesSA.Models;

namespace PerfilesSA.Services
{
    public class ServicioDepartamento : IServicioDepartamento
    {
        private readonly string cadenaConexion = ConfigurationManager.ConnectionStrings["PerfilesDB"].ConnectionString;

        public DataTable ObtenerTodosDepartamentos()
        {
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("sp_GetAllDepartments", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    var adaptador = new SqlDataAdapter(comando);
                    var tabla = new DataTable();
                    adaptador.Fill(tabla);
                    return tabla;
                }
            }
        }

        public DataTable ObtenerDepartamentosActivos()
        {
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("sp_GetActiveDepartments", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    var adaptador = new SqlDataAdapter(comando);
                    var tabla = new DataTable();
                    adaptador.Fill(tabla);
                    return tabla;
                }
            }
        }

        public Department ObtenerDepartamentoPorId(int idDepartamento)
        {
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("sp_GetDepartmentById", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@DepartmentId", idDepartamento);
                    conexion.Open();

                    using (var lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            return new Department
                            {
                                DepartmentId = (int)lector["DepartmentId"],
                                Name = lector["Name"].ToString(),
                                Description = lector["Description"] as string,
                                IsActive = (bool)lector["IsActive"],
                                CreatedDate = (DateTime)lector["CreatedDate"]
                            };
                        }
                        return null;
                    }
                }
            }
        }

        public void InsertarDepartamento(Department departamento)
        {
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("sp_InsertDepartment", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    AgregarParametrosDepartamento(comando, departamento);
                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarDepartamento(Department departamento)
        {
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("sp_UpdateDepartment", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@DepartmentId", departamento.DepartmentId);
                    comando.Parameters.AddWithValue("@Name", departamento.Name);
                    comando.Parameters.AddWithValue("@Description", (object)departamento.Description ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@IsActive", departamento.IsActive);
                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void EliminarDepartamento(int idDepartamento)
        {
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("sp_DeleteDepartment", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@DepartmentId", idDepartamento);
                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void CambiarEstadoDepartamento(int idDepartamento, bool estaActivo)
        {
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("sp_ToggleDepartmentStatus", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@DepartmentId", idDepartamento);
                    comando.Parameters.AddWithValue("@IsActive", estaActivo);
                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        private void AgregarParametrosDepartamento(SqlCommand comando, Department departamento)
        {
            comando.Parameters.AddWithValue("@Name", departamento.Name);
            comando.Parameters.AddWithValue("@Description", (object)departamento.Description ?? DBNull.Value);
            comando.Parameters.AddWithValue("@IsActive", departamento.IsActive);
        }
    }
}