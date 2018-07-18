using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniSociety.Dominio.Entitidades;
using STI.Compartilhado.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniSociety.Dominio.Testes.Entidades
{
    [TestClass]
    public class AlunoIncricaoTestes
    {
        [TestMethod]
        public void Nao_posso_inscrever_aluno_suspenso()
        {
            //Arrange
            var aluno = new Aluno(Nome.Criar("Gabreiel").Sucesso , Email.Criar("gabriel@society.com").Sucesso, DateTime.Now.AddYears(-18));
            aluno.Suspender(DateTime.Now.AddDays(7));
            var turma = new Turma(39, "aula de C#", TurmaModalidade.Tenis, TurmaStatus.Aberta, 123.55M, FaixaEtaria.Criar(0,20).Sucesso);

            //Act
            var resultado = aluno.RealizarInscricao(turma); 
            
            //Assert
            Assert.IsTrue(resultado.EhFalha);
          //  Assert.AreEqual(input, resultadoNome.Sucesso.Valor);
        }

        [TestMethod]
        public void Nao_posso_inscrever_aluno_em_turma_ja_inscrita()
        {
            //Arrange
            var aluno = new Aluno(Nome.Criar("Gabreiel").Sucesso, Email.Criar("gabriel@society.com").Sucesso, DateTime.Now.AddYears(-18));
            var turma = new Turma(39, "aula de C#", TurmaModalidade.Tenis, TurmaStatus.Aberta, 123.55M, FaixaEtaria.Criar(0, 20).Sucesso);
            aluno.RealizarInscricao(turma);

            //Act
            var resultado = aluno.RealizarInscricao(turma);

            //Assert
            Assert.IsTrue(resultado.EhFalha);
            //  Assert.AreEqual(input, resultadoNome.Sucesso.Valor);
        }

        [TestMethod]
        public void Nao_posso_inscrever_aluno_fora_da_faixa_etaria()
        {
            //Arrange
            var aluno = new Aluno(Nome.Criar("Gabreiel").Sucesso, Email.Criar("gabriel@society.com").Sucesso, DateTime.Now.AddYears(-19));
            var turma = new Turma(39, "aula de C#", TurmaModalidade.Tenis, TurmaStatus.Aberta, 123.55M, FaixaEtaria.Criar(5, 17).Sucesso);
            

            //Act
            var resultado = aluno.RealizarInscricao(turma);

            //Assert
            Assert.IsTrue(resultado.EhFalha);
            //  Assert.AreEqual(input, resultadoNome.Sucesso.Valor);
        }

        [TestMethod]
        public void Nao_posso_inscrever_aluno_em_Turma_Fechada()
        {
            //Arrange
            var aluno = new Aluno(Nome.Criar("Gabreiel").Sucesso, Email.Criar("gabriel@society.com").Sucesso, DateTime.Now.AddYears(-19));
            var turma = new Turma(39, "aula de C#", TurmaModalidade.Tenis, TurmaStatus.Fechada, 123.55M, FaixaEtaria.Criar(5, 23).Sucesso);

            //Act
            var resultado = aluno.RealizarInscricao(turma);

            //Assert
            Assert.IsTrue(resultado.EhFalha);
            //  Assert.AreEqual(input, resultadoNome.Sucesso.Valor);
        }

        [TestMethod]
        public void devo_inscrever_aluno_valido_em_uma_turma()
        {
            //Arrange
            var aluno = new Aluno(Nome.Criar("Gabreiel").Sucesso, Email.Criar("gabriel@society.com").Sucesso, DateTime.Now.AddYears(-17));
            var turma = new Turma(39, "aula de C#", TurmaModalidade.Tenis, TurmaStatus.Aberta, 123.55M, FaixaEtaria.Criar(5, 18).Sucesso);


            //Act
            var resultado = aluno.RealizarInscricao(turma);

            //Assert
            Assert.IsTrue(resultado.EhSucesso);
            Assert.AreEqual(1, aluno.Inscricoes.Count());
            Assert.AreEqual(turma.Id, aluno.Inscricoes.FirstOrDefault().Turma.Id);
            Assert.AreEqual(turma.ValorMensal, aluno.Inscricoes.FirstOrDefault().ValorMensal);
            //  Assert.AreEqual(input, resultadoNome.Sucesso.Valor);
        }
    }
}
