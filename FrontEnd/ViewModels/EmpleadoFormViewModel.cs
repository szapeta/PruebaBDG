using FrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.ViewModels
{
    public class EmpleadoFormViewModel
    {
        public Empleado Empleado { get; set; }

        public IEnumerable<SelectListItem> Puestos { get; set; }
        public IEnumerable<SelectListItem> Jefes { get; set; }
    }
}