using System.ComponentModel.DataAnnotations;

namespace AplicacaoEscola.Models
{
    public class AlunoViewModel
    {
        [Key]
        public int IdAluno { get; set; }
        public string Nome { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public bool Ativo { get; set; }
    }
}
