using System.ComponentModel.DataAnnotations;
using System.Windows;
using Microsoft.EntityFrameworkCore;
namespace CatalogoFilmes.Views;
public static class OperacoesTela
{
 public static async Task ExecutarAsync(Window tela, Func<Task> acao)
 {
  if (!tela.IsEnabled) return;
  tela.IsEnabled = false;
  try { await acao(); }
  catch (ValidationException ex) { MessageBox.Show(tela, ex.Message, "Confira os dados"); }
  catch (InvalidOperationException ex) { MessageBox.Show(tela, ex.Message, "Operação não concluída"); }
  catch (DbUpdateException) { MessageBox.Show(tela, "Não foi possível gravar. Confira os dados e os vínculos. Para excluir, remova primeiro os registros que usam este item.", "Banco de dados"); }
  catch (Exception) { MessageBox.Show(tela, "Não foi possível acessar os dados. Confira o MySQL, o banco e database-settings.json.", "Conexão"); }
  finally { tela.IsEnabled = true; }
 }
}
