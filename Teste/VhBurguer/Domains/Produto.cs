using System;
using System.Collections.Generic;

namespace VHBurguer.Domains;

public partial class Produto
{
    public int ProdutoId { get; set; }

    public string? Nome { get; set; }

    public string Descricao { get; set; } = null!;

    public byte[] Imagem { get; set; } = null!;

    public decimal Preco { get; set; }

    public bool? StatusProduto { get; set; }

    public int UsuarioId { get; set; }

    public virtual ICollection<Log_AlteracaoProduto> Log_AlteracaoProduto { get; set; } = new List<Log_AlteracaoProduto>();

    public virtual Usuario Usuario { get; set; } = null!;

    public virtual ICollection<Categoria> Categoria { get; set; } = new List<Categoria>();

    public virtual ICollection<Promocao> Promocao { get; set; } = new List<Promocao>();
}
