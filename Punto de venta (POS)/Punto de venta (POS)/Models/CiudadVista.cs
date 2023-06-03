using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Punto_de_venta__POS_.Models
{
    public class CiudadVista
    {
        public Estado estado { get; set; }
        public List<Ciudad> ciudades { get; set; }
    }
}