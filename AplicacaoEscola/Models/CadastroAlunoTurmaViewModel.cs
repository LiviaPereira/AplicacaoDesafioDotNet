namespace AplicacaoEscola.Models
{
    public class CadastroAlunoTurmaViewModel
    {
        
        public List<AlunoViewModel> Alunos { get; set; }
        public List<TurmaViewModel> Turmas { get; set; }
        public int SelectedAlunoId { get; set; }
        public int SelectedTurmaId { get; set; }
    }
}
