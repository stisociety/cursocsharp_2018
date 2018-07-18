using STI.Compartilhado.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSociety.Dominio.Entitidades
{
    public class Nome: ValueObject<Nome>
    {
        public string Valor { get; }

        private Nome(string value)
        {
            Valor = value;
        }

        public static Resultado<Nome, Falha> Criar(string nome)
        {
            nome = (nome ?? string.Empty).Trim();
            if (String.IsNullOrEmpty(nome))
                return Falha.Nova(400, "Nome inválido");
            if (nome.Length > 100)
                return Falha.Nova(400, "Nome muito grande");
            return new Nome(nome);
        }

        protected override bool EqualsCore(Nome other)
            => Valor.Equals(other.Valor);

        protected override int GetHashCodeCore()
            => Valor.GetHashCode();

        //public static implicit operator string(CustomerName customerName)
        //{
        //    return customerName.Value;
        //}

        //public static explicit operator CustomerName(string customerName)
        //{
        //    return Create(customerName).Value;
        //}
    }
}
