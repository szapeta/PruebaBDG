using FrontEnd.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    public class PuestoController : Controller
    {
        private readonly string apiUrl = "https://localhost:44397/api/puestos";

        public async Task<ActionResult> Index()
        {
            List<Puesto> puestos = new List<Puesto>();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    puestos = JsonConvert.DeserializeObject<List<Puesto>>(data);
                }
            }

            return View(puestos);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Puesto puesto)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(puesto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["mensaje"] = "Puesto creado correctamente.";
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Error al crear el puesto.");
                return View(puesto);
            }
        }
    }
}