using MiniSociety.Compartilhado.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSociety.Dominio
{
    public sealed class Aluno
    {
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public Genero Genero { get; set; }
    }
}
