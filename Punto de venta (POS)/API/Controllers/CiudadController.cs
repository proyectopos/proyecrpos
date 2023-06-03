using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using API.Models;
using System;

namespace API.Controllers
{
    public class CiudadController : ApiController
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        [HttpGet]
        public IHttpActionResult Get(int id, string tmp)
        {
            List<Ciudad> ciudades = new List<Ciudad>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM ciudad AS c INNER JOIN estado AS e ON c.EstadoId=e.EstadoId WHERE c.EstadoId = @EstadoId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@EstadoId", id);

                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Ciudad ciudad = new Ciudad();
                        ciudad.CiudadId = Convert.ToInt32(reader["CiudadId"]);
                        ciudad.NombreCiudad = reader["NombreCiudad"].ToString();
                        ciudad.EstadoId = Convert.ToInt32(reader["EstadoId"]);
                        ciudad.NombreEstado = reader["NombreEstado"].ToString();
                        ciudades.Add(ciudad);
                    }
                }
            }

            return Ok(ciudades);
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            Ciudad ciudad = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM ciudad AS c INNER JOIN estado AS e ON c.EstadoId=e.EstadoId WHERE c.CiudadId = @CiudadId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CiudadId", id);

                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ciudad = new Ciudad();
                        ciudad.CiudadId = Convert.ToInt32(reader["CiudadId"]);
                        ciudad.NombreCiudad = reader["NombreCiudad"].ToString();
                        ciudad.EstadoId = Convert.ToInt32(reader["EstadoId"]);
                        ciudad.NombreEstado = reader["NombreEstado"].ToString();
                    }
                }
            }

            if (ciudad == null)
            {
                return NotFound();
            }

            return Ok(ciudad);
        }

        [HttpPost]
        public IHttpActionResult Post(Ciudad ciudad)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO ciudad (NombreCiudad,EstadoId) VALUES (@NombreCiudad,@EstadoId)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@NombreCiudad", ciudad.NombreCiudad);
                command.Parameters.AddWithValue("@EstadoId", ciudad.EstadoId);

                connection.Open();
                command.ExecuteNonQuery();
            }

            return Created("", ciudad);
        }

        [HttpPut]
        public IHttpActionResult Put(int id, Ciudad ciudad)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "UPDATE ciudad SET NombreCiudad = @NombreCiudad WHERE CiudadId = @CiudadId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@NombreCiudad", ciudad.NombreCiudad);
                command.Parameters.AddWithValue("@CiudadId", id);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return NotFound();
                }
            }

            return Ok(ciudad);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "DELETE FROM ciudad WHERE CiudadId = @CiudadId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CiudadId", id);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return NotFound();
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}