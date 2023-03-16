using Microsoft.AspNetCore.Mvc;
using MvcNetCoreZapatillas.Models;
using MvcNetCoreZapatillas.Repositories;

namespace MvcNetCoreZapatillas.Controllers
{
    public class ZapatillasController : Controller
    {
        private RepositoryZapatillas repo;
            public ZapatillasController(RepositoryZapatillas repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<Zapatilla> zapas = await this.repo.GetZapatillasAsync();
            return View(zapas);
        }

        public async Task<IActionResult> Details(int idzapa)
        {
            Zapatilla zapa = await this.repo.FindZapatillas(idzapa);
            return View(zapa);
        }

        public async Task<IActionResult> _DetailsImagenes(int idzapa,int? posicion)
        {
            if (posicion == null)
            {

                ModelZapatillaCantidad model = await this.repo.GetZapatillasProductoAsync(idzapa, 1);
                ViewData["REGISTROS"] = model.CantidadRegistros;
                ViewData["POSICION"] = 1;
                return PartialView(model.ImagenZapatilla);
            }
            else
            {

                ModelZapatillaCantidad model = await this.repo.GetZapatillasProductoAsync(idzapa, posicion.Value);
                ViewData["REGISTROS"] = model.CantidadRegistros;
                ViewData["POSICION"] = posicion;
                return PartialView(model.ImagenZapatilla);
            }
        }
    }
}
