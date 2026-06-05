using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VHBurguer.Applications.Services;

namespace VHBurguer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogAlteracaoProdutoController : ControllerBase
    {
        private readonly Log_AlteracaoProdutoService _service; 
        public LogAlteracaoProdutoController( Log_AlteracaoProdutoService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Listar()
        {
            return Ok(_service.Listar());
        }

        [HttpGet("produto/{id}")]
        public ActionResult listarProduto(int id)
        {
            return Ok(_service.listarPorProduto(id));
        }

    }
}
