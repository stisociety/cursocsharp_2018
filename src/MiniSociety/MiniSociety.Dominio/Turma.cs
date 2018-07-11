using MiniSociety.Compartilhado.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSociety.Dominio
{
    public sealed class Turma
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public IEnumerable<Horario> Horarios { get; set; }
        public LimiteIdadeDefinicao LimiteIdade { get; set; }
        public CapacidadeDefinicao Capacidade { get; set; }
        public decimal ValorMensal { get; set; }

        public struct Horario
        {
            public string DiaDaSemana { get; set; }
            public Hora Inicio { get; set; }
            public Hora Fim { get; set; }
        }

        public struct LimiteIdadeDefinicao
        {
            public int Minimo { get; set; }
            public int Maximo { get; set; }
        }

        public struct CapacidadeDefinicao
        {
            public int Total { get; set; }
            public int Disponivel { get; set; }
        }
    }   
}
