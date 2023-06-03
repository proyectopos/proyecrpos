using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Punto_de_venta__POS_.Models;

namespace Punto_de_venta__POS_.Controllers
{
    public class LoginController : Controller
    {
        private static HttpClient httpClient = new HttpClient();

        public LoginController()
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

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(Login login)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/Login", login);
            if (response.IsSuccessStatusCode)
            {
                // La categoría se creó correctamente
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Usuario usuario = JsonConvert.DeserializeObject<Usuario>(json);
                    if(usuario.CodigoUsuario != 0)
                    {
                        FormsAuthentication.SetAuthCookie(usuario.Usuario_, true);
                        return RedirectToAction("Index", "Home");
                    }
                }                
            }

            // Manejar el caso de error de la API
            ModelState.AddModelError("", "Credenciales inválidas");
            return View(login);
        }
    }
}