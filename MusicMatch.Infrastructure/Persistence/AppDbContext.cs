using Microsoft.EntityFrameworkCore;
using MusicMatch.Domain.Entities;

namespace MusicMatch.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Artista> Artistas { get; set; }
    public DbSet<Contratante> Contratantes { get; set; }
    public DbSet<Agenda> Agendas { get; set; }
    public DbSet<Evento> Eventos { get; set; }
    public DbSet<Mensagem> Mensagens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Artista>().ToTable("Artistas");
        modelBuilder.Entity<Contratante>().ToTable("Contratantes");

        // Usuario base
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CpfCnpj).IsRequired().HasMaxLength(20);
            entity.Property(e => e.RazaoSocial).HasMaxLength(200);
            entity.Property(e => e.Celular1).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Celular2).HasMaxLength(20);
            entity.Property(e => e.CriadoEm).IsRequired();
        });

        /*
        // Artista — sem localização, sem equipamento, sem FormatosShow
        modelBuilder.Entity<Artista>(entity =>
        {
            entity.HasMany(e => e.Agendas)
                  .WithOne()
                  .HasForeignKey(a => a.ArtistaId);
        });
        */

        // Agenda
        modelBuilder.Entity<Agenda>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FormatoShow)
                  .IsRequired()
                  .HasConversion<string>();
            entity.Property(e => e.EquipamentoProprio).IsRequired();
            entity.Property(e => e.Data).IsRequired();
            entity.Property(e => e.HorarioInicial).IsRequired();
            entity.Property(e => e.HorarioFinal).IsRequired();
            entity.Property(e => e.BaseCacheHora).IsRequired().HasColumnType("decimal(10,2)");
            entity.Property(e => e.Disponivel).IsRequired();
            entity.Property(e => e.Cidade).IsRequired().HasMaxLength(300);
            entity.Property(e => e.Latitude).IsRequired();
            entity.Property(e => e.Longitude).IsRequired();
            entity.HasOne(a => a.Artista)
      .WithMany(ar => ar.Agendas)
      .HasForeignKey(a => a.ArtistaId);
        });

        // Evento
        modelBuilder.Entity<Evento>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Localizacao).IsRequired().HasMaxLength(300);
            entity.Property(e => e.Latitude).IsRequired();
            entity.Property(e => e.Longitude).IsRequired();
            entity.Property(e => e.RaioKm).IsRequired();
            entity.Property(e => e.FormatoShow).IsRequired().HasConversion<string>();
            entity.Property(e => e.Tipo).IsRequired().HasConversion<string>();
            entity.Property(e => e.DataInicio).IsRequired();
            entity.Property(e => e.DataFim).IsRequired();
            entity.Property(e => e.HorarioInicio).IsRequired();
            entity.Property(e => e.HorarioFim).IsRequired();
            entity.Property(e => e.BaseCacheHoraAte).IsRequired().HasColumnType("decimal(10,2)");
            entity.Property(e => e.CriadoEm).IsRequired();
            entity.HasOne(e => e.Contratante)
                  .WithMany(c => c.Eventos)
                  .HasForeignKey(e => e.ContratanteId);
        });

        // Mensagem
        modelBuilder.Entity<Mensagem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Msg).IsRequired();
            entity.Property(e => e.DataHora).IsRequired();
        });
    }
}