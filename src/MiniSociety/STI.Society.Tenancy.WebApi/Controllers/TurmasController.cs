using Dapper;
using Microsoft.AspNetCore.Mvc;
using STI.Society.Tenancy.WebApi.Model;
using System.Data.SqlClient;

namespace MiniSociety.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TurmasController : Controller
    {
        public TurmasController()
        {
           
        }

        [HttpGet]
        public IActionResult Consultar([FromQuery]bool disponivel)
        {
            var sql = "SELECT Id, Descricao, IdModalidade, Disponivel FROM Turmas WHERE Disponivel = @disponivelSqlParam";
            using (var conexao = new SqlConnection(@"Data Source = DRACO-VM\SQLEXPRESS2012_2; Initial Catalog = TesteAulaGabi; User ID = sa; Password = STI000;"))
            {
                var resultado = conexao.Query<dynamic>(sql, new { disponivelSqlParam = disponivel });
                return Ok(resultado);
            }
        }

        [HttpPost]
        public IActionResult Inserir([FromBody] Turma turma)
        {
            var sql = "INSERT INTO Turmas (Descricao, IdModalidade, Disponivel) VALUES (@Descricao, @IdModalidade, @Disponivel)";
            using (var conexao = new SqlConnection(@"Data Source = DRACO-VM\SQLEXPRESS2012_2; Initial Catalog = TesteAulaGabi; User ID = sa; Password = STI000;"))
            {
                var resultado = conexao.Execute(sql, new { turma.Descricao, turma.IdModalidade, turma.Disponivel });
                return Ok(resultado);
            }
        }

    }
}
