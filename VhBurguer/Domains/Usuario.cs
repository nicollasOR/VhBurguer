using System;
using System.Collections.Generic;

namespace VHBurguer.Domains;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public byte[] Senha { get; set; } = null!;

    public bool? StatusUsuario { get; set; }

    public virtual ICollection<Jogo> Jogo { get; set; } = new List<Jogo>();
}
