using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiniSociety.Dominio.Aplicacao;
using MiniSociety.Dominio.Dtos;
using MiniSociety.Dominio.Entitidades;
using MiniSociety.Dominio.Entitidades.EventosDominio;
using MiniSociety.Dominio.Repositorios;
using System.Threading.Tasks;

namespace MiniSociety.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AlunosController : Controller
    {
        private readonly AlunosRepositorio _alunosRepositorio;
        private readonly TurmasRepositorio _turmasRepositorio;
        private readonly IMediator _mediator;
        private readonly RealizarInscricaoHandler _realizarInscricaoHandler;

        public AlunosController(
            AlunosRepositorio alunosRepositorio,
            TurmasRepositorio turmasRepositorio,
            IMediator mediator,
            RealizarInscricaoHandler realizarInscricaoHandler)
        {
            _alunosRepositorio = alunosRepositorio;
            _turmasRepositorio = turmasRepositorio;
            _mediator = mediator;
            _realizarInscricaoHandler = realizarInscricaoHandler;
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
        public async Task<IActionResult> NovaInscricao(int id, [FromBody]int turmaId)
        {

            var resultado = await _mediator.Send(new RealizarInscricaoComando(id, turmaId));

            if (resultado.EhFalha)
                return StatusCode(resultado.Falha.Codigo, resultado.Falha.Mensagem);
            return CreatedAtAction(nameof(ConsultarPorId), new { id }, resultado.Sucesso);

        }
    }
}
