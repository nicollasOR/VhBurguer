using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.DataClassification;
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

            try
            {
                LerUsuarioDto usuarioDto = _usuarioService.ObterPorId(id);
                return Ok(usuarioDto);
            }

            catch(DomainException ex)
            {
                return NotFound(ex.Message);
            }


        }

        [HttpGet("email/{email}")]

        public ActionResult<LerUsuarioDto> ObterPorEmaiL(string email)
        {
            try
            {
                LerUsuarioDto usuarioDto = _usuarioService.ObterPorEmail(email);
                return Ok(usuarioDto);
            }

            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public ActionResult<LerUsuarioDto> Adicionar(CriarUsuarioDto usuarioDto)
        {
            try
            {
                LerUsuarioDto usuarioCriado = _usuarioService.Adicionar(usuarioDto);

                return StatusCode(201, usuarioCriado);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }

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

        //public IActionResult conferirUsuario(int id)
        //{
           
        //}
        


    }
}
