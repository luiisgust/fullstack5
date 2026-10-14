using System.ComponentModel.DataAnnotations;
using CatalogoFilmes.Data;
using CatalogoFilmes.Models;
using CatalogoFilmes.Repositories;
using Microsoft.EntityFrameworkCore;
namespace CatalogoFilmes.Services;

public class CatalogoService
{
 public static void Validar(Entidade item)
 {
  Validator.ValidateObject(item, new ValidationContext(item), true);
  switch (item)
  {
   case Genero g when string.IsNullOrWhiteSpace(g.Nome): throw new ValidationException("Informe o gênero.");
   case Diretor d when string.IsNullOrWhiteSpace(d.Nome): throw new ValidationException("Informe o diretor.");
   case Filme f:
    if (string.IsNullOrWhiteSpace(f.Titulo)) throw new ValidationException("Informe o título.");
    if (f.AnoLancamento < 1888 || f.AnoLancamento > DateTime.Today.Year + 5) throw new ValidationException("Informe um ano entre 1888 e o ano atual mais 5.");
    if (f.DuracaoMinutos <= 0 || f.DuracaoMinutos > 10000) throw new ValidationException("A duração deve ser de 1 a 10000 minutos.");
    if (f.GeneroId <= 0 || f.DiretorId <= 0) throw new ValidationException("Selecione gênero e diretor.");
    break;
   case Avaliacao a:
    if (a.FilmeId <= 0) throw new ValidationException("Selecione um filme.");
    if (a.Nota < 0 || a.Nota > 10 || decimal.Round(a.Nota, 1) != a.Nota) throw new ValidationException("A nota deve estar entre 0 e 10, com até uma casa decimal.");
    break;
  }
 }
 public async Task SalvarAsync<T>(T item) where T : Entidade
 {
  Validar(item);
  await new Repositorio<T>().SalvarAsync(item);
 }
 public Task ExcluirAsync<T>(int id) where T : Entidade => new Repositorio<T>().ExcluirAsync(id);
 // IndexOf é traduzido pelo provedor para LOCATE com o termo como parâmetro.
 public Task<List<Genero>> GenerosAsync(string termo = "") => new Repositorio<Genero>().ListarAsync(q => q.Where(g => g.Nome.IndexOf(termo) >= 0).OrderBy(g => g.Nome));
 public Task<List<Diretor>> DiretoresAsync(string termo = "") => new Repositorio<Diretor>().ListarAsync(q => q.Where(d => d.Nome.IndexOf(termo) >= 0).OrderBy(d => d.Nome));
 public Task<List<Filme>> FilmesAsync(string termo = "", int? generoId = null) => new Repositorio<Filme>().ListarAsync(q => q.Include(f => f.Genero).Include(f => f.Diretor).Where(f => f.Titulo.IndexOf(termo) >= 0 && (!generoId.HasValue || f.GeneroId == generoId)).OrderBy(f => f.Titulo));
 public Task<List<Avaliacao>> AvaliacoesAsync(string termo = "") => new Repositorio<Avaliacao>().ListarAsync(q => q.Include(a => a.Filme).Where(a => a.Filme.Titulo.IndexOf(termo) >= 0 || a.Comentario.IndexOf(termo) >= 0).OrderByDescending(a => a.DataAvaliacao));
 public async Task<int[]> ContagensAsync()
 {
  using var db = new AppDbContext();
  return new[] { await db.Filmes.CountAsync(), await db.Generos.CountAsync(), await db.Diretores.CountAsync(), await db.Avaliacoes.CountAsync() };
 }
}
