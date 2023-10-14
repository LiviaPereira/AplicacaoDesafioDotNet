using System.ComponentModel.DataAnnotations;

namespace AplicacaoEscola.Models
{
    public class InserirAlunoViewModel
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Usuario { get; set; }
        [Required]
        public string Senha { get; set; }
    }
}
