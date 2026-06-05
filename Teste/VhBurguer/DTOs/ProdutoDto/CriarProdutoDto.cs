namespace VHBurguer.DTOs.ProdutoDto
{
    public class CriarProdutoDto
    {
        public string Nome { get; set; } = null!;

        public decimal Preco { get; set; }

        public string Descricao { get; set; } = null!;
        public IFormFile Imagem { get; set; } = null!; // a imagem vem em multiparte do iformfile / form-data, ideal para upload de arquivo

        public List<int> CategoriasIds { get; set; } = new();



    }
}
