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
using System.IO;

namespace Punto_de_venta__POS_.Controllers
{
    [Authorize]
    public class ArticuloController : Controller
    {
        private static HttpClient httpClient = new HttpClient();

        public ArticuloController()
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
             HttpResponseMessage response = await httpClient.GetAsync("/api/Articulo");
             if (response.IsSuccessStatusCode)
             {
                 string json = await response.Content.ReadAsStringAsync();
                 List<Articulo> articulos = JsonConvert.DeserializeObject<List<Articulo>>(json);
                 return View(articulos);
             }

             // Manejar el caso de error de la API
             return View(new List<Articulo>());
        }

        public async Task<ActionResult> Create()
        {
            HttpResponseMessage response = await httpClient.GetAsync("/api/Categoria");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                List<Categoria> categorias = JsonConvert.DeserializeObject<List<Categoria>>(json);
                ViewBag.Categoria = categorias;
                return View();
            }

            // Manejar el caso de error de la API
            return View(new List<Categoria>());
        }

        [HttpPost]
        public async Task<ActionResult> Create(Articulo articulo)
        {
            if (ModelState.IsValid)
            {
                if (articulo.ImagenForm != null && articulo.ImagenForm.ContentLength > 0){
                     byte[] imagenData;
                     using (var binaryReader = new BinaryReader(articulo.ImagenForm.InputStream)){
                        imagenData = binaryReader.ReadBytes(articulo.ImagenForm.ContentLength);
                     }
                    articulo.Imagen = Convert.ToBase64String(imagenData);
                    articulo.ImagenForm = null;
                    HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/Articulo", articulo);
                    if (response.IsSuccessStatusCode)
                    {
                        // La categoría se creó correctamente
                        return RedirectToAction("Index");
                    }
                }
            }    

            // Manejar el caso de error de la API
            return View(articulo);
        }

       public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"/api/Articulo/{id}");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Articulo articulo = JsonConvert.DeserializeObject<Articulo>(json);
                HttpResponseMessage responseCategoria = await httpClient.GetAsync("/api/Categoria");
                if (responseCategoria.IsSuccessStatusCode)
                {
                    string jsonCategorias = await responseCategoria.Content.ReadAsStringAsync();
                    List<Categoria> categorias = JsonConvert.DeserializeObject<List<Categoria>>(jsonCategorias);
                    ViewBag.Categoria = categorias;
                    return View(articulo);
                }
            }

            // Manejar el caso de error de la API
            return HttpNotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Articulo articulo,string imageTmp)
        {
            if (ModelState.IsValid)
            {
                byte[] imagenData;
                if (articulo.ImagenForm != null && articulo.ImagenForm.ContentLength > 0)
                {
                    
                    using (var binaryReader = new BinaryReader(articulo.ImagenForm.InputStream))
                    {
                        imagenData = binaryReader.ReadBytes(articulo.ImagenForm.ContentLength);
                    }
                    articulo.Imagen = Convert.ToBase64String(imagenData);
                    articulo.ImagenForm = null;
                }
                else
                {
                    articulo.Imagen = imageTmp;
                    articulo.ImagenForm = null;
                }
                
                HttpResponseMessage response = await httpClient.PutAsJsonAsync($"/api/Articulo/{articulo.ArticuloId}", articulo);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(articulo);
        }

        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"/api/Articulo/{id}");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Articulo articulo = JsonConvert.DeserializeObject<Articulo>(json);
                return View(articulo);
            }

            // Manejar el caso de error de la API
            return HttpNotFound();
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync($"/api/Articulo/{id}");
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