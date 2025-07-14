using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface IEmpleadoRepository
    {
        Task<IEnumerable<Empleado>> ObtenerTodosAsync();
        Task<Empleado> ObtenerPorIdAsync(int id);
        Task InsertarAsync(Empleado empleado);
        Task ActualizarAsync(int id, Empleado empleado);
    }
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly string _connectionString;

        public EmpleadoRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public async Task<IEnumerable<Empleado>> ObtenerTodosAsync()
        {
            var lista = new List<Empleado>();
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Empleado_ObtenerTodos", conn) { CommandType = CommandType.StoredProcedure })
            {
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new Empleado
                        {
                            idEmpleado = (int)reader["idempleado"],
                            Nombre = reader["nombre"].ToString(),
                            idPuesto = reader["idpuesto"] as int?,
                            idJefe = reader["idjefe"] as int?,
                            FechaRegistro = (DateTime)reader["fecha_registro"]
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<Empleado> ObtenerPorIdAsync(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Empleado_ObtenerPorId", conn) { CommandType = CommandType.StoredProcedure })
            {
                cmd.Parameters.AddWithValue("@idempleado", id);
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Empleado
                        {
                            idEmpleado = (int)reader["idempleado"],
                            Nombre = reader["nombre"].ToString(),
                            idPuesto = reader["idpuesto"] as int?,
                            idJefe = reader["idjefe"] as int?,
                            FechaRegistro = (DateTime)reader["fecha_registro"]
                        };
                    }
                }
            }
            return null;
        }

        public async Task InsertarAsync(Empleado empleado)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Empleado_Insertar", conn) { CommandType = CommandType.StoredProcedure })
            {
                cmd.Parameters.AddWithValue("@nombre", empleado.Nombre);
                cmd.Parameters.AddWithValue("@idpuesto", (object)empleado.idPuesto ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@idjefe", (object)empleado.idJefe ?? DBNull.Value);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task ActualizarAsync(int id, Empleado empleado)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Empleado_Actualizar", conn) { CommandType = CommandType.StoredProcedure })
            {
                cmd.Parameters.AddWithValue("@idempleado", id);
                cmd.Parameters.AddWithValue("@nombre", empleado.Nombre);
                cmd.Parameters.AddWithValue("@idpuesto", (object)empleado.idPuesto ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@idjefe", (object)empleado.idJefe ?? DBNull.Value);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}