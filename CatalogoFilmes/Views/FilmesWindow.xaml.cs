using System.Windows;
using System.Windows.Controls;
using System.ComponentModel.DataAnnotations;
using CatalogoFilmes.Models;
using CatalogoFilmes.Services;
namespace CatalogoFilmes.Views;
public partial class FilmesWindow : Window
{
 private readonly CatalogoService service = new();
 private int _id;
 private DateTime _dataOriginal = DateTime.Now;
 public FilmesWindow()
 {
  InitializeComponent();
  Loaded += async (_, _) => await OperacoesTela.ExecutarAsync(this, async () => { cbGenero.ItemsSource = await service.GenerosAsync(); cbDiretor.ItemsSource = await service.DiretoresAsync(); await CarregarAsync(); });
 }
 private async Task CarregarAsync() => dgItens.ItemsSource = await service.FilmesAsync(txtBusca.Text.Trim());
 private async void Buscar_Click(object sender, RoutedEventArgs e) => await OperacoesTela.ExecutarAsync(this, CarregarAsync);
 private async void Salvar_Click(object sender, RoutedEventArgs e)
 {
  await OperacoesTela.ExecutarAsync(this, async () =>
  {
   if (!int.TryParse(txtAno.Text, out var ano) || !int.TryParse(txtDuracao.Text, out var duracao)) throw new ValidationException("Ano e duração devem ser números inteiros.");
   var item = new Filme { Id = _id, Titulo = txtTitulo.Text.Trim(), AnoLancamento = ano, DuracaoMinutos = duracao, GeneroId = (int)(cbGenero.SelectedValue ?? 0), DiretorId = (int)(cbDiretor.SelectedValue ?? 0) };
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
  await OperacoesTela.ExecutarAsync(this, async () => { await service.ExcluirAsync<Filme>(_id); Limpar(); await CarregarAsync(); });
 }
 private void Selecionar(object sender, SelectionChangedEventArgs e)
 {
  if (dgItens.SelectedItem is not Filme item) return;
  _id = item.Id; txtTitulo.Text=item.Titulo; txtAno.Text=item.AnoLancamento.ToString(); txtDuracao.Text=item.DuracaoMinutos.ToString(); cbGenero.SelectedValue=item.GeneroId; cbDiretor.SelectedValue=item.DiretorId; txtModo.Text = "Editar registro #" + _id;
 }
 private void Limpar_Click(object sender, RoutedEventArgs e) => Limpar();
 private void Limpar() { _id=0; _dataOriginal=DateTime.Now; txtTitulo.Clear(); txtAno.Clear(); txtDuracao.Clear(); cbGenero.SelectedIndex = -1; cbDiretor.SelectedIndex = -1; dgItens.SelectedItem=null; txtModo.Text="Novo registro"; }
}
