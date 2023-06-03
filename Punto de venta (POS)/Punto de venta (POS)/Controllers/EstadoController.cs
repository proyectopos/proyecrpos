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
    public class EstadoController : Controller
    {
        private static HttpClient httpClient = new HttpClient();

        public EstadoController()
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
            HttpResponseMessage response = await httpClient.GetAsync($"/api/Pais/{id}");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Pais pais = JsonConvert.DeserializeObject<Pais>(json);

                HttpResponseMessage responseEstados = await httpClient.GetAsync($"/api/Estado?id={id}&tmp={id}");
                if (responseEstados.IsSuccessStatusCode)
                {
                    string jsonEstados = await responseEstados.Content.ReadAsStringAsync();
                    List<Estado> estados = JsonConvert.DeserializeObject<List<Estado>>(jsonEstados);
                    return View(new EstadoVista() { pais = pais, estados = estados });
                }
            }

            // Manejar el caso de error de la API
            return HttpNotFound();
        }

        public ActionResult Create(int id)
        {
            ViewBag.PaisId = id;
            // Manejar el caso de error de la API
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Estado estado)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/Estado", estado);
            if (response.IsSuccessStatusCode)
            {
                // La categoría se creó correctamente
                return RedirectToAction("Index", "Estado", new {id = estado.PaisId });
            }

            // Manejar el caso de error de la API
            return View(estado);
        }

        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"/api/Estado/{id}");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Estado estado = JsonConvert.DeserializeObject<Estado>(json);
                return View(estado);
            }

            // Manejar el caso de error de la API
            return HttpNotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Estado estado)
        {
            HttpResponseMessage response = await httpClient.PutAsJsonAsync($"/api/Estado/{estado.EstadoId}", estado);
            if (response.IsSuccessStatusCode)
            {
                // La categoría se actualizó correctamente
                return RedirectToAction("Index", "Estado", new { id = estado.PaisId });
            }

            // Manejar el caso de error de la API
            return View(estado);
        }

        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"/api/Estado/{id}");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Estado estado = JsonConvert.DeserializeObject<Estado>(json);
                return View(estado);
            }

            // Manejar el caso de error de la API
            return HttpNotFound();
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id,int PaisId)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync($"/api/Estado/{id}");
            if (response.IsSuccessStatusCode)
            {
                // La categoría se eliminó correctamente
                return RedirectToAction("Index", "Estado", new { id = PaisId });
            }

            // Manejar el caso de error de la API
            return HttpNotFound();
        }

        public ActionResult Back(int id)
        {
            return RedirectToAction("Index", "Estado", new { id = id });
        }
        public ActionResult City(int id)
        {
            return RedirectToAction("Index", "Ciudad", new { id = id });

        }
    }
}