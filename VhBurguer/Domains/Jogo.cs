using System;
using System.Collections.Generic;

namespace VHBurguer.Domains;

public partial class Jogo
{
    public int JogoId { get; set; }

    public string? Nome { get; set; }

    public string? Descrição { get; set; }

    public decimal? Preco { get; set; }

    public bool? StatusJogo { get; set; }

    public byte[] Imagem { get; set; } = null!;

    public int? UsuarioIdFK { get; set; }

    public int? ClassificaçãoIdFK { get; set; }

    public virtual ClassificacaoIndicativa? ClassificaçãoIdFKNavigation { get; set; }

    public virtual ICollection<JogoPromocao> JogoPromocao { get; set; } = new List<JogoPromocao>();

    public virtual ICollection<Log_Alteracao_Jogo> Log_Alteracao_Jogo { get; set; } = new List<Log_Alteracao_Jogo>();

    public virtual Usuario? UsuarioIdFKNavigation { get; set; }

    public virtual ICollection<Genero> GeneroIdFK { get; set; } = new List<Genero>();

    public virtual ICollection<Plataforma> PlataformaIdFK { get; set; } = new List<Plataforma>();
}
