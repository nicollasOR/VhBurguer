using VHBurguer.Applications.Autenticacao;
using VHBurguer.Repositories;
using VHBurguer.Interfaces;
using System.Text.Unicode;
using VHBurguer.DTOs.AutenticacaoDto;
using VHBurguer.Domains;
using VHBurguer.Exceptions;

namespace VHBurguer.Applications.Services
{
    public class AutenticacaoService
    {

        private readonly IUsuarioRepository _repository;
        private readonly GeradorTokenJWT _tokenJWT;
        
        public AutenticacaoService(IUsuarioRepository repository, GeradorTokenJWT tokenJWT)
        {
            _repository = repository;
            _tokenJWT = tokenJWT;
        }

        //compara a hash

        private static bool verificarSenha(string senhaDigitada, byte[] SenhaHashBanco)
        {
            using var sha = 
                System.Security.Cryptography.SHA256.Create();

            var hashDigitado = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senhaDigitada));

            return hashDigitado.SequenceEqual(SenhaHashBanco);
        }

        public TokenDto Login(LoginDto _loginDto)
        {
            Usuario usuario = _repository.ObterPorEmail(_loginDto.Email);

            if (usuario == null)
                throw new DomainException("Email ou senha inválidos.");

            //comparar a senha digitada com a senha armazenada

            if (!verificarSenha(_loginDto.Senha, usuario.Senha))
                throw new DomainException("Email ou senha inválidos.");

            //gerando o token

            var token = _tokenJWT.gerarToken(usuario);

            TokenDto novoToken = new TokenDto { Token  = token };

            return novoToken                                                                                                             ;


        }


    }
}
