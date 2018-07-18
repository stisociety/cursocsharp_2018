using STI.Compartilhado.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSociety.Dominio.Entitidades
{
    public class Aluno
    {
        public Aluno(Nome nome, Email email, DateTime dataNascimento)
        {
            Nome = nome;
            Email = email;
            DataNascimento = dataNascimento;
            Situacao = new Ativo();
        }

        internal Aluno(int id, Nome nome, Email email, DateTime dataNascimento, Situacao situacao, IList<Inscricao> inscricoes)
        {
            Id = id;
            Nome = nome;
            Email = email;
            DataNascimento = dataNascimento;
            Situacao = situacao;
            Inscricoes = inscricoes;
        }

        public int Id { get; }
        public Nome Nome { get; }
        public Email Email { get; }
        public DateTime DataNascimento { get; }
        public Situacao Situacao { get; }
        public IList<Inscricao> Inscricoes { get; }

        public Resultado<bool, Falha> RealizarInscricao(Turma turma)
        {
            if (Situacao is Suspenso)
                return Falha.Nova(400, "Aluno precisa estar ativo.");

            if (Inscricoes.Any(i => i.Turma.Id == turma.Id))
                return Falha.Nova(400, "Aluno já inscrito para esta turma");

            var inscricao = new Inscricao(Guid.NewGuid().ToString(), Id, turma, DateTime.Now, turma.ValorMensal,
                InscricaoStatus.Ativa, null);
            Inscricoes.Add(inscricao);

            return true;
        }
    }

    public abstract class Situacao: ValueObject<Situacao>
    {
        protected Situacao(AlunoStatus status)
        {
            Status = status;
        }

        public AlunoStatus Status { get;  }    
    }

    public sealed class Ativo: Situacao
    {
        public Ativo(): base(AlunoStatus.Ativo)        {        }

        protected override bool EqualsCore(Situacao other)
            => true;

        protected override int GetHashCodeCore()
            => base.GetHashCode();

    }

    public sealed class Suspenso : Situacao
    {
        public Suspenso(DateTime ateQuando) : base(AlunoStatus.Suspenso) {
            AteQuando = ateQuando;
        }

        public DateTime AteQuando { get; }

        protected override bool EqualsCore(Situacao other)
        {
            if (other is Suspenso suspensao)
                return AteQuando.Equals(suspensao.AteQuando);
            return false;
        }
        protected override int GetHashCodeCore()
            => AteQuando.GetHashCode();
    }
}
