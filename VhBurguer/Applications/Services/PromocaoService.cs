using VHBurguer.Domains;
using VHBurguer.DTOs.PromocaoDto;
using VHBurguer.Interfaces;
using VHBurguer.Applications.Services;
using VHBurguer.Exceptions;
using VHBurguer.Applications.Regras;

namespace VHBurguer.Applications.Services
{
    public class PromocaoService 
    {

        private readonly IPromocaoRepository _repository; // adicionar uma camada de protecao a mais

        public PromocaoService(IPromocaoRepository repository)
        {
            _repository = repository;
        }

        public List<LerPromocaoDto> Listar()
        {
            List<Promocao> promocoes = _repository.Listar();

            List<LerPromocaoDto> promocoesDto = promocoes.Select(promocao => new LerPromocaoDto
            {
                PromocaoId = promocao.PromocaoID,
                Nome = promocao.Nome,
                DataExpiracao = promocao.DataExpiracao,
                StatusPromocao = promocao.StatusPromocao
            }).ToList();


        }

        public LerPromocaoDto ObterPorId(int id)
        {
            Promocao promocao = _repository.ObterPorId(id);
            if(promocao == null)
            {
                throw new DomainException("Promoção não encontrada");
            }

            LerPromocaoDto promocaoDto = new LerPromocaoDto
            {
                PromocaoId = promocao.PromocaoID,
                Nome = promocao.Nome,
                DataExpiracao = promocao.DataExpiracao,
                StatusPromocao = promocao.StatusPromocao
            };

            return promocaoDto;
        }

        private static void validarNome(string nome)
        {
            if(string.IsNullOrEmpty(nome))
            {
                throw new DomainException("Nome é obrigatório");
            }

        }

        public void Adicionar(CriarPromocaoDto criarPromocao)
        {

            validarNome(criarPromocao.Nome);
            ValidarDataExpiracaoPromocao.validarDataExpiracao(criarPromocao.DataExpiracao);

            if(_repository.NomeExiste(criarPromocao.Nome))
            {
                throw new DomainException("Promoção já existente.");
            }

            Promocao promocao = new Promocao
            {
                Nome = criarPromocao.Nome,
                DataExpiracao = criarPromocao.DataExpiracao,
                StatusPromocao = criarPromocao.StatusPromocao
            };

            _repository.Adicionar(promocao);

        }



    }
}
