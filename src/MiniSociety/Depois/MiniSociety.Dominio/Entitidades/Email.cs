using STI.Compartilhado.Core;
using System;
using System.Text.RegularExpressions;

namespace MiniSociety.Dominio.Entitidades
{
    public class Email
    {
        public string Valor { get; }

        private Email(string valor)
        {
            Valor = valor;
        }

        public static Resultado<Email, Falha> Criar(string email)
        {
            email = (email ?? string.Empty).Trim();
            if (email.Length == 0)
                return Falha.Nova(400, "Email não pode ser vazio");
            if (!Regex.IsMatch(email, @"^(.+)@(.+)$"))
                return Falha.Nova(400, "Email inválido");
            return new Email(email);
        }        

        //public static explicit operator Email(string email)
        //{
        //    return Create(email).Value;
        //}

        //public static implicit operator string(Email email)
        //{
        //    return email.Value;
        //}
    }
}
