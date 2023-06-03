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

public class CategoriaController : ApiController
{
    private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    [HttpGet]
    public IHttpActionResult Get()
    {
        List<Categoria> categorias = new List<Categoria>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM categoria";
            MySqlCommand command = new MySqlCommand(query, connection);

            connection.Open();

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Categoria categoria = new Categoria();
                    categoria.CategoriaId = Convert.ToInt32(reader["CategoriaId"]);
                    categoria.NombreCategoria = reader["NombreCategoria"].ToString();
                    categorias.Add(categoria);
                }
            }
        }

        return Ok(categorias);
    }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            Categoria categoria = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM categoria WHERE CategoriaId = @CategoriaId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CategoriaId", id);

                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        categoria = new Categoria();
                        categoria.CategoriaId = Convert.ToInt32(reader["CategoriaId"]);
                        categoria.NombreCategoria = reader["NombreCategoria"].ToString();
                    }
                }
            }

            if (categoria == null)
            {
                return NotFound();
            }

            return Ok(categoria);
        }

        [HttpPost]
    public IHttpActionResult Post(Categoria categoria)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "INSERT INTO categoria (NombreCategoria) VALUES (@NombreCategoria)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@NombreCategoria", categoria.NombreCategoria);

            connection.Open();
            command.ExecuteNonQuery();
        }

        return Created("", categoria);
    }

    [HttpPut]
    public IHttpActionResult Put(int id, Categoria categoria)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "UPDATE categoria SET NombreCategoria = @NombreCategoria WHERE CategoriaId = @CategoriaId";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@NombreCategoria", categoria.NombreCategoria);
            command.Parameters.AddWithValue("@CategoriaId", id);

            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                return NotFound();
            }
        }

        return Ok(categoria);
    }

    [HttpDelete]
    public IHttpActionResult Delete(int id)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "DELETE FROM categoria WHERE CategoriaId = @CategoriaId";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@CategoriaId", id);

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