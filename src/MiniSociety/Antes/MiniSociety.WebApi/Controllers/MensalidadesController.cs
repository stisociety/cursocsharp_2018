using Microsoft.AspNetCore.Mvc;

namespace MiniSociety.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class MensalidadesController : Controller
    {
        //[HttpPost]
        //public IActionResult Inserir([FromBody] Mensalidade mensalidade)
        //{
        //    var sql = "INSERT INTO Mensalidades (IdInscricao, Vencimento, Valor) VALUES (@IdInscricao, @Vencimento, @ValorMensal)";
        //    using (var conexao = new SqlConnection(@"Data Source = DRACO-VM\SQLEXPRESS2012_2; Initial Catalog = TesteAulaGabi; User ID = sa; Password = STI000;"))
        //    {
        //        var inscricao = conexao
        //                        .Query<dynamic>("SELECT Id, ValorMensal FROM Inscricao WHERE Id = @IdInscricao", new { mensalidade.IdInscricao })
        //                        .FirstOrDefault();
        //        if (inscricao == null)
        //            return BadRequest("Inscrição inválida");

        //        var resultado = conexao.Execute(sql, new { mensalidade.IdInscricao, mensalidade.Vencimento, ValorMensal = (decimal)inscricao.ValorMensal});
        //        return Ok();
        //    }
        //}
    }
}