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
    public class UsuarioController : Controller
    {
        private static HttpClient httpClient = new HttpClient();

        public UsuarioController()
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
            HttpResponseMessage response = await httpClient.GetAsync("/api/Usuario");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                List<Usuario> usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json);
                return View(usuarios);
            }

            // Manejar el caso de error de la API
            return View(new List<Usuario>());
        }

        public async Task<ActionResult> Create()
        {
            HttpResponseMessage response = await httpClient.GetAsync("/api/Rol");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                List<Rol> roles = JsonConvert.DeserializeObject<List<Rol>>(json);
                ViewBag.Roles = roles;
                return View();
            }

            // Manejar el caso de error de la API
            return View(new List<Rol>());
        }

        [HttpPost]
        public async Task<ActionResult> Create(Usuario usuario)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/Usuario", usuario);
            if (response.IsSuccessStatusCode)
            {
                // La categoría se creó correctamente
                return RedirectToAction("Index");
            }

            // Manejar el caso de error de la API
            return View(usuario);
        }

        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"/api/Usuario/{id}");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Usuario usuario = JsonConvert.DeserializeObject<Usuario>(json);

                HttpResponseMessage responseRol = await httpClient.GetAsync("/api/Rol");
                if (response.IsSuccessStatusCode)
                {
                    string jsonRol = await responseRol.Content.ReadAsStringAsync();
                    List<Rol> roles = JsonConvert.DeserializeObject<List<Rol>>(jsonRol);
                    ViewBag.Roles = roles;
                    return View(usuario);
                }
            }

            // Manejar el caso de error de la API
            return HttpNotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Usuario usuario)
        {
            HttpResponseMessage response = await httpClient.PutAsJsonAsync($"/api/Usuario/{usuario.CodigoUsuario}", usuario);
            if (response.IsSuccessStatusCode)
            {
                // La categoría se actualizó correctamente
                return RedirectToAction("Index");
            }

            // Manejar el caso de error de la API
            return View(usuario);
        }

        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"/api/Usuario/{id}");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Usuario usuario = JsonConvert.DeserializeObject<Usuario>(json);

                return View(usuario);
            }

            // Manejar el caso de error de la API
            return HttpNotFound();
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync($"/api/Usuario/{id}");
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