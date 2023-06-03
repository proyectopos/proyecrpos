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
    public class PaisController : ApiController
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        [HttpGet]
        public IHttpActionResult Get()
        {
            List<Pais> paises = new List<Pais>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM pais";
                MySqlCommand command = new MySqlCommand(query, connection);

                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Pais pais = new Pais();
                        pais.PaisId = Convert.ToInt32(reader["PaisId"]);
                        pais.NombrePais = reader["NombrePais"].ToString();
                        paises.Add(pais);
                    }
                }
            }

            return Ok(paises);
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            Pais pais = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM pais WHERE PaisId = @PaisId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@PaisId", id);

                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        pais = new Pais();
                        pais.PaisId = Convert.ToInt32(reader["PaisId"]);
                        pais.NombrePais = reader["NombrePais"].ToString();
                    }
                }
            }

            if (pais == null)
            {
                return NotFound();
            }

            return Ok(pais);
        }

        [HttpPost]
        public IHttpActionResult Post(Pais pais)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO pais (NombrePais) VALUES (@NombrePais)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@NombrePais", pais.NombrePais);

                connection.Open();
                command.ExecuteNonQuery();
            }

            return Created("", pais);
        }

        [HttpPut]
        public IHttpActionResult Put(int id, Pais pais)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "UPDATE pais SET NombrePais = @NombrePais WHERE PaisId = @PaisId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@NombrePais", pais.NombrePais);
                command.Parameters.AddWithValue("@PaisId", id);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return NotFound();
                }
            }

            return Ok(pais);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "DELETE FROM pais WHERE PaisId = @PaisId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@PaisId", id);

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