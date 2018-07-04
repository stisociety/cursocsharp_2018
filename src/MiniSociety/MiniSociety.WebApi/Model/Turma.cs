namespace MiniSociety.WebApi.Model
{
    public class Turma
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public int IdModalidade { get; set; }
        public bool Disponivel { get; set; }
    }
}