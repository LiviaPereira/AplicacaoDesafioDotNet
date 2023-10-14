using AplicacaoEscola.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace AplicacaoEscola.Controllers
{
    public class AlunoTurma : Controller
    {
        public async Task<IActionResult> Index()
        {
            var client = new RestClient("https://localhost:7233/api/alunoturma");

            var request = new RestRequest("listar-relacoes", Method.Get);

            var response = await client.ExecuteAsync<List<ListaAlunoTurmaViewModel>>(request);


            if (response.IsSuccessful)
            {
                var alunosturmas = response.Data;
                return View(alunosturmas);
            }
            else
            {
                TempData["ErrorMessage"] = "Erro ao obter a lista de relações entre alunos e turmas da API.";
                return View(new List<ListaAlunoTurmaViewModel>());
            }
        }

        public async Task<IActionResult> CadastroAlunoTurma()
        {
            var client = new RestClient("https://localhost:7233/api/turma");

            var requestTurmas = new RestRequest("listar-ativos", Method.Get);

            var responseTurmas = await client.ExecuteAsync<List<TurmaViewModel>>(requestTurmas);


            var client2 = new RestClient("https://localhost:7233/api/aluno");

            var requestAlunos = new RestRequest("listar-ativos", Method.Get);

            var responseAlunos = await client2.ExecuteAsync<List<AlunoViewModel>>(requestAlunos);


            if (responseTurmas.IsSuccessful && responseAlunos.IsSuccessful)
            {
                var alunos = responseAlunos.Data;
                var turmas = responseTurmas.Data;

                CadastroAlunoTurmaViewModel cadastroAlunoTurmaViewModel = new CadastroAlunoTurmaViewModel
                {
                    Alunos = alunos,
                    Turmas = turmas
                };

                return View(cadastroAlunoTurmaViewModel);
                
            }
            else
            {
                TempData["ErrorMessage"] = "Erro ao obter a lista de alunos e turmas da API.";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CriarRelacao([FromForm] CadastroAlunoTurmaViewModel cadastroAlunoTurmaViewModel)
        {
            try
            {

                var client = new RestClient("https://localhost:7233/api/alunoturma");
                var request = new RestRequest("inserir", Method.Post);
                request.AddHeader("Content-Type", "application/json");

                AlunoTurmaViewModel alunoTurmaViewModel = new AlunoTurmaViewModel
                {
                    IdAluno = cadastroAlunoTurmaViewModel.SelectedAlunoId,
                    IdTurma = cadastroAlunoTurmaViewModel.SelectedTurmaId
                };
                request.AddJsonBody(alunoTurmaViewModel);
                
                var response = await client.ExecuteAsync<AlunoTurmaViewModel>(request);

                if (response.IsSuccessful)
                {
                    TempData["SuccessMessage"] = "Relação cadastrada com sucesso!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Erro ao cadastrar a relação." + response.Content;
                    return RedirectToAction("CadastroAlunoTurma");
                }


            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao cadastrar a relação: " + ex.Message;
                return RedirectToAction("CadastroAlunoTurma");
            }
        }

        public async Task<IActionResult> ExcluirRelacao(int idAluno, int idTurma)
        {
            var client = new RestClient("https://localhost:7233/api/alunoturma");
            var request = new RestRequest($"excluir/{idAluno}/{idTurma}", Method.Delete);

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                TempData["SuccessMessage"] = "Relação excluida com sucesso!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Erro ao excluir relação:" + response.Content;
                return RedirectToAction("Index");
            }
        }

    }
}
