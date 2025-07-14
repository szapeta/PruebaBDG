using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class Empleado
    {
        public int IdEmpleado { get; set; }
        public string Nombre { get; set; }
        public int IdPuesto { get; set; }
        public int? IdJefe { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}