using VHBurguer.DTOs.ProdutoDto;
using VHBurguer.Domains;
using VHBurguer.DTOs;
using VHBurguer.Exceptions;
using VHBurguer.Interfaces;
using VHBurguer.Applications.Conversoes;

namespace VHBurguer.Applications.Services
{
    public class ProdutoService
    {

        private readonly IProdutoRepository _repository;

        public ProdutoService (IProdutoRepository repository)
        {
            _repository = repository;
        }

        public List<LerProdutoDto> Listar()
        {
            List<Produto> produtos = _repository.Listar();
            // SELECT perocrre cada produto e transforma EM DTO -> LerProdutoDto
            List<LerProdutoDto> produtosDto = produtos.Select(ProdutoParaDTO.converterParaDTO).ToList();
            return produtosDto;
        }

        public LerProdutoDto ObterPorId(int id)
        {
            Produto produto = _repository.ObterPorId(id);
            if (produto == null)
                throw new DomainException("Produto não encontrado");

            return ProdutoParaDTO.converterParaDTO(produto);
        }

    }
}
