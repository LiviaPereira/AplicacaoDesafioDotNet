using System.ComponentModel.DataAnnotations;

namespace AplicacaoEscola.Models
{
    public class InserirTurmaViewModel
    {
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Informe um número válido para o ID do Curso.")]
        public int IdCurso { get; set; }
        [Required]
        public string NomeTurma { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Informe um ano válido no formato YYYY.")]
        public int Ano { get; set; }
    }
}
