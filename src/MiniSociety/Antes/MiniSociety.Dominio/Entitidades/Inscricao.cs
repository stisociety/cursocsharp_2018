using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSociety.Dominio.Entitidades
{
    public class Inscricao
    {
        public string Matricula { get; set; }
        [JsonIgnore]
        public int AlunoId { get; set; }
        public Turma Turma { get; set; }
        public DateTime InscritoEm { get; set; }
        public decimal ValorMensal { get; set; }
        public InscricaoStatus Status { get; set; }
        public DateTime? EmFeriasAte { get; set; }
    }
}
