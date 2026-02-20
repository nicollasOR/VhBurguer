using VHBurguer.Domains;
using VHBurguer.Repositories;
using VHBurguer.Interfaces;
using VHBurguer.Contexts;
using Microsoft.EntityFrameworkCore;
namespace VHBurguer.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly Vh_BurguerProfContext _context;

        public ProdutoRepository(Vh_BurguerProfContext context)
        {
            _context = context;
        }

        public List<Produto> Listar()
        {
            // a variavel percorre pela tabela produto
            // e procura pela categoria
            List<Produto> produtos = _context.Produto
                .Include(produtos => produtos.Categoria)
                .Include(produtos => produtos.Usuario)
                .ToList();

            return produtos;
        }

        public Produto ObterPorId(int id)
        {
            Produto? produto = _context.Produto
           .Include(produtoDatabase => produtoDatabase.Categoria)
           .Include(produtoDatabase => produtoDatabase.Usuario)
           //
           .FirstOrDefault(produtoDatabase => produtoDatabase.ProdutoID == id);


           return produto;
        }

        public bool NomeExiste(string nome, int? produtoIdAtual = null)
        {
            // AsQueryable() => Monta a consulta no banco para executar passo a passo
            // nao executa nada no banco ainda
            var produtoConsultado = _context.Produto.AsQueryable();

            if (produtoIdAtual.HasValue)
                produtoConsultado
                         = produtoConsultado.Where(produto => produto.ProdutoID != produtoIdAtual.Value);
            return produtoConsultado.Any(produto => produto.Nome == nome);
        }

        public byte[] ObterImagem(int id)
        {
            var produto = _context.Produto.Where(produto => produto.ProdutoID == id)
                                              .Select(produto => produto.Imagem)
                                              .FirstOrDefault();
            return produto;
        }

        public void Adicionar(Produto produto, List<int> categoriaId)
        {

            List<Categoria> categorias = _context.Categoria
                                         .Where(categorias => categoriaId.Contains(categorias.CategoriaID))
                                         .ToList();
            produto.Categoria = categorias;
            _context.Produto.Add(produto);
            _context.SaveChanges();

        }

        public void Atualizar(Produto produto, List<int> categoriaIds)
        {
            Produto produtoBanco = _context.Produto
                .Include(produto => produto.Categoria)
                .FirstOrDefault(produtoAuxiliar => produtoAuxiliar.ProdutoID == produto.ProdutoID);

            if (produtoBanco == null)
                return;

            produtoBanco.Nome = produto.Nome;
            produtoBanco.Preco = produto.Preco;
            produtoBanco.Descricao = produto.Descricao;

            if (produto.Imagem != null && produto.Imagem.Length > 0)
                produtoBanco.Imagem = produto.Imagem;

            if (produto.StatusProduto.HasValue)
                produtoBanco.StatusProduto = produto.StatusProduto;

            var categorias = _context.Categoria.
                                               Where(categoria => categoriaIds.Contains(categoria.CategoriaID))
                                              .ToList(); // busca todas as categorias do banco
                                                         // com o id igual as das categorias que vieram da requisicao do front
            produtoBanco.Categoria.Clear(); // remove as ligacoes da categoria com o banco e a categoria
            foreach (var categoria in categorias)
            {
                produtoBanco.Categoria.Add(categoria);
            }

            _context.SaveChanges();
        }

            public void Remover(int id)
            {
            Produto? produto = _context.Produto.FirstOrDefault(produto => produto.ProdutoID == id);

            if (produto == null)
                return;

            _context.Produto.Remove(produto);
            _context.SaveChanges();
            }


    }
}
