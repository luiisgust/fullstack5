using System.Windows;
using System.Windows.Controls;
using CatalogoFilmes.Services;
namespace CatalogoFilmes.Views;
public partial class DashboardWindow : Window
{
 public DashboardWindow() { InitializeComponent(); Loaded += async (_, _) => await AtualizarAsync(); }
 private async Task AtualizarAsync()
 {
  IsEnabled = false;
  try { var n = await new CatalogoService().ContagensAsync(); txtFilmes.Text=n[0].ToString(); txtGeneros.Text=n[1].ToString(); txtDiretores.Text=n[2].ToString(); txtAvaliacoes.Text=n[3].ToString(); txtStatus.Text="Conectado ao MySQL. Clique em um card para começar."; }
  catch (Exception) { txtFilmes.Text=txtGeneros.Text=txtDiretores.Text=txtAvaliacoes.Text="—"; txtStatus.Text="Configure o MySQL e execute database/schema.sql. Depois, atualize o painel."; }
  finally { IsEnabled = true; }
 }
 private async void Atualizar_Click(object sender, RoutedEventArgs e) => await AtualizarAsync();
 private async void Abrir_Click(object sender, RoutedEventArgs e)
 {
  Window janela = ((Button)sender).Tag.ToString() switch { "Filmes" => new FilmesWindow(), "Generos" => new GenerosWindow(), "Diretores" => new DiretoresWindow(), _ => new AvaliacoesWindow() };
  janela.Owner=this; janela.ShowDialog(); await AtualizarAsync();
 }
}
