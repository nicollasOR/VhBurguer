using VHBurguer.Applications.ContentSafety;
using VHBurguer.Applications.Conversoes;
using VHBurguer.Applications.Regras;
using VHBurguer.Domains;
using VHBurguer.DTOs;
using VHBurguer.DTOs.ProdutoDto;
using VHBurguer.Exceptions;
using VHBurguer.Interfaces;

namespace VHBurguer.Applications.Services
{
    public class ProdutoService
    {

        private readonly IContentSafetyRepository _contentSafety;
        private readonly IProdutoRepository _repository;

        public ProdutoService(IProdutoRepository repository, IContentSafetyRepository contentSafety)
        {
            _repository = repository;
            _contentSafety = contentSafety;

        }


        private async Task ValidarConteudoProdutoAsync(string nome, string descricao)
        {
            string textoParaValidar = $@"
                Nome do produto: {nome}
                Descrição do produto: {descricao}";

            var resultado = await _contentSafety.ValidarConteudoProdutoAsync(textoParaValidar);

            if (!resultado.aprovado)
            {
                throw new DomainException(resultado.msg);
            }
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

        private static void ValidarCadastro(CriarProdutoDto produtoDto)
        { 
            //ValidarConteudoProdutoAsync(string nome, string descricao);
            if (string.IsNullOrWhiteSpace(produtoDto.Nome))
                throw new DomainException("Nome é obrigatório.");

            if (produtoDto.Preco < 0)
                throw new DomainException("Preço deve ser maior que zero.");
            if (string.IsNullOrWhiteSpace(produtoDto.Descricao))
                throw new DomainException("Descrição é obrigatória.");

            if (produtoDto.Imagem == null || produtoDto.Imagem.Length == 0)
                throw new DomainException("Imagem é obrigatória.");

            if (produtoDto.CategoriasIds == null || produtoDto.CategoriasIds.Count == 0)
                throw new DomainException("Produto deve ter ao menos uma categoria.");
        }

        public byte[] ObterImagem(int id)
        {
            byte[] imagem = _repository.ObterImagem(id);

            if (imagem == null || imagem.Length == 0)
                throw new DomainException("Imagem não encontrada");



            return imagem;
        }

        public async Task<LerProdutoDto> Adicionar(CriarProdutoDto produtoDto, int usuarioId)
        {
            ValidarCadastro(produtoDto);
            await ValidarConteudoProdutoAsync(produtoDto.Nome, produtoDto.Descricao);
            if (_repository.NomeExiste(produtoDto.Nome))
            {
                throw new DomainException("Produto já existente");
            }

            Produto produto = new Produto
            {
                Nome = produtoDto.Nome,
                Preco = produtoDto.Preco,
                Descricao = produtoDto.Descricao,
                Imagem = ImagemParaBytes.ConverterImagem(produtoDto.Imagem),
                StatusProduto = true,
                UsuarioId = usuarioId
            };

            _repository.Adicionar(produto, produtoDto.CategoriasIds);

            return ProdutoParaDTO.converterParaDTO(produto);
        }

        public LerProdutoDto Atualizar(int id, AtualizarProdutoDto produtoDto)
        {
            HorarioAlteracaoProduto.validarHorario();

            Produto produtoBanco = _repository.ObterPorId(id);

            if (produtoBanco == null)
            {
                throw new DomainException("Produto não encontrado.");
            }

            // produtoIdAtual: -> dois pontos serve para passar o valor do parametro
            if (_repository.NomeExiste(produtoDto.Nome, produtoIdAtual: id))
            {
                throw new DomainException("Já existe outro produto com esse nome.");
            }

            if (produtoDto.CategoriasIds == null || produtoDto.CategoriasIds.Count == 0)
            {
                throw new DomainException("Produto deve ter ao menos uma categoria.");
            }

            if (produtoDto.Preco < 0)
            {
                throw new DomainException("Preço deve ser maior que zero.");
            }

            produtoBanco.Nome = produtoDto.Nome;
            produtoBanco.Preco = produtoDto.Preco;
            produtoBanco.Descricao = produtoDto.Descricao;

            if (produtoDto.Imagem != null && produtoDto.Imagem.Length > 0)
            {
                produtoBanco.Imagem = ImagemParaBytes.ConverterImagem(produtoDto.Imagem);
            }

            if (produtoDto.StatusProduto.HasValue)
            {
                produtoBanco.StatusProduto = produtoDto.StatusProduto.Value;
            }

            _repository.Atualizar(produtoBanco, produtoDto.CategoriasIds);

            return ProdutoParaDTO.converterParaDTO(produtoBanco);

        }

        public void Remover(int id)
        {
            HorarioAlteracaoProduto.validarHorario();

            Produto produto = _repository.ObterPorId(id);

            if (produto == null)
            {
                throw new DomainException("Produto não encontrado.");
            }

            _repository.Remover(id);
        }

    }
}
