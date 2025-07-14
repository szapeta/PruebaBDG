using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [RoutePrefix("api/empleado")]
    public class EmpleadoController : ApiController
    {
        private readonly IEmpleadoService _service;

        public EmpleadoController(IEmpleadoService service)
        {
            _service = service;
        }

        /// <summary>Obtiene todos los empleados.</summary>
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Get()
        {
            var lista = await _service.ObtenerTodosAsync();
            return Ok(lista);
        }

        /// <summary>Obtiene un empleado por ID.</summary>
        [HttpGet, Route("{id:int}")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            var empleado = await _service.ObtenerPorIdAsync(id);
            if (empleado == null) return NotFound();
            return Ok(empleado);
        }

        /// <summary>Crea un nuevo empleado.</summary>
        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Crear([FromBody] Empleado empleado)
        {
            await _service.InsertarAsync(empleado);
            return Ok("Empleado creado correctamente.");
        }

        /// <summary>Actualiza un empleado.</summary>
        [HttpPut, Route("{id:int}")]
        public async Task<IHttpActionResult> Actualizar(int id, [FromBody] Empleado empleado)
        {
            await _service.ActualizarAsync(id, empleado);
            return Ok("Empleado actualizado.");
        }

        /// <summary>Elimina un empleado.</summary>
        [HttpDelete, Route("{id:int}")]
        public async Task<IHttpActionResult> Eliminar(int id)
        {
            try
            {
                await _service.EliminarAsync(id);
                return Ok("Empleado eliminado correctamente.");
            }
            catch (InvalidOperationException ex)
            {
                
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
               
                return InternalServerError(ex);
            }
        }
    }
}
