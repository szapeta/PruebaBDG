using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.ViewModels
{
    public class EmpleadoListViewModel
    {
        public int IdEmpleado { get; set; }
        public string Nombre { get; set; }

        public string Puesto { get; set; }
        public string Jefe { get; set; }

        public string FechaRegistro { get; set; }
    }
}