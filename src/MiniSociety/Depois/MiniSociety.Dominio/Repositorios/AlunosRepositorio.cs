using Dapper;
using MiniSociety.Dominio.Crosscutting;
using MiniSociety.Dominio.Entitidades;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace MiniSociety.Dominio.Repositorios
{
    public class AlunosRepositorio
    {
        private readonly AppSettingsHelper _appSettings;

        public AlunosRepositorio(AppSettingsHelper appSettings)
        {
            _appSettings = appSettings;
        }

        public IEnumerable<Aluno> Recuperar()
        {
            var sql = "SELECT Id, Nome, Email, DataNascimento, Status, SuspensoAte  FROM Alunos";
            using (var conexao = new SqlConnection(_appSettings.GetConnectionString()))
            {
                return conexao
                            .Query<dynamic>(sql)
                            .Select(a => new Aluno
                            {
                                Id = a.Id,
                                Nome = Nome.Criar(a.Nome),
                                Email = a.Email,
                                Status = (AlunoStatus)((int)a.Status),
                                DataNascimento = a.DataNascimento,
                                SuspensoAte = a.SuspensoAte,
                                Inscricoes = null
                            })
                            .ToList();
            }
        }

        public Aluno Recuperar(int id)
        {
            var sql = "SELECT Id, Nome, Email, DataNascimento, Status, SuspensoAte  FROM Alunos WHERE Id = @id";
            using (var conexao = new SqlConnection(_appSettings.GetConnectionString()))
            {
                return conexao
                            .Query<dynamic>(sql, new { id })
                            .Select(a =>
                            {
                                var inscricoes = conexao
                                                    .Query<dynamic>("SELECT Id, Matricula, IdAluno, IdTurma, ValorMensal, Status, InscritoEm, EmFeriasAte FROM Inscricoes WHERE IdAluno = @id", new { id })
                                                    .Select(i =>
                                                    {
                                                        var turma = conexao
                                                                        .Query<dynamic>("SELECT Id, Descricao, Modalidade, Status, ValorMensal FROM Turmas WHERE Id = @id", new { id = i.IdTurma })
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
                                                            Matricula = i.Matricula,
                                                            AlunoId = i.IdAluno,
                                                            Status = (InscricaoStatus)((int)i.Status),
                                                            InscritoEm = i.InscritoEm,
                                                            ValorMensal = i.ValorMensal,
                                                            EmFeriasAte = i.EmFeriasAte,
                                                            Turma = turma
                                                        };
                                                    })
                                                    .ToList();
                                return new Aluno
                                {
                                    Id = a.Id,
                                    Nome = Nome.Criar(a.Nome),
                                    Email = a.Email,
                                    Status = (AlunoStatus)((int)a.Status),
                                    DataNascimento = a.DataNascimento,
                                    SuspensoAte = a.SuspensoAte,
                                    Inscricoes = inscricoes
                                };
                            })
                            .FirstOrDefault();
            }
        }

        public Aluno RecuperarPorEmail(string email)
        {
            var sql = "SELECT Id, Nome, Email, DataNascimento, Status, SuspensoAte  FROM Alunos WHERE Email = @email";
            using (var conexao = new SqlConnection(_appSettings.GetConnectionString()))
            {
                return conexao
                            .Query<dynamic>(sql, new { email })
                            .Select(a => new Aluno
                            {
                                Id = a.Id,
                                Nome = Nome.Criar(a.Nome),
                                Email = a.Email,
                                Status = (AlunoStatus)((int)a.Status),
                                DataNascimento = a.DataNascimento,
                                SuspensoAte = a.SuspensoAte,
                                Inscricoes = null
                            })
                            .FirstOrDefault();
            }
        }

        public Aluno Inserir(Aluno aluno)
        {
            var sql = "INSERT INTO Alunos (Nome, Email, DataNascimento, Status, SuspensoAte) VALUES (@Nome, @Email, @DataNascimento, @Status, @SuspensoAte); SELECT CAST(SCOPE_IDENTITY() as int); ";
            using (var conexao = new SqlConnection(_appSettings.GetConnectionString()))
            {
                var resultado = conexao.Query<int>(sql, new { Nome = aluno.Nome.Valor, aluno.Email, aluno.DataNascimento, aluno.Status, aluno.SuspensoAte }).First();
                return Recuperar(resultado);
            }
        }
    }
}
