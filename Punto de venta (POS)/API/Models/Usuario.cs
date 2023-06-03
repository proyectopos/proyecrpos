using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Usuario
    {
        public int CodigoUsuario { get; set; }
        public string Usuario_ { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Contrasena { get; set; }
        public int CodigoRol { get; set; }
        public string NombreRol { get; set; }
    }
}