using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSociety.Dominio
{
    public sealed class Inscricao
    {
        public Inscricao(string matricula, string aluno, string turma, DateTime data, decimal valorMensal)
        {
            Matricula = matricula;
            Aluno = aluno;
            Turma = turma;
            Data = data;
            ValorMensal = valorMensal;
        }

        public string Matricula { get; }
        public string Aluno { get;}
        public string Turma { get;}
        public DateTime Data { get; }
        public decimal ValorMensal { get;}

        public static Inscricao Nova(string matricula, Aluno aluno, Turma turma)
        {
            if (aluno.DataNascimento.GetAge() < turma.LimiteIdade.Minimo ||
                aluno.DataNascimento.GetAge() > turma.LimiteIdade.Maximo)
                throw new InvalidOperationException("Limite de idade invalido");
            if(turma.Capacidade.Disponivel <=0)
                throw new InvalidOperationException("Não existe mais vagas");
            return new Inscricao(matricula, aluno.Cpf, turma.Codigo, DateTime.Now, turma.ValorMensal);
        }

    }
}
