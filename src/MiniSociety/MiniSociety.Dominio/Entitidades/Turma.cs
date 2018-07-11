using System.ComponentModel.DataAnnotations;

namespace MiniSociety.Dominio.Entitidades
{
    public class Turma
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Descricao muito grande")]
        public string Descricao { get; set; }
        public TurmaModalidade Modalidade { get; set; }
        public TurmaStatus Status { get; set; }
        [Required]
        [Range(1,999, ErrorMessage = "Valor obrigatorio")]
        public decimal ValorMensal { get; set; }
    }
}
