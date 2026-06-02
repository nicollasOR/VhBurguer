using System;
using System.Collections.Generic;

namespace VHBurguer.Domains;

public partial class Promocao
{
    public int PromocaoId { get; set; }

    public string Nome { get; set; } = null!;

    public DateTime DataExpiração { get; set; }

    public bool StatusPromocao { get; set; }

    public virtual ICollection<JogoPromocao> JogoPromocao { get; set; } = new List<JogoPromocao>();
}
