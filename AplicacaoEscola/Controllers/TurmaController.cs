using AplicacaoEscola.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace AplicacaoEscola.Controllers
{
    public class TurmaController : Controller
    {
        public IActionResult CadastroTurma()
        {
            return View();
        }
        public async Task<IActionResult> Index()
        {
            var client = new RestClient("https://localhost:7233/api/turma");

            var request = new RestRequest("listar-ativos", Method.Get);

            var response = await client.ExecuteAsync<List<TurmaViewModel>>(request);


            if (response.IsSuccessful)
            {
                var turmas = response.Data;
                return View(turmas);
            }
            else
            {
                TempData["ErrorMessage"] = "Erro ao obter a lista de alunos ativos da API.";
                return View(new List<AlunoViewModel>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> InserirTurma([FromForm] InserirTurmaViewModel turma)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var client = new RestClient("https://localhost:7233/api/turma");
                    var request = new RestRequest("inserir", Method.Post);
                    request.AddHeader("Content-Type", "application/json");

                    request.AddJsonBody(turma);

                    var response = await client.ExecuteAsync<InserirTurmaViewModel>(request);

                    if (response.IsSuccessful)
                    {
                        TempData["SuccessMessage"] = "Turma cadastrada com sucesso!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Erro ao cadastrar a Turma." + response.Content;
                        return RedirectToAction("CadastroTurma");
                    }
                }
                else
                {

                    TempData["ErrorMessage"] = "Por favor, preencha todos os campos com valores válidos.";
                    return RedirectToAction("CadastroTurma");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao cadastrar a Turma: " + ex.Message;
                return RedirectToAction("CadastroTurma");
            }
        }

        public async Task<IActionResult> EditarTurma(int id)
        {
            var client = new RestClient("https://localhost:7233/api/turma");
            var request = new RestRequest($"obter/{id}", Method.Get);
            var response = await client.ExecuteAsync<TurmaViewModel>(request);

            if (response.IsSuccessful)
            {
                var turma = response.Data;
                AtualizarTurmaViewModel atualizarTurmaViewModel = new AtualizarTurmaViewModel
                {
                    IdTurma = turma.IdTurma,
                    NomeTurma= turma.NomeTurma,
                    Ano = turma.Ano,
                    IdCurso= turma.IdCurso,
                    Ativo = turma.Ativo
                };
                return View(atualizarTurmaViewModel);
            }
            else
            {
                TempData["ErrorMessage"] = "Erro ao obter os detalhes da Turma da API.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarTurma([FromForm] AtualizarTurmaViewModel turma)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var client = new RestClient("https://localhost:7233/api/turma");
                    var request = new RestRequest("atualizar", Method.Put);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddJsonBody(turma);

                    var response = await client.ExecuteAsync<AtualizarTurmaViewModel>(request);

                    if (response.IsSuccessful)
                    {
                        TempData["SuccessMessage"] = "Turma atualizada com sucesso!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Erro ao atualizar a Turma." + response.Content;
                        return RedirectToAction("EditarTurma", new { id = turma.IdTurma });
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Por favor, preencha todos os campos com valores válidos.";
                    return RedirectToAction("EditarTurma", new { id = turma.IdTurma });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao atualizar a Turma: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

    }
}
