using Dapper;
using Microsoft.AspNetCore.Mvc;
using MiniSociety.WebApi.Infra;
using MiniSociety.WebApi.Model;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

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
            var sql = "INSERT INTO Inscricoes (IdAluno, IdTurma, ValorMensal) VALUES (@IdAluno, @IdTurma, @ValorMensal); SELECT SCOPE_IDENTITY();";
            using (var conexao = new SqlConnection(@"Data Source = DRACO-VM\SQLEXPRESS2012_2; Initial Catalog = TesteAulaGabi; User ID = sa; Password = STI000;"))
            {
                var aluno = conexao
                                .Query<dynamic>("SELECT Id, Nome, DataNascimeto AS 'DataNascimento' FROM Alunos WHERE Id = @IdAluno", new { inscricao.IdAluno })
                                .FirstOrDefault();
                if (aluno == null)
                    return BadRequest("Aluno inválido");

                var turma = conexao
                                .Query<dynamic>("SELECT Disponivel, IdModalidade, IdadeMinima, IdadeMaxima FROM Turmas WHERE Id = @Id", new { Id = inscricao.IdTurma })
                                .FirstOrDefault();
                if (turma == null)
                    return BadRequest("Turma inválida");
                if ((bool)turma.Disponivel == false)
                    return BadRequest("Turma indisponivel");
                var idade = ((DateTime) aluno.DataNascimento).GetAge();
                if (idade < turma.IdadeMinima || idade > turma.IdadeMaxima)
                    return BadRequest("Fora da faixa de idade para a turma.");


                var valorMensal = conexao
                                    .Query<decimal>("SELECT ValorMensal FROM Modalidades WHERE Id = @Id", new { Id = turma.IdModalidade })
                                    .FirstOrDefault();
                
                var idInscricao = conexao.Query<int>(sql, new { inscricao.IdAluno, inscricao.IdTurma, ValorMensal = valorMensal }).First();

                var sqlMensalidade = new StringBuilder();  
                for (int i = 0; i < 12; i++)
                {
                    var vencimento = DateTime.Now.AddMonths(i);
                    sqlMensalidade.Append($"INSERT INTO Mensalidades(idInscricao, vencimento, valor) VALUES ({idInscricao}, '{vencimento.ToString("yyyy-MM-ddTHH:mm:ss")}', {valorMensal});");
                }
                var result = conexao.Execute(sqlMensalidade.ToString());

                return Ok();
            }
        }

    }
}
