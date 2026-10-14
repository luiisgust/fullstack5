using System.Windows;
using System.Windows.Controls;
using System.ComponentModel.DataAnnotations;
using CatalogoFilmes.Models;
using CatalogoFilmes.Services;
namespace CatalogoFilmes.Views;
public partial class AvaliacoesWindow : Window
{
 private readonly CatalogoService service = new();
 private int _id;
 private DateTime _dataOriginal = DateTime.Now;
 public AvaliacoesWindow()
 {
  InitializeComponent();
  Loaded += async (_, _) => await OperacoesTela.ExecutarAsync(this, async () => { cbFilme.ItemsSource = await service.FilmesAsync(); await CarregarAsync(); });
 }
 private async Task CarregarAsync() => dgItens.ItemsSource = await service.AvaliacoesAsync(txtBusca.Text.Trim());
 private async void Buscar_Click(object sender, RoutedEventArgs e) => await OperacoesTela.ExecutarAsync(this, CarregarAsync);
 private async void Salvar_Click(object sender, RoutedEventArgs e)
 {
  await OperacoesTela.ExecutarAsync(this, async () =>
  {
   if (!decimal.TryParse(txtNota.Text, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.GetCultureInfo("pt-BR"), out var nota)) throw new ValidationException("Informe uma nota válida, por exemplo 8,5.");
   var item = new Avaliacao { Id = _id, FilmeId = (int)(cbFilme.SelectedValue ?? 0), Nota = nota, Comentario = txtComentario.Text.Trim(), DataAvaliacao = _dataOriginal };
   await service.SalvarAsync(item);
   Limpar();
   await CarregarAsync();
   MessageBox.Show(this, "Registro salvo.");
  });
 }
 private async void Excluir_Click(object sender, RoutedEventArgs e)
 {
  if (_id == 0) { MessageBox.Show(this, "Selecione um registro."); return; }
  if (MessageBox.Show(this, "Excluir este registro?", "Confirmar", MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;
  await OperacoesTela.ExecutarAsync(this, async () => { await service.ExcluirAsync<Avaliacao>(_id); Limpar(); await CarregarAsync(); });
 }
 private void Selecionar(object sender, SelectionChangedEventArgs e)
 {
  if (dgItens.SelectedItem is not Avaliacao item) return;
  _id = item.Id; cbFilme.SelectedValue=item.FilmeId; txtNota.Text=item.Nota.ToString(System.Globalization.CultureInfo.GetCultureInfo("pt-BR")); txtComentario.Text=item.Comentario; _dataOriginal=item.DataAvaliacao; txtModo.Text = "Editar registro #" + _id;
 }
 private void Limpar_Click(object sender, RoutedEventArgs e) => Limpar();
 private void Limpar() { _id=0; _dataOriginal=DateTime.Now; cbFilme.SelectedIndex = -1; txtNota.Clear(); txtComentario.Clear(); dgItens.SelectedItem=null; txtModo.Text="Novo registro"; }
}
