using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VHBurguer.Applications.Services;
using VHBurguer.DTOs.UsuarioDto;
using VHBurguer.Exceptions;

namespace VHBurguer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {


        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public ActionResult<List<LerUsuarioDto>> Listar()
        {
            List<LerUsuarioDto> usuarios = _usuarioService.Listar();
            if (usuarios == null)
                return NotFound(usuarios);

            else
                return Ok(usuarios);

        }

        [HttpGet("{id}")]
        public ActionResult<LerUsuarioDto> ObterPorId(int id)
        {

            LerUsuarioDto usuarioDto = _usuarioService.ObterPorId(id);
            if (usuarioDto == null)
                return NotFound(usuarioDto);
            else
                return Ok(usuarioDto);



        }

        [HttpGet("email/{email}")]

        public ActionResult<LerUsuarioDto> ObterPorEmaiL(string email)
        {
            LerUsuarioDto usuarioDto = _usuarioService.ObterPorEmail(email);
            if (usuarioDto != null)
                return Ok(usuarioDto);
            else
                return NotFound(usuarioDto);
        }

        [HttpPost]
        public ActionResult<LerUsuarioDto> Adicionar(CriarUsuarioDto usuarioDto)
        {

            try
            {
                LerUsuarioDto usuarioCriado = _usuarioService.Adicionar(usuarioDto);
                return StatusCode(201, usuarioCriado);
            }

            catch (DomainException exception)
            {
                return BadRequest(exception.Message);
            }


            /*
            if(usuarioDto != null)
            return Ok(usuarioDto);

            else
            return BadRequest(usuarioDto);
            
             */
        }

        [HttpPut("{id}")]
        public ActionResult<LerUsuarioDto> Atualizar(int id, CriarUsuarioDto usuarioDto)
        {
            try
            {
                LerUsuarioDto usuarioAtualizado = _usuarioService.Atualizar(id, usuarioDto);
                return Ok(usuarioAtualizado);
            }

            catch (DomainException excecao)
            {
                return BadRequest(excecao.Message);
            }
        }

        [HttpDelete("{id}")]

        //remover os dados
        //a trigger vai ativar e inativar o usuario
        // big soft delete
        public IActionResult Remover(int id)
        {
            try
            {
                _usuarioService.Remover(id);
                return Ok(id);
            }

            catch(Exception ex)
            {
                return BadRequest(id);
            }
            }
        


    }
}
