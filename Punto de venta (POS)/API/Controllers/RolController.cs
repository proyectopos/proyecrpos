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
    public class RolController : ApiController
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        [HttpGet]
        public IHttpActionResult Get()
        {
            List<Rol> roles = new List<Rol>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM rol";
                MySqlCommand command = new MySqlCommand(query, connection);

                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Rol rol = new Rol();
                        rol.CodigoRol = Convert.ToInt32(reader["CodigoRol"]);
                        rol.NombreRol = reader["NombreRol"].ToString();
                        roles.Add(rol);
                    }
                }
            }

            return Ok(roles);
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            Rol rol = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM rol WHERE CodigoRol = @CodigoRol";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CodigoRol", id);

                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        rol = new Rol();
                        rol.CodigoRol = Convert.ToInt32(reader["CodigoRol"]);
                        rol.NombreRol = reader["NombreRol"].ToString();
                    }
                }
            }

            if (rol == null)
            {
                return NotFound();
            }

            return Ok(rol);
        }

        [HttpPost]
        public IHttpActionResult Post(Rol rol)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO rol (NombreRol) VALUES (@NombreRol)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@NombreRol", rol.NombreRol);

                connection.Open();
                command.ExecuteNonQuery();
            }

            return Created("", rol);
        }

        [HttpPut]
        public IHttpActionResult Put(int id, Rol rol)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "UPDATE rol SET NombreRol = @NombreRol WHERE CodigoRol = @CodigoRol";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@NombreRol", rol.NombreRol);
                command.Parameters.AddWithValue("@CodigoRol", id);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return NotFound();
                }
            }

            return Ok(rol);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "DELETE FROM rol WHERE CodigoRol = @CodigoRol";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CodigoRol", id);

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