using Dapper;
using MiniSociety.Dominio.Crosscutting;
using MiniSociety.Dominio.Entitidades;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace MiniSociety.Dominio.Repositorios
{
    public class TurmasRepositorio
    {
        private readonly AppSettingsHelper _appSettings;

        public TurmasRepositorio(AppSettingsHelper appSettings)
        {
            _appSettings = appSettings;
        }

        public IEnumerable<Turma> Recuperar()
        {
            var sql = "SELECT Id, Descricao, Modalidade, Status, ValorMensal FROM Turmas";
            using (var conexao = new SqlConnection(_appSettings.GetConnectionString()))
            {
                return conexao
                            .Query<dynamic>(sql)
                            .Select(a => new Turma
                            {
                                Id = a.Id,
                                Descricao = a.Descricao,                                
                                Status = (TurmaStatus)((int)a.Status),
                                Modalidade = (TurmaModalidade)((int)a.Status),
                                ValorMensal = a.ValorMensal
                            })
                            .ToList();
            }
        }

        public Turma Recuperar(int id)
        {
            var sql = "SELECT Id, Descricao, Modalidade, Status, ValorMensal FROM Turmas WHERE Id = @id";
            using (var conexao = new SqlConnection(_appSettings.GetConnectionString()))
            {
                return conexao
                            .Query<dynamic>(sql, new { id })
                            .Select(a => new Turma
                            {
                                Id = a.Id,
                                Descricao = a.Descricao,
                                Status = (TurmaStatus)((int)a.Status),
                                Modalidade = (TurmaModalidade)((int)a.Status),
                                ValorMensal = a.ValorMensal
                            })
                            .FirstOrDefault();
            }
        }

        public Turma Inserir(Turma turma)
        {
            var sql = "INSERT INTO Turmas ( Descricao, Modalidade, Status, ValorMensal) VALUES (@Descricao, @Modalidade, @Status, @ValorMensal); SELECT CAST(SCOPE_IDENTITY() as int); ";
            using (var conexao = new SqlConnection(_appSettings.GetConnectionString()))
            {
                var resultado = conexao.Query<int>(sql, new { turma.Descricao, turma.Modalidade, turma.Status, turma.ValorMensal }).First();
                return Recuperar(resultado);
            }
        }
    }
}
