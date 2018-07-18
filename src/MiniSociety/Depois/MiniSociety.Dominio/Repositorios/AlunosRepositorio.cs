using Dapper;
using MiniSociety.Dominio.Crosscutting;
using MiniSociety.Dominio.Entitidades;
using STI.Compartilhado.Core;
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
                                                                        .Select(turmabd => new Turma(turmabd.Id, turmabd.Descricao, (TurmaModalidade)((int)turmabd.Status), (TurmaStatus)((int)turmabd.Status), turmabd.ValorMensal, FaixaEtaria.Criar(0, 0).Sucesso))
                                                                        .FirstOrDefault();
                                                        return new Inscricao(i.Matricula, i.IdAluno, turma, i.InscritoEm, i.ValorMensal, (InscricaoStatus)((int)i.Status), i.EmFeriasAte);
                                                    })
                                                    .ToList();
                                Situacao situacao = null;
                                if (((int)a.Status) == 0)
                                    situacao = new Ativo();
                                if (((int)a.Status) == 1)
                                    situacao = new Suspenso(a.SuspensoAte);
                                return new Aluno(a.Id, Nome.Criar(a.Nome).Sucesso, Email.Criar(a.Email).Sucesso, a.DataNascimento, situacao, inscricoes);
                            })
                            .FirstOrDefault();
            }
        }

        public Resultado<Aluno, Falha> Atualizar(Aluno aluno)
        {
            const string updateAluno = @"UPDATE Alunos SET nome = @Nome, Email = @Email, DataNascimento = @DataNascimento, @Status = @Status, SuspensoAte=@SuspensoAte WHERE Id = @Id";
            const string insertInscricao = @"INSERT INTO Inscricoes (Matricula, IdAluno, IdTurma, InscritoEm, EmFeriasAte, Status, ValorMensal) VALUES (@Matricula, @IdAluno, @IdTurma, @InscritoEm, @EmFeriasAte, @Status, @ValorMensal); SELECT CAST(SCOPE_IDENTITY() as int);";
            const string updateInscricao = @"UPDATE Inscricoes SET Matricula = @Matricula, IdAluno = @IdAluno, IdTurma = @IdTurma, InscritoEm=@InscritoEm, EmFeriasAte = @EmFeriasAte, Status = @Status, ValorMensal = @ValorMensal WHERE Id = @Id";
            const string deleteInscricao = @"DELETE FROM Inscricoes WHERE Id = @Id";
            const string recuperarInscricoes = @"SELECT id FROM Inscricoes WHERE IdAluno = @IdAluno";

            using (var conexao = new SqlConnection(_appSettings.GetConnectionString()))
            {
                try
                {
                    conexao.Open();
                    using (var transacao = conexao.BeginTransaction())
                    {
                        try
                        {
                            DateTime? dataSuspensao = null;
                            if (aluno.Situacao is Suspenso suspensao)
                                dataSuspensao = suspensao.AteQuando;
                            var resultado = conexao.Execute(updateAluno, new { Nome = aluno.Nome.Valor, Email = aluno.Email.Valor, aluno.DataNascimento, aluno.Situacao.Status, SuspensoAte=dataSuspensao, aluno.Id }, transacao);
                            if (resultado <= 0)
                                throw new InvalidOperationException("Não foi possível atualizar aluno");

                            var inscricoesParaInserir = aluno.Inscricoes.Where(inscricao => inscricao.Id == 0);
                            foreach (var inscricao in inscricoesParaInserir)
                            {
                                var idInscricao = conexao.Query<int>(insertInscricao, new { inscricao.Matricula, IdAluno = inscricao.AlunoId, IdTurma = inscricao.Turma.Id, inscricao.InscritoEm, inscricao.EmFeriasAte, inscricao.Status, inscricao.ValorMensal }, transacao).First();
                                if(idInscricao <= 0)
                                    throw new InvalidOperationException("Falha ao inserir inscrição");
                                inscricao.AtualizarId(idInscricao);
                            }

                            transacao.Commit();
                            return aluno;
                        }
                        catch (Exception ex)
                        {
                            transacao.Rollback();
                            return Falha.Nova(500, "Erro ao atualizar aluno");
                        }
                    }
                }
                finally
                {
                    conexao.Close();
                }
            }

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
