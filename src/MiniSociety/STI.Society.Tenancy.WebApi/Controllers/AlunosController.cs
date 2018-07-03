using Dapper;
using Microsoft.AspNetCore.Mvc;
using STI.Society.Tenancy.WebApi.Model;
using System.Data.SqlClient;

namespace MiniSociety.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AlunosController : Controller
    {
        public AlunosController()
        {
           
        }

        [HttpGet]
        public IActionResult Consultar()
        {
            var sql = "SELECT Id, Nome FROM Alunos";
            using (var conexao = new SqlConnection(@"Data Source = DRACO-VM\SQLEXPRESS2012_2; Initial Catalog = TesteAulaGabi; User ID = sa; Password = STI000;"))
            {
                var resultado = conexao.Query<dynamic>(sql);
                return Ok(resultado);
            }
        }

        [HttpPost]
        public IActionResult Inserir([FromBody] Aluno aluno)
        {
            var sql = "INSERT INTO Alunos (Nome) VALUES (@Nome)";
            using (var conexao = new SqlConnection(@"Data Source = DRACO-VM\SQLEXPRESS2012_2; Initial Catalog = TesteAulaGabi; User ID = sa; Password = STI000;"))
            {
                var resultado = conexao.Execute(sql, new { aluno.Nome});
                return Ok(resultado);
            }
        }

    }
}
