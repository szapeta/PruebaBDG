using FrontEnd.Models;
using FrontEnd.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly string apiEmpleado = "https://localhost:44397/api/empleado";
        private readonly string apiPuesto = "https://localhost:44397/api/puestos";

        public async Task<ActionResult> Index()
        {
            var empleadosVM = new List<EmpleadoListViewModel>();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiEmpleado);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var empleados = JsonConvert.DeserializeObject<List<Empleado>>(json);

                    // Obtener los puestos y jefes para traducirlos
                    var puestos = await ObtenerPuestosAsync();
                    var empleadosTodos = empleados.ToDictionary(e => e.IdEmpleado, e => e.Nombre);

                    foreach (var emp in empleados)
                    {
                        empleadosVM.Add(new EmpleadoListViewModel
                        {
                            IdEmpleado = emp.IdEmpleado,
                            Nombre = emp.Nombre,
                            Puesto = puestos.FirstOrDefault(p => p.Value == emp.IdPuesto.ToString()).Text,
                            Jefe = emp.IdJefe.HasValue && empleadosTodos.ContainsKey(emp.IdJefe.Value)
                                ? empleadosTodos[emp.IdJefe.Value]
                                : "(Sin jefe)",
                            FechaRegistro = emp.FechaRegistro.ToString("yyyy-MM-dd HH:mm")
                        });
                    }
                }
            }

            return View(empleadosVM);
        }

        public async Task<ActionResult> Create()
        {
            var viewModel = new EmpleadoFormViewModel
            {
                Empleado = new Empleado { FechaRegistro = DateTime.Now },
                Puestos = await ObtenerPuestosAsync(),
                Jefes = await ObtenerJefesAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Create(EmpleadoFormViewModel viewModel)
        {
            using (var client = new HttpClient())
            {
                var empleado = viewModel.Empleado;
                var json = JsonConvert.SerializeObject(empleado);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiEmpleado, content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["mensaje"] = "Empleado creado correctamente.";
                    return RedirectToAction("Create");
                }

                ModelState.AddModelError("", "Error al crear el empleado.");
                viewModel.Puestos = await ObtenerPuestosAsync();
                viewModel.Jefes = await ObtenerJefesAsync();
                return View(viewModel);
            }
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerPuestosAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiPuesto);
                if (!response.IsSuccessStatusCode) return new List<SelectListItem>();

                var data = await response.Content.ReadAsStringAsync();
                var puestos = JsonConvert.DeserializeObject<List<Puesto>>(data);

                var items = new List<SelectListItem>();
                foreach (var p in puestos)
                {
                    items.Add(new SelectListItem { Value = p.idPuesto.ToString(), Text = p.Nombre });
                }

                return items;
            }
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerJefesAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiEmpleado);
                if (!response.IsSuccessStatusCode) return new List<SelectListItem>();

                var data = await response.Content.ReadAsStringAsync();
                var jefes = JsonConvert.DeserializeObject<List<Empleados>>(data);

                var items = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "-- Sin jefe --" }
                };

                foreach (var j in jefes)
                {
                    items.Add(new SelectListItem { Value = j.IdEmpleado.ToString(), Text = j.Nombre });
                }

                return items;
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{apiEmpleado}/{id}");
                if (!response.IsSuccessStatusCode)
                    return HttpNotFound();

                var json = await response.Content.ReadAsStringAsync();
                var empleado = JsonConvert.DeserializeObject<Empleado>(json);

                var viewModel = new EmpleadoFormViewModel
                {
                    Empleado = empleado,
                    Puestos = await ObtenerPuestosAsync(),
                    Jefes = await ObtenerJefesAsync()
                };

                return View(viewModel);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EmpleadoFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Puestos = await ObtenerPuestosAsync();
                viewModel.Jefes = await ObtenerJefesAsync();
                return View(viewModel);
            }

            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(viewModel.Empleado);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"{apiEmpleado}/{viewModel.Empleado.IdEmpleado}", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["mensaje"] = "Empleado actualizado correctamente.";
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Error al actualizar el empleado.");
                viewModel.Puestos = await ObtenerPuestosAsync();
                viewModel.Jefes = await ObtenerJefesAsync();
                return View(viewModel);
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{apiEmpleado}/{id}");
                if (!response.IsSuccessStatusCode)
                    return HttpNotFound();

                var json = await response.Content.ReadAsStringAsync();
                var empleado = JsonConvert.DeserializeObject<Empleado>(json);

                var puestos = await ObtenerPuestosAsync();
                var puestoNombre = puestos.FirstOrDefault(p => p.Value == empleado.IdPuesto.ToString())?.Text ?? "(Desconocido)";

                var viewModel = new EmpleadoListViewModel
                {
                    IdEmpleado = empleado.IdEmpleado,
                    Nombre = empleado.Nombre,
                    Puesto = puestoNombre
                };

                return View(viewModel);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync($"{apiEmpleado}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["mensaje"] = "Empleado eliminado correctamente.";
                    return RedirectToAction("Index");
                }

                TempData["mensaje"] = "Error al eliminar el empleado.";
                return RedirectToAction("Index");
            }
        }


    }
}