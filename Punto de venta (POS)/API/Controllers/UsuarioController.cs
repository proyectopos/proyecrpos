using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using API.Models;
using BCrypt.Net;

using System;

namespace API.Controllers
{
    public class UsuarioController : ApiController
    {
        // GET api/<controller>
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        [HttpGet]
        public IHttpActionResult Get()
        {
            List<Usuario> usuarios = new List<Usuario>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM usuario AS u INNER JOIN rol AS r ON u.CodigoRol=r.CodigoRol";
                MySqlCommand command = new MySqlCommand(query, connection);

                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Usuario usuario = new Usuario();
                        usuario.CodigoUsuario = Convert.ToInt32(reader["CodigoUsuario"]);
                        usuario.Nombre = reader["Nombre"].ToString();
                        usuario.Apellido = reader["Apellido"].ToString();
                        usuario.Usuario_ = reader["Usuario"].ToString();
                        usuario.Contrasena = "****";
                        usuario.CodigoRol = Convert.ToInt32(reader["CodigoRol"]);
                        usuario.NombreRol = reader["NombreRol"].ToString();
                        usuarios.Add(usuario);
                    }
                }
            }

            return Ok(usuarios);
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            Usuario usuario = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM usuario AS u INNER JOIN rol AS r ON u.CodigoRol=r.CodigoRol WHERE CodigoUsuario = @CodigoUsuario";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CodigoUsuario", id);

                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new Usuario();
                        usuario.CodigoUsuario = Convert.ToInt32(reader["CodigoUsuario"]);
                        usuario.Nombre = reader["Nombre"].ToString();
                        usuario.Apellido = reader["Apellido"].ToString();
                        usuario.Usuario_ = reader["Usuario"].ToString();
                        usuario.Contrasena = "****";
                        usuario.CodigoRol = Convert.ToInt32(reader["CodigoRol"]);
                        usuario.NombreRol = reader["NombreRol"].ToString();
                    }
                }
            }

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        [HttpPost]
        public IHttpActionResult Post(Usuario usuario)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO usuario (Usuario, Nombre,Apellido, Contrasena, CodigoRol) VALUES (@Usuario_, @Nombre,@Apellido, @Contrasena, @CodigoRol)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Usuario_", usuario.Usuario_);
                command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                command.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                command.Parameters.AddWithValue("@Contrasena", BCrypt.Net.BCrypt.HashPassword(usuario.Contrasena));
                //bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(userInputPassword, hashedPassword);
                command.Parameters.AddWithValue("@CodigoRol", usuario.CodigoRol);

                connection.Open();
                command.ExecuteNonQuery();
            }

            return Created("", usuario);
        }

        [HttpPut]
        public IHttpActionResult Put(int id, Usuario usuario)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "UPDATE usuario SET Usuario=@Usuario_,Nombre=@Nombre,Apellido=@Apellido,CodigoRol=@CodigoRol WHERE CodigoUsuario = @CodigoUsuario";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Usuario_", usuario.Usuario_);
                command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                command.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                //command.Parameters.AddWithValue("@Contrasena", BCrypt.Net.BCrypt.HashPassword(usuario.Contrasena));
                
                command.Parameters.AddWithValue("@CodigoRol", usuario.CodigoRol);
                command.Parameters.AddWithValue("@CodigoUsuario", id);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return NotFound();
                }
            }

            return Ok(usuario);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "DELETE FROM usuario WHERE CodigoUsuario = @CodigoUsuario";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CodigoUsuario", id);

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