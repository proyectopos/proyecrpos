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
    public class EstadoController : ApiController
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        [HttpGet]
        public IHttpActionResult Get(int id,string tmp)
        {
            List<Estado> estados = new List<Estado>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM estado AS e INNER JOIN pais AS p ON p.PaisId=e.PaisId WHERE e.PaisId = @PaisId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@PaisId", id);

                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Estado estado = new Estado();
                        estado.EstadoId = Convert.ToInt32(reader["EstadoId"]);
                        estado.NombreEstado = reader["NombreEstado"].ToString();
                        estado.PaisId = Convert.ToInt32(reader["PaisId"]);
                        estado.NombrePais = reader["NombrePais"].ToString();
                        estados.Add(estado);
                    }
                }
            }

            return Ok(estados);
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            Estado estado = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM estado AS e INNER JOIN pais AS p ON p.PaisId=e.PaisId WHERE e.EstadoId = @EstadoId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@EstadoId", id);

                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        estado = new Estado();
                        estado.EstadoId = Convert.ToInt32(reader["EstadoId"]);
                        estado.NombreEstado = reader["NombreEstado"].ToString();
                        estado.PaisId = Convert.ToInt32(reader["PaisId"]);
                        estado.NombrePais = reader["NombrePais"].ToString();
                    }
                }
            }

            if (estado == null)
            {
                return NotFound();
            }

            return Ok(estado);
        }

        [HttpPost]
        public IHttpActionResult Post(Estado estado)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO estado (NombreEstado,PaisId) VALUES (@NombreEstado,@PaisId)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@NombreEstado", estado.NombreEstado);
                command.Parameters.AddWithValue("@PaisId", estado.PaisId);

                connection.Open();
                command.ExecuteNonQuery();
            }

            return Created("", estado);
        }
        
        [HttpPut]
        public IHttpActionResult Put(int id, Estado estado)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "UPDATE estado SET NombreEstado = @NombreEstado WHERE EstadoId = @EstadoId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@NombreEstado", estado.NombreEstado);
                command.Parameters.AddWithValue("@EstadoId", id);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return NotFound();
                }
            }

            return Ok(estado);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "DELETE FROM estado WHERE EstadoId = @EstadoId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@EstadoId", id);

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