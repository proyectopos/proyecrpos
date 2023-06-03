using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web;
using System.Web.Mvc;
using Punto_de_venta__POS_.Models;

namespace Punto_de_venta__POS_.Controllers
{
    [Authorize]
    public class CiudadController : Controller
    {
        private static HttpClient httpClient = new HttpClient();

        public CiudadController()
        {
            try
            {
                httpClient.BaseAddress = new Uri("http://localhost:51488"); // URL de tu API
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            catch (Exception e)
            {

            }
        }

        public async Task<ActionResult> Index(int id)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"/api/Estado/{id}");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Estado estado = JsonConvert.DeserializeObject<Estado>(json);

                HttpResponseMessage responseCiudad = await httpClient.GetAsync($"/api/Ciudad?id={id}&tmp={id}");
                if (responseCiudad.IsSuccessStatusCode)
                {
                    string jsonCiudad = await responseCiudad.Content.ReadAsStringAsync();
                    List<Ciudad> ciudades = JsonConvert.DeserializeObject<List<Ciudad>>(jsonCiudad);
                    return View(new CiudadVista() { estado = estado, ciudades = ciudades });
                }
            }

            // Manejar el caso de error de la API
            return HttpNotFound();
        }

        public ActionResult Create(int id)
        {
            ViewBag.EstadoId = id;
            // Manejar el caso de error de la API
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Ciudad ciudad)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/Ciudad", ciudad);
            if (response.IsSuccessStatusCode)
            {
                // La categoría se creó correctamente
                return RedirectToAction("Index", "Ciudad", new { id = ciudad.EstadoId });
            }

            // Manejar el caso de error de la API
            return View(ciudad);
        }

        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"/api/Ciudad/{id}");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Ciudad ciudad = JsonConvert.DeserializeObject<Ciudad>(json);
                return View(ciudad);
            }

            // Manejar el caso de error de la API
            return HttpNotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Ciudad ciudad)
        {
            HttpResponseMessage response = await httpClient.PutAsJsonAsync($"/api/Ciudad/{ciudad.CiudadId}", ciudad);
            if (response.IsSuccessStatusCode)
            {
                // La categoría se actualizó correctamente
                return RedirectToAction("Index", "Ciudad", new { id = ciudad.EstadoId });
            }

            // Manejar el caso de error de la API
            return View(ciudad);
        }

        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"/api/Ciudad/{id}");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Ciudad ciudad = JsonConvert.DeserializeObject<Ciudad>(json);
                return View(ciudad);
            }

            // Manejar el caso de error de la API
            return HttpNotFound();
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id, int EstadoId)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync($"/api/Ciudad/{id}");
            if (response.IsSuccessStatusCode)
            {
                // La categoría se eliminó correctamente
                return RedirectToAction("Index", "Ciudad", new { id = EstadoId });
            }

            // Manejar el caso de error de la API
            return HttpNotFound();
        }

        public ActionResult Back(int id)
        {
            return RedirectToAction("Index", "Ciudad", new { id = id });
        }
    }
}