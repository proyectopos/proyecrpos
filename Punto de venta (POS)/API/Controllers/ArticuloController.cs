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
    public class ArticuloController : ApiController
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        [HttpGet]
        public IHttpActionResult Get()
        {
            List<Articulo> articulos = new List<Articulo>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM articulo AS a INNER JOIN categoria AS c ON a.CategoriaId = c.CategoriaId";
                MySqlCommand command = new MySqlCommand(query, connection);

                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Articulo articulo = new Articulo();
                        articulo.ArticuloId = Convert.ToInt32(reader["ArticuloId"]);
                        articulo.CategoriaId = Convert.ToInt32(reader["CategoriaId"]);
                        articulo.NombreCategoria =reader["NombreCategoria"].ToString();
                        articulo.NombreArticulo = reader["NombreArticulo"].ToString();
                        articulo.Descripcion = reader["Descripcion"].ToString();
                        articulo.Stock = Convert.ToInt32(reader["Stock"]);
                        articulo.Precio = Convert.ToDecimal(reader["Precio"]);
                        articulo.Imagen = reader["Imagen"].ToString();
                        articulos.Add(articulo);
                    }
                }
            }

            return Ok(articulos);
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            Articulo articulo = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM  articulo AS a INNER JOIN categoria AS c ON a.CategoriaId = c.CategoriaId WHERE ArticuloId = @ArticuloId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ArticuloId", id);

                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        articulo = new Articulo();
                        articulo.ArticuloId = Convert.ToInt32(reader["ArticuloId"]);
                        articulo.CategoriaId = Convert.ToInt32(reader["CategoriaId"]);
                        articulo.NombreCategoria = reader["NombreCategoria"].ToString();
                        articulo.NombreArticulo = reader["NombreArticulo"].ToString();
                        articulo.Descripcion = reader["Descripcion"].ToString();
                        articulo.Stock = Convert.ToInt32(reader["Stock"]);
                        articulo.Precio = Convert.ToDecimal(reader["Precio"]);
                        articulo.Imagen = reader["Imagen"].ToString();
                    }
                }
            }

            if (articulo == null)
            {
                return NotFound();
            }

            return Ok(articulo);
        }

        [HttpPost]
        public IHttpActionResult Post(Articulo articulo)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO articulo (CategoriaId, NombreArticulo, Descripcion, Stock, Precio, Imagen) VALUES (@CategoriaId, @NombreArticulo, @Descripcion, @Stock, @Precio, @Imagen)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CategoriaId", articulo.CategoriaId);
                command.Parameters.AddWithValue("@NombreArticulo", articulo.NombreArticulo);
                command.Parameters.AddWithValue("@Descripcion", articulo.Descripcion);
                command.Parameters.AddWithValue("@Stock", articulo.Stock);
                command.Parameters.AddWithValue("@Precio", articulo.Precio);
                command.Parameters.AddWithValue("@Imagen", articulo.Imagen);
                connection.Open();
                command.ExecuteNonQuery();
            }

            return Created("", articulo);
        }

        [HttpPut]
        public IHttpActionResult Put(int id, Articulo articulo)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "UPDATE `articulo` SET CategoriaId=@CategoriaId,NombreArticulo=@NombreArticulo,Descripcion=@Descripcion,Stock=@Stock,Precio=@Precio,Imagen=@Imagen WHERE ArticuloId = @ArticuloId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CategoriaId", articulo.CategoriaId);
                command.Parameters.AddWithValue("@NombreArticulo", articulo.NombreArticulo);
                command.Parameters.AddWithValue("@Descripcion", articulo.Descripcion);
                command.Parameters.AddWithValue("@Stock", articulo.Stock);
                command.Parameters.AddWithValue("@Precio", articulo.Precio);
                command.Parameters.AddWithValue("@Imagen", articulo.Imagen);
                command.Parameters.AddWithValue("@ArticuloId", id);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return NotFound();
                }
            }

            return Ok(articulo);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "DELETE FROM articulo WHERE ArticuloId = @ArticuloId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ArticuloId", id);

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