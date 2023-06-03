using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Articulo
    {
        public int ArticuloId { get; set; }
        public int CategoriaId { get; set; }
        public string NombreCategoria { get; set; }
        public string NombreArticulo { get; set; }
        public string Descripcion { get; set; }
        public int Stock { get; set; }
        public decimal Precio { get; set; }
        public string Imagen { get; set; }
        public HttpPostedFileBase ImagenForm { get; set; }

    }
}