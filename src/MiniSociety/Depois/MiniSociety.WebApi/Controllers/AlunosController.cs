using Microsoft.AspNetCore.Mvc;
using MiniSociety.Dominio.Dtos;
using MiniSociety.Dominio.Entitidades;
using MiniSociety.Dominio.Repositorios;

namespace MiniSociety.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AlunosController : Controller
    {
        private readonly AlunosRepositorio _alunosRepositorio;
        private readonly TurmasRepositorio _turmasRepositorio;

        public AlunosController(
            AlunosRepositorio alunosRepositorio,
            TurmasRepositorio turmasRepositorio)
        {
            _alunosRepositorio = alunosRepositorio;
            _turmasRepositorio = turmasRepositorio;
        }

        [HttpGet]
        public IActionResult Consultar()
            => Ok(_alunosRepositorio.Recuperar());

        [HttpGet("{id}")]
        public IActionResult ConsultarPorId(int id)
            => Ok(_alunosRepositorio.Recuperar(id));

        [HttpPost]
        public IActionResult Inserir([FromBody]CriarAlunoDto model)
        {
            try
            {
                if (_alunosRepositorio.RecuperarPorEmail(model.Email) != null)
                    return BadRequest("Email já está em uso: " + model.Email);

                var nome = Nome.Criar(model.Nome);
                if (nome.EhFalha)
                    return StatusCode(nome.Falha.Codigo, nome.Falha.Mensagem);
                var email = Email.Criar(model.Email);
                if (email.EhFalha)
                    return StatusCode(email.Falha.Codigo, email.Falha.Mensagem);

                var aluno = new Aluno(nome.Sucesso, email.Sucesso, model.DataNascimento);
                var alunoInserido = _alunosRepositorio.Inserir(aluno);

                return CreatedAtAction(nameof(ConsultarPorId), new { id = alunoInserido.Id }, alunoInserido);
            }
            catch (System.Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        [HttpPost("{id}/inscricoes")]
        public IActionResult NovaInscricao(int id, [FromBody]int turmaId)
        {
            try
            {
                var turma = _turmasRepositorio.Recuperar(turmaId);
                if (turma == null)
                    return BadRequest("Turma inexistente");
                var aluno = _alunosRepositorio.Recuperar(id);
                if (aluno == null)
                    return BadRequest("Aluno inexistente");

                var inscricao = aluno.RealizarInscricao(turma);
                if (inscricao.EhFalha)
                    return BadRequest(inscricao.Falha.Mensagem);

                var mensalidade = _fabricaMensalidades.GerarProximosMeses(inscricao.Sucesso, 3);



                var inscricaoInserida = _alunosRepositorio.Atualizar(aluno);
                return CreatedAtAction(nameof(ConsultarPorId), new { id }, aluno);
            }
            catch (System.Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }
    }
}
