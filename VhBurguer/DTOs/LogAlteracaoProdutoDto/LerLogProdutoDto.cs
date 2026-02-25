namespace VHBurguer.DTOs.LogAlteracaoProdutoDto
{
    public class LerLogProdutoDto
    {

        public int? logId { get; set; }
        public int? produtoId { get; set; }

        public string nomeAnterior { get; set; } = null!;
        public decimal? precoAnterior { get; set; }
        public DateTime DataAlteracao { get; set; }

    }
}
