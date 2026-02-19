using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VHBurguer.Applications.Services;
using VHBurguer.DTOs.AutenticacaoDto;
using VHBurguer.Domains;
using VHBurguer.Exceptions;

namespace VHBurguer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {

        private readonly AutenticacaoService _service;

        public AutenticacaoController(AutenticacaoService service)
        {
            _service = service;
        }


        [HttpPost("login")]
        public ActionResult <TokenDto> Login(LoginDto _loginDto)
        {
            try
            {
                var token = _service.Login(_loginDto);
                return StatusCode(200, token);
            }

            catch(DomainException ex)
            {
                return BadRequest(ex.Message);

            }
            return Login();
        }

    }
}
