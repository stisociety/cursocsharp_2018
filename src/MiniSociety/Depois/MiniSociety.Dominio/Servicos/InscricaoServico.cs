using MiniSociety.Dominio.Entitidades;
using STI.Compartilhado.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSociety.Dominio.Servicos
{
    public sealed class InscricaoServico
    {
        public Inscricao RealizarInscricao(Aluno aluno, Turma turma)
        {
            var matricula = GeradorMatricula();
            var inscricao = new Inscricao
            {
                AlunoId = aluno.Id,
                InscritoEm = DateTime.Now,
                Matricula = matricula,
                Status = InscricaoStatus.Ativa,
                EmFeriasAte = null,
                Turma = turma,
                ValorMensal = turma.ValorMensal
            };

            aluno.Inscricoes.Add(inscricao);
            return inscricao;
        }

        public string GeradorMatricula()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
