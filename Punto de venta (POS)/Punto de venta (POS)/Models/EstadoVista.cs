using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Punto_de_venta__POS_.Models
{
    public class EstadoVista
    {
        public Pais pais { get; set; }
        public List<Estado> estados { get; set; }
    }
}