using System;
using System.Collections.Generic;

namespace VHBurguer.Domains;

public partial class Log_Alteracao_Jogo
{
    public int Log_Alteracao_Jogo_Id { get; set; }

    public DateTime DataAlteracao { get; set; }

    public string NomeAnterior { get; set; } = null!;

    public decimal PrecoAnterior { get; set; }

    public int? JogoId { get; set; }

    public virtual Jogo? Jogo { get; set; }
}
