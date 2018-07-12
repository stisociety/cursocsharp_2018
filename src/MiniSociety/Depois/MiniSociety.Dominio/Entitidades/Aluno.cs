using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace MiniSociety.Dominio.Entitidades
{
    public class Aluno
    {
        internal Aluno() { }

        public Aluno(Nome nome, string email, DateTime dataNascimento)
        {
            
            if(!Regex.IsMatch(email, @"^(.+)@(.+)$"))
                throw new ArgumentException("Email inválido");
            Nome = nome;
            Email = email;
            DataNascimento = dataNascimento;
            Status = AlunoStatus.Ativo;
        }

        public int Id { get; set; }
        public Nome Nome { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }
        public AlunoStatus Status { get; set; }
        public DateTime? SuspensoAte { get; set; }
        public IList<Inscricao> Inscricoes { get; set; }
    }
}
