using Dapper;
using MiniSociety.Dominio.Crosscutting;
using MiniSociety.Dominio.Entitidades;
using System.Data.SqlClient;
using System.Linq;

namespace MiniSociety.Dominio.Repositorios
{
    public class InscricaoRepositorio
    {
        private readonly AppSettingsHelper _appSettings;

        public InscricaoRepositorio(AppSettingsHelper appSettings)
        {
            _appSettings = appSettings;
        }

        public Inscricao Recuperar(int id)
        {
            var sql = "SELECT Id, Matricula, IdAluno, IdTurma, ValorMensal, Status, InscritoEm, EmFeriasAte FROM Inscricoes WHERE Id = @id";
            using (var conexao = new SqlConnection(_appSettings.GetConnectionString()))
            { 
                return conexao
                            .Query<dynamic>(sql, new { id })
                            .Select(a => 
                            {
                                var turma = conexao
                                                .Query<dynamic>("SELECT Id, Descricao, Modalidade, Status, ValorMensal FROM Turmas WHERE Id = @id", new { id = a.IdTurma })
                                                .Select(y => new Turma
                                                {
                                                    Id = y.Id,
                                                    Descricao = y.Descricao,
                                                    Status = (TurmaStatus)((int)y.Status),
                                                    Modalidade = (TurmaModalidade)((int)y.Status),
                                                    ValorMensal = y.ValorMensal
                                                })
                                                .FirstOrDefault();
                                return new Inscricao
                                {
                                    Matricula = a.Matricula,
                                    AlunoId = a.IdAluno,
                                    Status = (InscricaoStatus)((int)a.Status),
                                    InscritoEm = a.InscritoEm,
                                    ValorMensal = a.ValorMensal,
                                    EmFeriasAte = a.EmFeriasAte,
                                    Turma = turma
                                };
                            })
                            .FirstOrDefault();
            }
        }

        public Inscricao Inserir(Inscricao inscricao)
        {
            var sql = "INSERT INTO Inscricoes ( Matricula, IdAluno, IdTurma, ValorMensal, Status, InscritoEm, EmFeriasAte) VALUES (@Matricula, @IdAluno, @IdTurma, @ValorMensal, @Status, @InscritoEm, @EmFeriasAte); SELECT CAST(SCOPE_IDENTITY() as int); ";
            using (var conexao = new SqlConnection(_appSettings.GetConnectionString()))
            {
                var resultado = conexao.Query<int>(sql, new { IdAluno = inscricao.AlunoId, IdTurma = inscricao.Turma.Id, inscricao.Matricula, inscricao.InscritoEm, inscricao.ValorMensal, inscricao.Status, inscricao.EmFeriasAte }).First();
                return Recuperar(resultado);
            }
        }
    }
}
