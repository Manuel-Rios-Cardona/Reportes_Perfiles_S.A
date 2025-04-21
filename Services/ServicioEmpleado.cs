using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using PerfilesSA.Models;

namespace PerfilesSA.Services
{
    public class ServicioEmpleado : IServicioEmpleado
    {
        private readonly string cadenaConexion = ConfigurationManager.ConnectionStrings["PerfilesDB"].ConnectionString;

        public DataTable ObtenerTodosEmpleados()
        {
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("sp_GetAllEmployees", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    var adaptador = new SqlDataAdapter(comando);
                    var tabla = new DataTable();
                    adaptador.Fill(tabla);
                    return tabla;
                }
            }
        }

        public DataTable ObtenerEmpleadosPorRangoFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("sp_GetEmployeesByDateRange", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@StartDate", fechaInicio);
                    comando.Parameters.AddWithValue("@EndDate", fechaFin);
                    var adaptador = new SqlDataAdapter(comando);
                    var tabla = new DataTable();
                    adaptador.Fill(tabla);
                    return tabla;
                }
            }
        }

        public DataTable ObtenerEmpleadosPorDepartamento(int idDepartamento)
        {
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("sp_GetEmployeesByDepartment", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@DepartmentId", idDepartamento);
                    var adaptador = new SqlDataAdapter(comando);
                    var tabla = new DataTable();
                    adaptador.Fill(tabla);
                    return tabla;
                }
            }
        }

        public Employee ObtenerEmpleadoPorId(int idEmpleado)
        {
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("sp_GetEmployeeById", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@EmployeeId", idEmpleado);
                    conexion.Open();

                    using (var lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            return new Employee
                            {
                                EmployeeId = (int)lector["EmployeeId"],
                                FirstName = lector["FirstName"].ToString(),
                                LastName = lector["LastName"].ToString(),
                                DPI = lector["DPI"].ToString(),
                                BirthDate = (DateTime)lector["BirthDate"],
                                Gender = lector["Gender"].ToString(),
                                HireDate = (DateTime)lector["HireDate"],
                                Address = lector["Address"] as string,
                                NIT = lector["NIT"] as string,
                                DepartmentId = (int)lector["DepartmentId"],
                                DepartmentName = lector["DepartmentName"].ToString()
                            };
                        }
                        return null;
                    }
                }
            }
        }

        public void InsertarEmpleado(Employee empleado)
        {
            if (!empleado.IsValidDPI())
            {
                throw new ArgumentException("El DPI debe contener exactamente 13 números, sin letras ni caracteres especiales.");
            }

            if (EmpleadoEstaEnDepartamento(empleado.DPI))
            {
                throw new Exception("El empleado con este DPI ya está asignado a un departamento.");
            }

            try
            {
                using (var conexion = new SqlConnection(cadenaConexion))
                {
                    using (var comando = new SqlCommand("sp_InsertEmployee", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        AgregarParametrosEmpleado(comando, empleado);
                        conexion.Open();
                        comando.ExecuteNonQuery();
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

        public void ActualizarEmpleado(Employee empleado)
        {
            if (!empleado.IsValidDPI())
            {
                throw new ArgumentException("El DPI debe contener exactamente 13 números, sin letras ni caracteres especiales.");
            }

            if (EmpleadoEstaEnDepartamento(empleado.DPI, empleado.EmployeeId))
            {
                throw new Exception("Ya existe otro empleado con este DPI asignado a un departamento.");
            }

            try
            {
                using (var conexion = new SqlConnection(cadenaConexion))
                {
                    using (var comando = new SqlCommand("sp_UpdateEmployee", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@EmployeeId", empleado.EmployeeId);
                        AgregarParametrosEmpleado(comando, empleado);
                        conexion.Open();
                        comando.ExecuteNonQuery();
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

        public void EliminarEmpleado(int idEmpleado)
        {
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("sp_DeleteEmployee", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@EmployeeId", idEmpleado);
                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        public bool EmpleadoEstaEnDepartamento(string dpi, int? excluirIdEmpleado = null)
        {
            using (var conexion = new SqlConnection(cadenaConexion))
            {
                using (var comando = new SqlCommand("sp_CheckEmployeeDepartment", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@DPI", dpi);
                    comando.Parameters.AddWithValue("@ExcludeEmployeeId", (object)excluirIdEmpleado ?? DBNull.Value);
                    conexion.Open();
                    return Convert.ToInt32(comando.ExecuteScalar()) > 0;
                }
            }
        }

        public int CalcularEdad(DateTime fechaNacimiento)
        {
            var hoy = DateTime.Today;
            var edad = hoy.Year - fechaNacimiento.Year;
            if (fechaNacimiento > hoy.AddYears(-edad))
                edad--;
            return edad;
        }

        public int CalcularAniosServicio(DateTime fechaContratacion)
        {
            var hoy = DateTime.Today;
            var anios = hoy.Year - fechaContratacion.Year;
            if (fechaContratacion > hoy.AddYears(-anios))
                anios--;
            return anios;
        }

        private void AgregarParametrosEmpleado(SqlCommand comando, Employee empleado)
        {
            comando.Parameters.AddWithValue("@FirstName", empleado.FirstName);
            comando.Parameters.AddWithValue("@LastName", empleado.LastName);
            comando.Parameters.AddWithValue("@DPI", empleado.DPI?.Trim());
            comando.Parameters.AddWithValue("@BirthDate", empleado.BirthDate);
            comando.Parameters.AddWithValue("@Gender", empleado.Gender);
            comando.Parameters.AddWithValue("@HireDate", empleado.HireDate);
            comando.Parameters.AddWithValue("@Address", (object)empleado.Address ?? DBNull.Value);
            comando.Parameters.AddWithValue("@NIT", (object)empleado.NIT ?? DBNull.Value);
            comando.Parameters.AddWithValue("@DepartmentId", empleado.DepartmentId);
            comando.Parameters.AddWithValue("@IsActive", empleado.IsActive);
        }
    }
}