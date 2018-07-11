using Microsoft.AspNetCore.Mvc;
using MiniSociety.Dominio.Entitidades;
using MiniSociety.Dominio.Repositorios;

namespace MiniSociety.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TurmasController : Controller
    {
        private readonly TurmasRepositorio _turmasRepositorio;

        public TurmasController(TurmasRepositorio turmasRepositorio)
        {
            _turmasRepositorio = turmasRepositorio;
        }

        [HttpGet]
        public IActionResult Consultar()
            => Ok(_turmasRepositorio.Recuperar());

        [HttpGet("{id}")]
        public IActionResult ConsultarPorId(int id)
            => Ok(_turmasRepositorio.Recuperar(id));

        [HttpPost]
        public IActionResult Inserir([FromBody]Turma turma)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                turma.Id = 0;
                turma.Status = TurmaStatus.Fechada;
                var turmaInserida = _turmasRepositorio.Inserir(turma);

                return CreatedAtAction(nameof(ConsultarPorId), new { id = turma.Id }, turmaInserida);
            }
            catch (System.Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }
    }
}
