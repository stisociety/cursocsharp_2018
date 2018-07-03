using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace ExemploTesteUnitario
{
    public enum TurnoDefinacao { Manha, Tarde, Noite }

    public sealed class Inscricao
    {
        public Inscricao(TurnoDefinacao turno, decimal valorMensal)
        {
            Turno = turno;
            ValorMensal = valorMensal;
        }

        public TurnoDefinacao Turno { get; }
        public decimal ValorMensal { get; }

        public decimal GerarMensalidade()
        {
            if (Turno.Equals(TurnoDefinacao.Manha)) return ValorMensal - (ValorMensal * 0.10M) ;
            if (Turno.Equals(TurnoDefinacao.Tarde)) return ValorMensal * 0.30M;
            return ValorMensal;
        }
    }


    [TestClass]
    public class TesteDescontoSimples
    {
        [TestMethod]
        public void Devo_Calcular_Mensalidade_Com_Desconto_Quando_Turno_Eh_Manha()
        {
            var inscricao = new Inscricao(TurnoDefinacao.Manha, 100M);

            var mensalidade = inscricao.GerarMensalidade();

            mensalidade.ShouldBe(90M);
        }
    }
}
