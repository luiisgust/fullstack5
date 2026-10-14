using System.Windows;
using System.Windows.Controls;
using System.ComponentModel.DataAnnotations;
using CatalogoFilmes.Models;
using CatalogoFilmes.Services;
namespace CatalogoFilmes.Views;
public partial class GenerosWindow : Window
{
 private readonly CatalogoService service = new();
 private int _id;
 private DateTime _dataOriginal = DateTime.Now;
 public GenerosWindow()
 {
  InitializeComponent();
  Loaded += async (_, _) => await OperacoesTela.ExecutarAsync(this, async () => {  await CarregarAsync(); });
 }
 private async Task CarregarAsync() => dgItens.ItemsSource = await service.GenerosAsync(txtBusca.Text.Trim());
 private async void Buscar_Click(object sender, RoutedEventArgs e) => await OperacoesTela.ExecutarAsync(this, CarregarAsync);
 private async void Salvar_Click(object sender, RoutedEventArgs e)
 {
  await OperacoesTela.ExecutarAsync(this, async () =>
  {
   
   var item = new Genero { Id = _id, Nome = txtNome.Text.Trim() };
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
  await OperacoesTela.ExecutarAsync(this, async () => { await service.ExcluirAsync<Genero>(_id); Limpar(); await CarregarAsync(); });
 }
 private void Selecionar(object sender, SelectionChangedEventArgs e)
 {
  if (dgItens.SelectedItem is not Genero item) return;
  _id = item.Id; txtNome.Text=item.Nome; txtModo.Text = "Editar registro #" + _id;
 }
 private void Limpar_Click(object sender, RoutedEventArgs e) => Limpar();
 private void Limpar() { _id=0; _dataOriginal=DateTime.Now; txtNome.Clear(); dgItens.SelectedItem=null; txtModo.Text="Novo registro"; }
}
