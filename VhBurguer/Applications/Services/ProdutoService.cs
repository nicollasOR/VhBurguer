using System.Threading.Tasks;
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

        private readonly IProdutoRepository _repository;
        private readonly IContentSafetyRepository _content;

        public ProdutoService(IProdutoRepository repository, IContentSafetyRepository content)
        {
            _repository = repository;
            _content = content;
        }

        public List<LerProdutoDto> Listar()
        {
            List<Produto> produtos = _repository.Listar();
            // SELECT percorre cada produto e transforma EM DTO -> LerProdutoDto
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

        private static async Task ValidarCadastro(CriarProdutoDto produtoDto)
        {
            if (string.IsNullOrWhiteSpace(produtoDto.Nome))
                throw new DomainException("Nome é obrigatório.");

            if (produtoDto.Preco < 0)
                throw new DomainException("Preço deve ser maior que zero.");
            //if (string.IsNullOrWhiteSpace(produtoDto.Descricao))
                //throw new DomainException("Descrição é obrigatória."); comentar para a geração de descrição funcionar

            if (produtoDto.Imagem == null || produtoDto.Imagem.Length == 0)
                throw new DomainException("Imagem é obrigatória.");

            if (produtoDto.CategoriasIds == null || produtoDto.CategoriasIds.Count == 0)
                throw new DomainException("Produto deve ter ao menos uma categoria.");

            
        }
        //public Task<LerProdutoDto> Adicionarv2(CriarProdutoDto produtoDto, int usuarioId)
        //{
            
        //}
        public byte[] ObterImagem(int id)
        {
            byte[] imagem = _repository.ObterImagem(id);

            if (imagem == null || imagem.Length == 0)
                throw new DomainException("Imagem não encontrada");



            return imagem;
        }



        // fazer este funcionar -> funcionando meu fio
        public async Task<LerProdutoDto> AdicionarAsync(CriarProdutoDto produtoDto, int usuarioId)
        {
            ValidarCadastro(produtoDto);

            if (_repository.NomeExiste(produtoDto.Nome))
            {
                throw new DomainException("Produto já existente");
            }

            string descricaoFinal = produtoDto.Descricao;

            // CORREÇÃO: Validando o texto de forma assíncrona antes de avançar
            if (!string.IsNullOrWhiteSpace(descricaoFinal))
            {
                var (aprovado, msg) = await _content.ValidarConteudoProdutoAsync(descricaoFinal);
                if (!aprovado)
                {
                    throw new DomainException($"Conteúdo inadequado detectado: {msg}");
                }
            }
            else // Caso a descrição seja vazia, a IA gera uma baseada na imagem
            {
                try
                {
                    // trocando o .GetAwaiter().GetResult() pelo await da forma certa 
                    // A IA que recomendou 👌
                    descricaoFinal = await _content.gerarDescricao(produtoDto.Imagem);
                }
                catch (DomainException ex)
                {
                    
                    descricaoFinal = "Descrição não informada.";
                    throw new DomainException("Descrição não informada");
                }
            }

            Produto produto = new Produto
            {
                Nome = produtoDto.Nome,
                Preco = produtoDto.Preco,
                Descricao = descricaoFinal,
                Imagem = ImagemParaBytes.ConverterImagem(produtoDto.Imagem),
                StatusProduto = true,
                UsuarioId = usuarioId
            };

            _repository.Adicionar(produto, produtoDto.CategoriasIds);

            return ProdutoParaDTO.converterParaDTO(produto);
        }




        /*
         Método sem async
          public LerProdutoDto Adicionar(CriarProdutoDto produtoDto, int usuarioId)
        {
            //await ValidarCadastro(produtoDto);

            if (_repository.NomeExiste(produtoDto.Nome))
            {
                throw new DomainException("Produto já existente");
            }

            string descricaoFinal = produtoDto.Descricao;
            _content.ValidarConteudoProdutoAsync(descricaoFinal);

            // verificação da descrição para que caso seja vazio
            if (string.IsNullOrWhiteSpace(descricaoFinal))
            {
                try
                {
                    // Executa o método do Gemini de forma síncrona sem precisar do async/await no Adicionar (demorei muito pra fazer)
                    descricaoFinal = _content.gerarDescricao(produtoDto.Imagem)
                                             .GetAwaiter() // esse devolve um objeto Task<string> que esta no gerarDesc acima
                                             .GetResult(); // aqui funciona o await e async, ele para o código e executa tudo dentro do método
                                                           // se der certo, retorna true e a descrição gerada por IA
                                                           // se não, vai pro catch
                }
                catch (DomainException ex)
                {
                    // Se o Gemini falhar por algum motivo (espero que nunca)
                    // o sistema não quebra e assume um texto padrão para continuar o cadastro
                    descricaoFinal = "Descrição não informada.";
                    throw new DomainException("Descrição deu erro pae");
                }
            }

            Produto produto = new Produto
            {
                Nome = produtoDto.Nome,
                Preco = produtoDto.Preco,
                Descricao = descricaoFinal, // Salva o texto enviado, o gerado pela IA ou o fallback
                Imagem = ImagemParaBytes.ConverterImagem(produtoDto.Imagem),
                StatusProduto = true,
                UsuarioId = usuarioId
            };

            _repository.Adicionar(produto, produtoDto.CategoriasIds);

            return ProdutoParaDTO.converterParaDTO(produto);
        }


        */


        public async Task<LerProdutoDto> Adicionar(CriarProdutoDto produtoDto, int usuarioId)
        {
            ValidarCadastro(produtoDto);

            //await ValidarConteudoProdutoAsync(produtoDto.Nome, produtoDto.Descricao);

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

            if (produtoDto.CategoriaIds == null || produtoDto.CategoriaIds.Count == 0)
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

            _repository.Atualizar(produtoBanco, produtoDto.CategoriaIds);

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
