using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VHBurguer.Domains;

namespace VHBurguer.Contexts;

public partial class VH_BurguerContext : DbContext
{
    public VH_BurguerContext()
    {
    }

    public VH_BurguerContext(DbContextOptions<VH_BurguerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<Log_AlteracaoProduto> Log_AlteracaoProduto { get; set; }

    public virtual DbSet<Produto> Produto { get; set; }

    public virtual DbSet<Promocao> Promocao { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=VhBuguerKessia;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CategoriaId).HasName("PK__Categori__F353C1E5442F34E4");

            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Log_AlteracaoProduto>(entity =>
        {
            entity.HasKey(e => e.Log_AlteracaoId).HasName("PK__Log_Alte__E2FC4AC33D1FA7A9");

            entity.Property(e => e.DataAlteracao).HasPrecision(0);
            entity.Property(e => e.NomeAnterior)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PrecoAnterior).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Produto).WithMany(p => p.Log_AlteracaoProduto)
                .HasForeignKey(d => d.ProdutoId)
                .HasConstraintName("FK__Log_Alter__Produ__693CA210");
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.ProdutoId).HasName("PK__Produto__9C8800E393F01466");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("trg_AlteracaoProduto");
                    tb.HasTrigger("trg_ExcluirProduto");
                });

            entity.HasIndex(e => e.Nome, "UQ__Produto__7D8FE3B25F7AC6D5").IsUnique();

            entity.Property(e => e.Descricao).IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Preco).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.StatusProduto).HasDefaultValue(true);

            entity.HasOne(d => d.Usuario).WithMany(p => p.Produto)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ProdutoUsuario_Usuario");

            entity.HasMany(d => d.Categoria).WithMany(p => p.Produto)
                .UsingEntity<Dictionary<string, object>>(
                    "ProdutoCategoria",
                    r => r.HasOne<Categoria>().WithMany()
                        .HasForeignKey("CategoriaId")
                        .HasConstraintName("ProdutoCategoria_Categoria"),
                    l => l.HasOne<Produto>().WithMany()
                        .HasForeignKey("ProdutoId")
                        .HasConstraintName("ProdutoCategoria_Produto"),
                    j =>
                    {
                        j.HasKey("ProdutoId", "CategoriaId").HasName("FK_ProdutoCategoria_PK");
                    });

            entity.HasMany(d => d.Promocao).WithMany(p => p.Produto)
                .UsingEntity<Dictionary<string, object>>(
                    "ProdutoPromocao",
                    r => r.HasOne<Promocao>().WithMany()
                        .HasForeignKey("PromocaoId")
                        .HasConstraintName("ProdutoPromocao_Promocao_FK"),
                    l => l.HasOne<Produto>().WithMany()
                        .HasForeignKey("ProdutoId")
                        .HasConstraintName("ProdutoPromocao_Produto_FK"),
                    j =>
                    {
                        j.HasKey("ProdutoId", "PromocaoId").HasName("FK_ProdutoPromocao_PK");
                    });
        });

        modelBuilder.Entity<Promocao>(entity =>
        {
            entity.HasKey(e => e.PromocaoId).HasName("PK__Promocao__254B581DF25574D6");

            entity.Property(e => e.DataExpiracao).HasPrecision(0);
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StatusPromocao).HasDefaultValue(true);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuario__2B3DE7B88BEC4D2C");

            entity.ToTable(tb => tb.HasTrigger("trg_ExclusaoUsuario"));

            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Senha).HasMaxLength(32);
            entity.Property(e => e.StatusUsuario).HasDefaultValue(true);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
