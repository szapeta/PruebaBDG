using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApi.Models;
using WebApi.Repository;

namespace WebApi.Services
{
    public interface IEmpleadoService
    {
        Task<IEnumerable<Empleado>> ObtenerTodosAsync();
        Task<Empleado> ObtenerPorIdAsync(int id);
        Task InsertarAsync(Empleado empleado);
        Task ActualizarAsync(int id, Empleado empleado);
        Task EliminarAsync(int id);

    }

    public class EmpleadoService : IEmpleadoService
    {
        private readonly IEmpleadoRepository _repo;

        public EmpleadoService(IEmpleadoRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Empleado>> ObtenerTodosAsync() => _repo.ObtenerTodosAsync();
        public Task<Empleado> ObtenerPorIdAsync(int id) => _repo.ObtenerPorIdAsync(id);
        public Task InsertarAsync(Empleado empleado) => _repo.InsertarAsync(empleado);
        public Task ActualizarAsync(int id, Empleado empleado) => _repo.ActualizarAsync(id, empleado);
        public Task EliminarAsync(int id) => _repo.EliminarAsync(id);

    }
}