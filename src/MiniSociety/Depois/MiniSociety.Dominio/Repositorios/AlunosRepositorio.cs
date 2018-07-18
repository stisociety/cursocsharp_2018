using Dapper;
using MiniSociety.Dominio.Crosscutting;
using MiniSociety.Dominio.Entitidades;
using System;
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
                            .Select(a =>
                            {
                                Situacao situacao = null;
                                if (((int)a.Status) == 0)
                                    situacao = new Ativo();
                                if (((int)a.Status) == 1)
                                    situacao = new Suspenso(a.SuspensoAte);
                                return new Aluno(a.Id, Nome.Criar(a.Nome), Email.Criar(a.Email), a.DataNascimento, situacao, null);
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
                                Situacao situacao = null;
                                if (((int)a.Status) == 0)
                                    situacao = new Ativo();
                                if (((int)a.Status) == 1)
                                    situacao = new Suspenso(a.SuspensoAte);
                                return new Aluno(a.Id, Nome.Criar(a.Nome), Email.Criar(a.Email), a.DataNascimento, situacao, inscricoes);
                            })
                            .FirstOrDefault();
            }
        }

        public object Atualizar(Aluno aluno)
        {
            throw new NotImplementedException();
        }

        public Aluno RecuperarPorEmail(string email)
        {
            var sql = "SELECT Id, Nome, Email, DataNascimento, Status, SuspensoAte  FROM Alunos WHERE Email = @email";
            using (var conexao = new SqlConnection(_appSettings.GetConnectionString()))
            {
                return conexao
                            .Query<dynamic>(sql, new { email })
                            .Select(a => 
                            {
                                Situacao situacao = null;
                                if (((int)a.Status) == 0)
                                    situacao = new Ativo();
                                if (((int)a.Status) == 1)
                                    situacao = new Suspenso(a.SuspensoAte);
                                return new Aluno(a.Id, Nome.Criar(a.Nome), Email.Criar(a.Email), a.DataNascimento, situacao, null);
                            })
                            .FirstOrDefault();
            }
        }

        public Aluno Inserir(Aluno aluno)
        {
            var sql = "INSERT INTO Alunos (Nome, Email, DataNascimento, Status, SuspensoAte) VALUES (@Nome, @Email, @DataNascimento, @Status, @SuspensoAte); SELECT CAST(SCOPE_IDENTITY() as int); ";
            using (var conexao = new SqlConnection(_appSettings.GetConnectionString()))
            {
                DateTime? dataSuspensao = null;
                if (aluno.Situacao is Suspenso suspensao)
                    dataSuspensao = suspensao.AteQuando;
                var resultado = conexao.Query<int>(sql, new { Nome = aluno.Nome.Valor, Email = aluno.Email.Valor, aluno.DataNascimento, aluno.Situacao.Status, dataSuspensao }).First();
                return Recuperar(resultado);
            }
        }
    }
}
