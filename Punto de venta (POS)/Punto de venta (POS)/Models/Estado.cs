using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Punto_de_venta__POS_.Models
{
    public class Estado
    {
        public int EstadoId { get; set; }
        public string NombreEstado { get; set; }

        public int PaisId { get; set; }
        public string NombrePais { get; set; }
    }
}