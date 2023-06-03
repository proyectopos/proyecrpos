using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using API.Models;
using System;
using BCrypt.Net;

namespace API.Controllers
{
    public class LoginController : ApiController
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

      
        [HttpPost]
        public IHttpActionResult Post(Login login)
        {

            Usuario usuario = new Usuario();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM usuario AS u INNER JOIN rol AS r ON u.CodigoRol=r.CodigoRol WHERE Usuario = @Usuario_";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Usuario_", login.Usuario_);

                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(login.Contrasena, reader["Contrasena"].ToString());
                        if (isPasswordCorrect)
                        {
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
            }

            return Ok(usuario);
        }

       
    }
}