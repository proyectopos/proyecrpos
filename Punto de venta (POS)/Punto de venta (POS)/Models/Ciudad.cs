﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Punto_de_venta__POS_.Models
{
    public class Ciudad
    {
        public int CiudadId { get; set; }
        public string NombreCiudad { get; set; }
        public int EstadoId { get; set; }
        public string NombreEstado { get; set; }
    }
}