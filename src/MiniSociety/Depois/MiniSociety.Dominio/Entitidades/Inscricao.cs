using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSociety.Dominio.Entitidades
{
    public class Inscricao
    {
        public Inscricao(string matricula, int alunoId, Turma turma, DateTime inscritoEm, decimal valorMensal, InscricaoStatus status, DateTime? emFeriasAte)
        {
            Matricula = matricula;
            AlunoId = alunoId;
            Turma = turma;
            InscritoEm = inscritoEm;
            ValorMensal = valorMensal;
            Status = status;
            EmFeriasAte = emFeriasAte;
        }

        public string Matricula { get; }
        public int AlunoId { get; }
        public Turma Turma { get; }
        public DateTime InscritoEm { get; }
        public decimal ValorMensal { get; }
        public InscricaoStatus Status { get; }
        public DateTime? EmFeriasAte { get; }
    }
}
