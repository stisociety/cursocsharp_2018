using System;

namespace MiniSociety.Dominio.Dtos
{
    public class CriarAlunoDto
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}
