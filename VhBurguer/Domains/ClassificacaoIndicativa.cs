using System;
using System.Collections.Generic;

namespace VHBurguer.Domains;

public partial class ClassificacaoIndicativa
{
    public int ClassificacaoIndicativaId { get; set; }

    public string Classificacao { get; set; } = null!;

    public virtual ICollection<Jogo> Jogo { get; set; } = new List<Jogo>();
}
