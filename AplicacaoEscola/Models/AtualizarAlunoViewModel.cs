using System.ComponentModel.DataAnnotations;

namespace AplicacaoEscola.Models
{
    public class AtualizarAlunoViewModel
    {
        [Required]
        public int IdAluno { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Usuario { get; set; }
        [Required]
        public bool Ativo { get; set; }
    }
}
