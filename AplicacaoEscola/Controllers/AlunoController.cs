using AplicacaoEscola.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Diagnostics;

namespace AplicacaoEscola.Controllers
{
    public class AlunoController : Controller
    {        

        public async Task<IActionResult> Index()
        {
            var client = new RestClient("https://localhost:7233/api/aluno");

            var request = new RestRequest("listar-ativos", Method.Get);
            
            var response = await client.ExecuteAsync<List<AlunoViewModel>>(request);

            
            if (response.IsSuccessful)
            {
                var alunos = response.Data;
                return View(alunos);
            }
            else
            {
                TempData["ErrorMessage"] = "Erro ao obter a lista de alunos ativos da API.";
                return View(new List<AlunoViewModel>());
            }
        }

        public IActionResult CadastroAluno()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InserirAluno([FromForm] InserirAlunoViewModel aluno)
        {
            try
            {
                if (ModelState.IsValid) 
                {
                    var client = new RestClient("https://localhost:7233/api/aluno");
                    var request = new RestRequest("inserir", Method.Post);
                    request.AddHeader("Content-Type", "application/json");

                    request.AddJsonBody(aluno); 

                    var response = await client.ExecuteAsync<InserirAlunoViewModel>(request);

                    if (response.IsSuccessful)
                    {
                        TempData["SuccessMessage"] = "Aluno cadastrado com sucesso!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Erro ao cadastrar o aluno." + response.Content;
                        return RedirectToAction("CadastroAluno");
                    }
                }
                else
                {
                    
                    TempData["ErrorMessage"] = "Por favor, preencha todos os campos." ;
                    return RedirectToAction("CadastroAluno");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao cadastrar o aluno: " + ex.Message;
                return RedirectToAction("CadastroAluno");
            }
        }

        public async Task<IActionResult> EditarAluno(int id)
        {
            var client = new RestClient("https://localhost:7233/api/aluno");
            var request = new RestRequest($"obter/{id}", Method.Get);
            var response = await client.ExecuteAsync<AlunoViewModel>(request);

            if (response.IsSuccessful)
            {
                var aluno = response.Data;
                AtualizarAlunoViewModel atualizarAlunoViewModel = new AtualizarAlunoViewModel
                {
                    Nome = aluno.Nome,
                    Usuario = aluno.Usuario,
                    Ativo = aluno.Ativo,
                    IdAluno = aluno.IdAluno
                };
                return View(atualizarAlunoViewModel);
            }
            else
            {
                TempData["ErrorMessage"] = "Erro ao obter os detalhes do aluno da API.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarAluno([FromForm] AtualizarAlunoViewModel aluno)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var client = new RestClient("https://localhost:7233/api/aluno");
                    var request = new RestRequest("atualizar", Method.Put);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddJsonBody(aluno);

                    var response = await client.ExecuteAsync<AtualizarAlunoViewModel>(request);

                    if (response.IsSuccessful)
                    {
                        TempData["SuccessMessage"] = "Aluno atualizado com sucesso!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Erro ao atualizar o aluno." + response.Content;
                        return RedirectToAction("EditarAluno", new { id = aluno.IdAluno });
                    }
                } else
                {
                    TempData["ErrorMessage"] = "Por favor, preencha todos os campos com valores válidos.";
                    return RedirectToAction("EditarAluno", new { id = aluno.IdAluno });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao atualizar o aluno: " + ex.Message;
                return RedirectToAction("Index");
            }
        }



    }




}