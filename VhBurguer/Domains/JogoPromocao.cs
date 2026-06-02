using System;
using System.Collections.Generic;

namespace VHBurguer.Domains;

public partial class JogoPromocao
{
    public int JogoIdFK { get; set; }

    public int PromocaoIdFK { get; set; }

    public decimal PrecoAtual { get; set; }

    public virtual Jogo JogoIdFKNavigation { get; set; } = null!;

    public virtual Promocao PromocaoIdFKNavigation { get; set; } = null!;
}
