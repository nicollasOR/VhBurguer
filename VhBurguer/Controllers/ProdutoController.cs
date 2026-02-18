using Microsoft.AspNetCore.Mvc;

namespace VHBurguer.Controllers
{
    public class ProdutoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
