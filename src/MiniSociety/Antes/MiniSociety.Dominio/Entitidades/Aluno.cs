using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MiniSociety.Dominio.Entitidades
{
    public class Aluno
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage ="Nome muito grande")]
        public string Nome { get; set; }
        [Required]
        [RegularExpression(@"^(.+)@(.+)$", ErrorMessage = "Email inválido")]
        public string Email { get; set; }
        [Required]
        public DateTime DataNascimento { get; set; }
        
        public AlunoStatus Status { get; set; }
        public DateTime? SuspensoAte { get; set; }
        public IList<Inscricao> Inscricoes { get; set; }
    }
}
