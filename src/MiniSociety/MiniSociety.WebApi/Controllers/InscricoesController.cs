using Dapper;
using Microsoft.AspNetCore.Mvc;
using MiniSociety.WebApi.Model;
using System.Data.SqlClient;
using System.Linq;

namespace MiniSociety.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class InscricoesController : Controller
    {
        public InscricoesController()
        {
           
        }

        [HttpGet]
        public IActionResult Consultar()
        {
            var sql = "SELECT Id, IdAluno, IdTurma FROM Inscricoes";
            using (var conexao = new SqlConnection(@"Data Source = DRACO-VM\SQLEXPRESS2012_2; Initial Catalog = TesteAulaGabi; User ID = sa; Password = STI000;"))
            {
                var resultado = conexao.Query<dynamic>(sql);
                return Ok(resultado);
            }
        }

        [HttpPost]
        public IActionResult Inserir([FromBody] Inscricao inscricao)
        {
            var sql = "INSERT INTO Inscricoes (IdAluno, IdTurma, ValorMensal) VALUES (@IdAluno, @IdTurma, @ValorMensal)";
            using (var conexao = new SqlConnection(@"Data Source = DRACO-VM\SQLEXPRESS2012_2; Initial Catalog = TesteAulaGabi; User ID = sa; Password = STI000;"))
            {
                var aluno = conexao
                                .Query<dynamic>("SELECT Id, Nome FROM Aluno WHERE Id = @IdAluno", new { inscricao.IdAluno })
                                .FirstOrDefault();
                if (aluno == null)
                    return BadRequest("Aluno inválido");

                var turma = conexao
                                .Query<dynamic>("SELECT Disponivel, IdModalidade FROM Turmas WHERE Id = @Id", new { Id = inscricao.IdTurma })
                                .FirstOrDefault();
                if (turma == null)
                    return BadRequest("Turma inválida");
                if ((bool)turma.Disponivel == false)
                    return BadRequest("Turma indisponivel");

                var valorMensal = conexao
                                    .Query<decimal>("SELECT ValorMensal FROM Modalidade WHERE Id = @Id", new { Id = turma.IdModalidade })
                                    .FirstOrDefault();
                
                var resultado = conexao.Execute(sql, new { inscricao.IdAluno, inscricao.IdTurma, ValorMensal = valorMensal });
                return Ok();
            }
        }

    }
}
