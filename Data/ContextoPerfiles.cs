using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using PerfilesSA.Models;

namespace PerfilesSA.Data
{
    public class ContextoPerfiles : IDisposable
    {
        private readonly string cadenaConexion;

        public ContextoPerfiles()
        {
            cadenaConexion = ConfigurationManager.ConnectionStrings["PerfilesDB"].ConnectionString;
        }

        #region Departamentos
        public DataTable ObtenerTodosDepartamentos()
        {
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand comando = new SqlCommand("sp_GetAllDepartments", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    DataTable tabla = new DataTable();
                    conexion.Open();
                    tabla.Load(comando.ExecuteReader());
                    return tabla;
                }
            }
        }

        public DataTable ObtenerDepartamentosActivos()
        {
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand comando = new SqlCommand("sp_GetActiveDepartments", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    DataTable tabla = new DataTable();
                    conexion.Open();
                    tabla.Load(comando.ExecuteReader());
                    return tabla;
                }
            }
        }

        public Department ObtenerDepartamentoPorId(int idDepartamento)
        {
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand comando = new SqlCommand("SELECT DepartmentId, Name, Description, IsActive FROM Departments WHERE DepartmentId = @DepartmentId", conexion))
                {
                    comando.Parameters.AddWithValue("@DepartmentId", idDepartamento);
                    conexion.Open();
                    using (var lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            return new Department
                            {
                                DepartmentId = lector.GetInt32(0),
                                Name = lector.GetString(1),
                                Description = lector.IsDBNull(2) ? null : lector.GetString(2),
                                IsActive = lector.GetBoolean(3)
                            };
                        }
                        return null;
                    }
                }
            }
        }

        public void InsertarDepartamento(string nombre, string descripcion, bool estaActivo)
        {
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand comando = new SqlCommand("sp_InsertDepartment", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@Name", nombre);
                    comando.Parameters.AddWithValue("@Description", (object)descripcion ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@IsActive", estaActivo);

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarDepartamento(int idDepartamento, string nombre, string descripcion, bool estaActivo)
        {
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand comando = new SqlCommand("sp_UpdateDepartment", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@DepartmentId", idDepartamento);
                    comando.Parameters.AddWithValue("@Name", nombre);
                    comando.Parameters.AddWithValue("@Description", (object)descripcion ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@IsActive", estaActivo);

                    conexion.Open();
                    int resultado = comando.ExecuteNonQuery();
                    if (resultado == 0)
                    {
                        throw new Exception("No se encontró el departamento para actualizar.");
                    }
                }
            }
        }
        #endregion

        #region Empleados
        public DataTable ObtenerTodosEmpleados()
        {
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand comando = new SqlCommand("sp_GetAllEmployees", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    DataTable tabla = new DataTable();
                    conexion.Open();
                    tabla.Load(comando.ExecuteReader());
                    return tabla;
                }
            }
        }

        public DataTable ObtenerEmpleadosPorRangoFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand comando = new SqlCommand("sp_GetEmployeesByDateRange", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@StartDate", fechaInicio);
                    comando.Parameters.AddWithValue("@EndDate", fechaFin);

                    DataTable tabla = new DataTable();
                    conexion.Open();
                    tabla.Load(comando.ExecuteReader());
                    return tabla;
                }
            }
        }

        public bool EmpleadoEstaEnDepartamento(string dpi, int? excluirIdEmpleado = null)
        {
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand comando = new SqlCommand("sp_CheckEmployeeDepartment", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@DPI", dpi);
                    if (excluirIdEmpleado.HasValue)
                    {
                        comando.Parameters.AddWithValue("@ExcludeEmployeeId", excluirIdEmpleado.Value);
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@ExcludeEmployeeId", DBNull.Value);
                    }

                    conexion.Open();
                    var resultado = comando.ExecuteScalar();
                    return Convert.ToInt32(resultado) > 0;
                }
            }
        }

        public void InsertarEmpleado(Employee empleado)
        {
            if (EmpleadoEstaEnDepartamento(empleado.DPI))
            {
                throw new Exception("El empleado con este DPI ya está asignado a un departamento.");
            }

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand comando = new SqlCommand("sp_InsertEmployee", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@FirstName", empleado.FirstName);
                    comando.Parameters.AddWithValue("@LastName", empleado.LastName);
                    comando.Parameters.AddWithValue("@DPI", empleado.DPI);
                    comando.Parameters.AddWithValue("@BirthDate", empleado.BirthDate);
                    comando.Parameters.AddWithValue("@Gender", empleado.Gender);
                    comando.Parameters.AddWithValue("@HireDate", empleado.HireDate);
                    comando.Parameters.AddWithValue("@Address", (object)empleado.Address ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@NIT", (object)empleado.NIT ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@DepartmentId", empleado.DepartmentId);

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarEmpleado(Employee empleado)
        {
            if (EmpleadoEstaEnDepartamento(empleado.DPI, empleado.EmployeeId))
            {
                throw new Exception("Ya existe otro empleado con este DPI asignado a un departamento.");
            }

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand comando = new SqlCommand("sp_UpdateEmployee", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@EmployeeId", empleado.EmployeeId);
                    comando.Parameters.AddWithValue("@FirstName", empleado.FirstName);
                    comando.Parameters.AddWithValue("@LastName", empleado.LastName);
                    comando.Parameters.AddWithValue("@DPI", empleado.DPI);
                    comando.Parameters.AddWithValue("@BirthDate", empleado.BirthDate);
                    comando.Parameters.AddWithValue("@Gender", empleado.Gender);
                    comando.Parameters.AddWithValue("@HireDate", empleado.HireDate);
                    comando.Parameters.AddWithValue("@Address", (object)empleado.Address ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@NIT", (object)empleado.NIT ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@DepartmentId", empleado.DepartmentId);

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }
        #endregion

        public void Dispose()
        {
            
        }
    }
}