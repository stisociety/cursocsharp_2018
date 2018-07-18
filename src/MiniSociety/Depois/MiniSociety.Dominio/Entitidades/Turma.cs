using System.ComponentModel.DataAnnotations;

namespace MiniSociety.Dominio.Entitidades
{
    public class Turma
    {
        public Turma(int id, string descricao, TurmaModalidade modalidade, TurmaStatus status, decimal valorMensal, FaixaEtaria faixaEtaria)
        {
            Id = id;
            Descricao = descricao;
            Modalidade = modalidade;
            Status = status;
            ValorMensal = valorMensal;
            FaixaEtaria = faixaEtaria;
        }

        public int Id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Descricao muito grande")]
        public string Descricao { get; set; }
        public TurmaModalidade Modalidade { get; set; }
        public TurmaStatus Status { get; set; }
        [Required]
        [Range(1,999, ErrorMessage = "Valor obrigatorio")]
        public decimal ValorMensal { get; set; }
        public FaixaEtaria FaixaEtaria { get; set; }
    }
}
