using System;
using System.Collections.Generic;

namespace VHBurguer.Domains;

public partial class Genero
{
    public int GeneroId { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<Jogo> JogoIdFK { get; set; } = new List<Jogo>();
}
