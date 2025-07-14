using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class Puesto
    {
        public int idPuesto { get; set; }

        [Required(ErrorMessage = "El nombre del puesto es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; }
    }
}