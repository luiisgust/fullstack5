using System.IO;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using CatalogoFilmes.Models;
namespace CatalogoFilmes.Data;

public class ConfiguracaoBanco
{
 public string Servidor { get; set; } = "localhost";
 public uint Porta { get; set; } = 3306;
 public string Banco { get; set; } = "catalogo_filmes_exemplo";
 public string Usuario { get; set; } = "root";
 public string Senha { get; set; } = "";
}
public class AppDbContext : DbContext
{
 public DbSet<Filme> Filmes => Set<Filme>();
 public DbSet<Genero> Generos => Set<Genero>();
 public DbSet<Diretor> Diretores => Set<Diretor>();
 public DbSet<Avaliacao> Avaliacoes => Set<Avaliacao>();
 protected override void OnConfiguring(DbContextOptionsBuilder options)
 {
  var path = Path.Combine(AppContext.BaseDirectory, "database-settings.json");
  var config = JsonSerializer.Deserialize<ConfiguracaoBanco>(File.ReadAllText(path))
      ?? throw new InvalidOperationException("Configuração de banco inválida.");
  var connection = new MySqlConnectionStringBuilder {
   Server = config.Servidor, Port = config.Porta, Database = config.Banco,
   UserID = config.Usuario, Password = config.Senha, ConnectionTimeout = 5
  };
  options.UseMySql(connection.ConnectionString, new MySqlServerVersion(new Version(8, 0, 0)));
 }
 protected override void OnModelCreating(ModelBuilder model)
 {
  model.Entity<Genero>().ToTable("generos");
  model.Entity<Diretor>().ToTable("diretores");
  model.Entity<Filme>().ToTable("filmes");
  model.Entity<Avaliacao>().ToTable("avaliacoes");
  model.Entity<Avaliacao>().Property(a => a.Nota).HasPrecision(3, 1);
  model.Entity<Filme>().HasOne(f => f.Genero).WithMany().HasForeignKey(f => f.GeneroId).OnDelete(DeleteBehavior.Restrict);
  model.Entity<Filme>().HasOne(f => f.Diretor).WithMany().HasForeignKey(f => f.DiretorId).OnDelete(DeleteBehavior.Restrict);
  model.Entity<Avaliacao>().HasOne(a => a.Filme).WithMany().HasForeignKey(a => a.FilmeId).OnDelete(DeleteBehavior.Restrict);
 }
}
