namespace MiniSociety.Compartilhado.Dominio
{
    public struct Hora
    {
        public int Horas { get; set; }
        public int Minutos { get; set; }

        public override string ToString()
            => $"{Horas.ToString().PadLeft(2, '0')}:{Minutos.ToString().PadLeft(2, '0')}";
    }
}
