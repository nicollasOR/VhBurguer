using VHBurguer.DTOs.ProdutoDto;
using VHBurguer.Domains;

namespace VHBurguer.Applications.Conversoes
{
    public class ProdutoParaDTO
    {

        public static LerProdutoDto converterParaDTO(Produto produto)
    {
            return new LerProdutoDto
            {
                ProdutoID = produto.ProdutoID,
                Nome = produto.Nome,
                Preco = produto.Preco,
                Descricao = produto.Descricao,
                StatusProduto = produto.StatusProduto,
                CategoriaIds = produto.Categoria.Select(categoria => categoria.CategoriaID).ToList(),

                Categorias = produto.Categoria.Select(categorias => categorias.Nome).ToList(),
                UsuarioID = produto.UsuarioID,
                UsuarioNome = produto.Usuario.Nome,
                UsuarioEmail = produto.Usuario.Email
            };
    }

    }
}
