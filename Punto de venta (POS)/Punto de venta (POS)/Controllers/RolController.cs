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
    public class RolController : Controller
    {
        private static HttpClient httpClient = new HttpClient();

        public RolController()
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

        public async Task<ActionResult> Index()
        {
            HttpResponseMessage response = await httpClient.GetAsync("/api/Rol");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                List<Rol> categorias = JsonConvert.DeserializeObject<List<Rol>>(json);
                return View(categorias);
            }

            // Manejar el caso de error de la API
            return View(new List<Rol>());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Rol rol)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/Rol", rol);
            if (response.IsSuccessStatusCode)
            {
                // La categoría se creó correctamente
                return RedirectToAction("Index");
            }

            // Manejar el caso de error de la API
            return View(rol);
        }

        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"/api/Rol/{id}");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Rol rol = JsonConvert.DeserializeObject<Rol>(json);

                return View(rol);
            }

            // Manejar el caso de error de la API
            return HttpNotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Rol rol)
        {
            HttpResponseMessage response = await httpClient.PutAsJsonAsync($"/api/Rol/{rol.CodigoRol}", rol);
            if (response.IsSuccessStatusCode)
            {
                // La categoría se actualizó correctamente
                return RedirectToAction("Index");
            }

            // Manejar el caso de error de la API
            return View(rol);
        }

        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"/api/Rol/{id}");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Rol rol = JsonConvert.DeserializeObject<Rol>(json);

                return View(rol);
            }

            // Manejar el caso de error de la API
            return HttpNotFound();
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync($"/api/Rol/{id}");
            if (response.IsSuccessStatusCode)
            {
                // La categoría se eliminó correctamente
                return RedirectToAction("Index");
            }

            // Manejar el caso de error de la API
            return HttpNotFound();
        }
    }
}