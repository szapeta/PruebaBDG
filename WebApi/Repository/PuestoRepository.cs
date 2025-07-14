using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApi.Models;

namespace WebApi.Repository
{
    public class PuestoRepository
    {
        private readonly string _connectionString;

        public PuestoRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public List<Puesto> ObtenerTodos()
        {
            var puestos = new List<Puesto>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_puesto_getall", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var puesto = new Puesto
                        {
                            idPuesto = reader.GetInt32(reader.GetOrdinal("idPuesto")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre"))
                        };
                        puestos.Add(puesto);
                    }
                }
            }

            return puestos;

        }

        public void Insertar(Puesto puesto)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_puesto_insertar", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", puesto.Nombre);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}