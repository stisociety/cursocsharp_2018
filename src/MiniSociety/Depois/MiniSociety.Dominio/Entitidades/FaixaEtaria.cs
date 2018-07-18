using STI.Compartilhado.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSociety.Dominio.Entitidades
{
    public sealed class FaixaEtaria : ValueObject<FaixaEtaria>
    {
        private FaixaEtaria(int idadeMinima, int idadeMaxima)
        {
            IdadeMinima = idadeMinima;
            IdadeMaxima = idadeMaxima;
        }

        public int IdadeMinima{ get; }
        public int IdadeMaxima { get; }

        public static Resultado<FaixaEtaria, Falha>  Criar(int idadeMinima, int idadeMaxima)
        {
            if(idadeMinima < 0)
            {
                return Falha.Nova(400, "Idade minima não pode ser menor que zero");
            }
            if (idadeMaxima < 0)
            {
                return Falha.Nova(400, "Idade maxima não pode ser menor que zero");
            }
            if(idadeMinima > idadeMaxima)
            {
                return Falha.Nova(400, "Idade minima não pode ser maior que a idade maxima");
            }
            return new FaixaEtaria(idadeMinima, idadeMaxima);
        }


        protected override bool EqualsCore(FaixaEtaria other)
        {
            throw new NotImplementedException();
        }

        protected override int GetHashCodeCore()
        {
            throw new NotImplementedException();
        }
    }

}
