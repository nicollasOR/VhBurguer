using VHBurguer.Domains;
using VHBurguer.DTOs.LogAlteracaoProdutoDto;
using VHBurguer.Interfaces;

namespace VHBurguer.Applications.Services
{
    public class Log_AlteracaoProdutoService
    {
        private readonly ILogAlteracaoProdutoRepository _repository;

        public Log_AlteracaoProdutoService(ILogAlteracaoProdutoRepository repository)
        {
            _repository = repository;
        }

        public List<LerLogProdutoDto> Listar( )
        {
            List<Log_AlteracaoProduto> log = _repository.Listar();

            List<LerLogProdutoDto> listaLogProduto = log.Select(logg => new LerLogProdutoDto
            {
                logId = logg.Log_AlteracaoId,
                produtoId = logg.ProdutoId,
                DataAlteracao = logg.DataAlteracao,
                nomeAnterior = logg.NomeAnterior,
                precoAnterior = logg.PrecoAnterior
            }).ToList();

            return listaLogProduto;
        }

        public List<LerLogProdutoDto> listarPorProduto(int produtoId)
        {
            List<Log_AlteracaoProduto> logs = _repository.listarPorProduto(produtoId);
            List<LerLogProdutoDto> listarLogProdutoDto = logs.Select(log_ => new LerLogProdutoDto
            {
                logId = log_.Log_AlteracaoId,
                produtoId = log_.ProdutoId,
                nomeAnterior = log_.NomeAnterior,
                DataAlteracao = log_.DataAlteracao,
                precoAnterior = log_.PrecoAnterior
            }).ToList();

            return listarLogProdutoDto;

        }


    }
}
