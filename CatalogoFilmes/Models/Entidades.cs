using System.ComponentModel.DataAnnotations;
namespace CatalogoFilmes.Models;

public abstract class Entidade { public int Id { get; set; } }
public class Genero : Entidade
{
 [Required, MaxLength(80)] public string Nome { get; set; } = "";
}
public class Diretor : Entidade
{
 [Required, MaxLength(120)] public string Nome { get; set; } = "";
 [MaxLength(80)] public string Nacionalidade { get; set; } = "";
}
public class Filme : Entidade
{
 [Required, MaxLength(150)] public string Titulo { get; set; } = "";
 public int AnoLancamento { get; set; }
 public int DuracaoMinutos { get; set; }
 public int GeneroId { get; set; }
 public int DiretorId { get; set; }
 public Genero Genero { get; set; } = null!;
 public Diretor Diretor { get; set; } = null!;
}
public class Avaliacao : Entidade
{
 public int FilmeId { get; set; }
 public decimal Nota { get; set; }
 [MaxLength(500)] public string Comentario { get; set; } = "";
 public DateTime DataAvaliacao { get; set; } = DateTime.Now;
 public Filme Filme { get; set; } = null!;
}
