using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VHBurguer.Domains;
using VHBurguer.Exceptions;

namespace VHBurguer.Applications.Autenticacao
{
    public class GeradorTokenJWT
    {

        private readonly IConfiguration _config;

        // Ele percorre pelo appsettings.json e puxa as informações de lá

        public GeradorTokenJWT(IConfiguration config)
        {
            _config = config;
        }


        public string gerarToken(Usuario usuario)
        {
            // KEY -> chave secreta
            var chave = _config["JWT:Key"];
            // usada para assinar o token JWT

            // issuer -> quem gerou o token (nome da API / sistema que gerou)
            var issuer = _config["JWT:Issuer"];
            // A API valida se o token veio do emissor correto

            // AUDIENCE -> para quem o token foi criado
            var audience = _config["JWT:Audience"];
            // define qual sistema pode usar o token


            // TEMPO DE EXPIRAÇÂO -> Define quantos minutos o token será valido
            var ExpiraEmMinutos = int.Parse(_config["JWT:ExpiraEmMinutos"]);
            //depois disso , o usuário precisará logar novamente

            // Converter para bytes agora ( necessário para assinatura)
            var keyBytes = Encoding.UTF8.GetBytes(chave);

            //Segurança exige uma senha com mais de 32 caracteres pelo menos.
            if(keyBytes.Length < 32)
                throw new DomainException("JWT: Key precisa ter 32 caracteres");

            // cria a chave de segurança usada para assinar o token
            var securityKey = new SymmetricSecurityKey(keyBytes);

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Claims agora fio -> que são informações do usuario que vão dentro do token
            // elas servem para identificar quem está logado

            var claims = new List<Claim>
            {
                // passar o id do usuario ne
            new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioID.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nome.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email.ToString())
            };

            // criar nosso token jwt agora

            var token = new JwtSecurityToken(
            issuer: issuer,                                     // quem gerou o token
            audience: audience,                                 // quem pode usar o token
            claims: claims,                                     // dados do usuario 
            expires: DateTime.Now.AddMinutes(ExpiraEmMinutos),  // validade do token
            signingCredentials: credentials                     // assinatura de segurança
            );

            // Converte o token para a string e ela é enviada para o cliente

            return new JwtSecurityTokenHandler().WriteToken(token);

        }



    }
}
