using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class Empleado
    {
        public int idEmpleado { get; set; }
        public string Nombre { get; set; }
        public int? idPuesto { get; set; }
        public int? idJefe { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}