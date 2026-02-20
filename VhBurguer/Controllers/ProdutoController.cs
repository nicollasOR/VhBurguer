using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VHBurguer.Applications.Services;
using VHBurguer.DTOs.ProdutoDto;
using VHBurguer.Exceptions;
using VHBurguer.Applications.Services;
using Microsoft.AspNetCore.Authorization;
namespace VHBurguer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {

        private readonly ProdutoService _service;

        public ProdutoController(ProdutoService service)
        {
            _service = service;
        }

        private int ObterUsuarioIdLogado()
        {

            //busca no token/claims o valor armazenado como id do usuario
            string? idTexto = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //ClaimTypes.NameIdentifier geralmente guarda o ID do usuário no JWT

            if (string.IsNullOrEmpty(idTexto))
            {
                throw new DomainException("Usuário não encotnrado");
            }

            // string - > int
            // nosso usuarioId no sistema está como int
            return int.Parse(idTexto);
            //as claims que são os usuários dentro do token sempre serão armazenadas como texto
        }


        // autenticação do usuário

        [HttpGet]
        public ActionResult<List<LerProdutoDto>> Listar()
        {
            List<LerProdutoDto> produtos = _service.Listar();

            //return StatusCode(200, produtos);
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public ActionResult<LerProdutoDto> ObterPorId(int id)
        {
            LerProdutoDto produto = _service.ObterPorId(id);

            if (produto == null)
            {
                //return StatusCode(404);
                return NotFound();
            }

            return Ok(produto);
        }

        [HttpGet("{id}/imagem")]
        public ActionResult ObterImagem(int id)
        {

            try
            {
                var imagem = _service.ObterImagem(id);
                // retorna o arquivo para o navegador
                //"imagem/jpeg" informa o tipo da imagem (MIME type)
                //O navegador entende que deve renderizar como Imagem
                
                
                return File(imagem, "image/jpeg");
            }

            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }



        [HttpPost]
        //Indica que recebe dados no formato "multipart/form-data"
        // é necessário quando enviamos arquivos (ex. Img do produto)
        [Consumes("multipart/form-data")]
        [Authorize] // usado para que exija uma autenticacao para realizar tal metodo
        public ActionResult Adicionar([FromForm] CriarProdutoDto produtoDto) // [ FromForm ] diz que os dados vem do formulário da requisição
        {
            try
            {
                int usuarioId = ObterUsuarioIdLogado();
                _service.Adicionar(produtoDto, usuarioId); // cadastro fica associado ao usuario logado
                return NoContent();
            }

            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        [Authorize]

        public ActionResult Atualizar(int id, [FromForm] AtualizarProdutoDto produtoDto)
        {
            
            try
            {
                _service.Atualizar(id, produtoDto);
                return NoContent();
            }

            catch(DomainException ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpDelete("{id}")]
        [Authorize]

        public ActionResult Delete(int id)
        {

            try
            {
                _service.Remover(id);
                return StatusCode(204, id);
            }

            catch(DomainException ex)
            {
                return StatusCode(400, id);
            }

            

        }


    }
}
