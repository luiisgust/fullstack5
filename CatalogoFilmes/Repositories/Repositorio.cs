using CatalogoFilmes.Data;
using CatalogoFilmes.Models;
using Microsoft.EntityFrameworkCore;
namespace CatalogoFilmes.Repositories;

// Contexto curto por operação: leitura sem rastreamento; escrita por entidade.
public class Repositorio<T> where T : Entidade
{
 public async Task<List<T>> ListarAsync(Func<IQueryable<T>, IQueryable<T>> consulta)
 {
  using var db = new AppDbContext();
  return await consulta(db.Set<T>().AsNoTracking()).ToListAsync();
 }
 public async Task SalvarAsync(T dados)
 {
  using var db = new AppDbContext();
  if (dados.Id == 0) db.Set<T>().Add(dados);
  else
  {
   var atual = await db.Set<T>().FindAsync(dados.Id)
       ?? throw new InvalidOperationException("Registro não encontrado. Atualize a lista.");
   // Copia os campos simples e IDs, sem inserir as entidades relacionadas.
   db.Entry(atual).CurrentValues.SetValues(dados);
  }
  await db.SaveChangesAsync();
 }
 public async Task ExcluirAsync(int id)
 {
  using var db = new AppDbContext();
  var item = await db.Set<T>().FindAsync(id)
      ?? throw new InvalidOperationException("Registro não encontrado. Atualize a lista.");
  db.Remove(item);
  await db.SaveChangesAsync();
 }
}
