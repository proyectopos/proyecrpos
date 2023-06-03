using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Estado
    {
        public int EstadoId { get; set; }
        public string NombreEstado { get; set; }

        public int PaisId { get; set; }
        public string NombrePais { get; set; }
    }
}