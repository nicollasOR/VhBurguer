using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VHBurguer.Applications.Services;
using VHBurguer.DTOs.ProdutoDto;
using VHBurguer.DTOs.PromocaoDto;
using VHBurguer.Repositories;

namespace VHBurguer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromocaoController : ControllerBase
    {

        private readonly PromocaoService _service; 

        public PromocaoController(PromocaoService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult <List<LerPromocaoDto>> Listar()
        {

            List<LerPromocaoDto> promocoes = _service.Listar();
            return Ok(promocoes);
        }

        [HttpGet("{id}")]
        public ActionResult <LerPromocaoDto> ObterPorId(int id)
        {

            try
            {
                LerPromocaoDto promocao = _service.ObterPorId(id);
                return Ok(promocao);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }


        [HttpPost]
        [Authorize]
        public ActionResult Adicionar(CriarPromocaoDto criarPromocaoDto)
        {
            try
            {
                _service.Adicionar(criarPromocaoDto);
                return Ok(criarPromocaoDto);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        [Authorize]
        public ActionResult Atualizar(CriarPromocaoDto atualizarPromocaoDto, int id)
        {
            try
            {
                _service.Atualizar(id, atualizarPromocaoDto);
                return NoContent();
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Authorize]
        public ActionResult Remover(int id)
        {
            try
            {
                _service.Remover(id);
                return NoContent();
            }

            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        



    }
}
