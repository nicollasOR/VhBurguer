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

    public virtual DbSet<ProdutoPromocao> ProdutoPromocao { get; set; }

    public virtual DbSet<Promocao> Promocao { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=NICOLLAS\\SQLEXPRESS;Database=VH_Burguer;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CategoriaId).HasName("PK__Categori__F353C1E53C83ADA2");

            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Log_AlteracaoProduto>(entity =>
        {
            entity.HasKey(e => e.Log_AlteracaoId).HasName("PK__Log_Alte__E2FC4AC36ED6D0B5");

            entity.Property(e => e.DataAlteracao).HasPrecision(0);
            entity.Property(e => e.NomeAnterior)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PrecoAnterior).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Produto).WithMany(p => p.Log_AlteracaoProduto)
                .HasForeignKey(d => d.ProdutoId)
                .HasConstraintName("FK__Log_Alter__Produ__5812160E");
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.ProdutoId).HasName("PK__Produto__9C8800E361DDA702");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("trg_AlteracaoProduto");
                    tb.HasTrigger("trg_ExcluirProduto");
                });

            entity.HasIndex(e => e.Nome, "UQ__Produto__7D8FE3B2A6B78BA1").IsUnique();

            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Preco).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.StatusProduto).HasDefaultValue(true);

            entity.HasOne(d => d.Usuario).WithMany(p => p.Produto)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__Produto__Usuario__5070F446");

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
        });

        modelBuilder.Entity<ProdutoPromocao>(entity =>
        {
            entity.HasKey(e => new { e.ProdutoId, e.PromocaoId }).HasName("FK_ProdutoPromocao_PK");

            entity.Property(e => e.PrecoAtual).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Produto).WithMany(p => p.ProdutoPromocao)
                .HasForeignKey(d => d.ProdutoId)
                .HasConstraintName("ProdutoPromocao_Produto_FK");

            entity.HasOne(d => d.Promocao).WithMany(p => p.ProdutoPromocao)
                .HasForeignKey(d => d.PromocaoId)
                .HasConstraintName("ProdutoPromocao_Promocao_FK");
        });

        modelBuilder.Entity<Promocao>(entity =>
        {
            entity.HasKey(e => e.PromocaoId).HasName("PK__Promocao__254B581DB43B1A29");

            entity.Property(e => e.DataExpiracao).HasPrecision(0);
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StatusPromocao).HasDefaultValue(true);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuario__2B3DE7B87CA1F39C");

            entity.ToTable(tb => tb.HasTrigger("trg_ExclusaoUsuario"));

            entity.HasIndex(e => e.Email, "UQ__Usuario__A9D10534F38A74BA").IsUnique();

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
