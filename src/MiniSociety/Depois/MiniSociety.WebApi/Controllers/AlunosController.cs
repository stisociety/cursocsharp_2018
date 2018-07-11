using Microsoft.AspNetCore.Mvc;
using MiniSociety.Dominio.Entitidades;
using MiniSociety.Dominio.Repositorios;
using MiniSociety.Dominio.Servicos;
using System.Linq;

namespace MiniSociety.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AlunosController : Controller
    {
        private readonly AlunosRepositorio _alunosRepositorio;
        private readonly TurmasRepositorio _turmasRepositorio;
        private readonly InscricaoServico _servicoInscricao;
        private readonly InscricaoRepositorio _inscricaoRepositorio;

        public AlunosController(
            AlunosRepositorio alunosRepositorio,
            TurmasRepositorio turmasRepositorio,
            InscricaoServico servicoInscricao,
            InscricaoRepositorio inscricaoRepositorio)
        {
            _alunosRepositorio = alunosRepositorio;
            _turmasRepositorio = turmasRepositorio;
            _servicoInscricao = servicoInscricao;
            _inscricaoRepositorio = inscricaoRepositorio;
        }

        [HttpGet]
        public IActionResult Consultar()
            => Ok(_alunosRepositorio.Recuperar());

        [HttpGet("{id}")]
        public IActionResult ConsultarPorId(int id)
            => Ok(_alunosRepositorio.Recuperar(id));

        [HttpPost]
        public IActionResult Inserir([FromBody]Aluno aluno)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                
                if (_alunosRepositorio.RecuperarPorEmail(aluno.Email) != null)                
                    return BadRequest("Email já está em uso: " + aluno.Email);

                aluno.Id = 0;
                aluno.Status = AlunoStatus.Ativo;
                var alunoInserido = _alunosRepositorio.Inserir(aluno);

                return CreatedAtAction(nameof(ConsultarPorId), new { id = aluno.Id }, alunoInserido);
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
                    return BadRequest("Turma inválida");
                var aluno = _alunosRepositorio.Recuperar(id);
                if (aluno == null)
                    return BadRequest("Aluno inválido");
                if (aluno.Inscricoes.Any(i => i.Turma.Equals(turma.Id)))
                    return BadRequest("Aluno já inscrito para esta turma");

                var inscricao = _servicoInscricao.RealizarInscricao(aluno, turma);
                var inscricaoInserida = _inscricaoRepositorio.Inserir(inscricao);
                return CreatedAtAction(nameof(ConsultarPorId), new { id }, aluno);
            }
            catch (System.Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }
    }
}
