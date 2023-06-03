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
    public class PaisController : Controller
    {
        private static HttpClient httpClient = new HttpClient();

        public PaisController()
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
            HttpResponseMessage response = await httpClient.GetAsync("/api/Pais");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                List<Pais> paises = JsonConvert.DeserializeObject<List<Pais>>(json);
                return View(paises);
            }

            // Manejar el caso de error de la API
            return View(new List<Pais>());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Pais pais)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/Pais", pais);
            if (response.IsSuccessStatusCode)
            {
                // La categoría se creó correctamente
                return RedirectToAction("Index");
            }

            // Manejar el caso de error de la API
            return View(pais);
        }

        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"/api/Pais/{id}");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Pais pais = JsonConvert.DeserializeObject<Pais>(json);
                return View(pais);
            }

            // Manejar el caso de error de la API
            return HttpNotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Pais pais)
        {
            HttpResponseMessage response = await httpClient.PutAsJsonAsync($"/api/Pais/{pais.PaisId}", pais);
            if (response.IsSuccessStatusCode)
            {
                // La categoría se actualizó correctamente
                return RedirectToAction("Index");
            }

            // Manejar el caso de error de la API
            return View(pais);
        }

        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"/api/Pais/{id}");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Pais pais = JsonConvert.DeserializeObject<Pais>(json);
                return View(pais);
            }

            // Manejar el caso de error de la API
            return HttpNotFound();
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync($"/api/Pais/{id}");
            if (response.IsSuccessStatusCode)
            {
                // La categoría se eliminó correctamente
                return RedirectToAction("Index");
            }

            // Manejar el caso de error de la API
            return HttpNotFound();
        }

        public ActionResult State(int id)
        {
            return RedirectToAction("Index", "Estado", new { id = id });

        }
    }
}