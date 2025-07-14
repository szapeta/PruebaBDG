using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;
using WebApi.Repository;

namespace WebApi.Controllers
{
    [RoutePrefix("api/puestos")]
    public class PuestoController : ApiController
    {
        private readonly PuestoRepository _repo = new PuestoRepository();

        [HttpGet]
        [Route("")]
        public IEnumerable<Puesto> Get()
        {
            return _repo.ObtenerTodos();
        }

        /// <summary>
        /// Crea un puesto
        /// </summary>
        /// <param name="puesto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] Puesto puesto)
        {
            _repo.Insertar(puesto);
            return Ok("Puesto creado.");
        }
    }
}
