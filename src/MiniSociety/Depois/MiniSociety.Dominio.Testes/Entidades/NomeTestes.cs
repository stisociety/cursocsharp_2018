using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniSociety.Dominio.Entitidades;
using STI.Compartilhado.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSociety.Dominio.Testes.Entidades
{
    [TestClass]
    public class NomeTestes
    {
        [TestMethod]
        public void Nome_Deve_Ser_Valido_Quando_Nao_For_Vazio_E_Menor_Que_100_Letras()
        {
            //Arrange
            var input = "Gabriel";

            //Act
            var resultadoNome = Nome.Criar(input);

            //Assert
            Assert.IsTrue(resultadoNome.EhSucesso);
            Assert.AreEqual(input, resultadoNome.Sucesso.Valor);
        }

        [TestMethod]
        public void Nao_Posso_Criar_Nome_Vazio()
        {
            //Arrange
            var input = "";

            //Act
            var resultadoNome = Nome.Criar(input);

            //Assert
            Assert.IsTrue(resultadoNome.EhFalha);
            Assert.AreEqual(Falha.Nova(400, "Nome inválido"), resultadoNome.Falha);
        }

        [TestMethod]
        public void Nao_Posso_Criar_Nome_Com_Mais_De_100_Letras()
        {
            //Arrange
            var input = "Gabriel".PadRight(101, 'O');

            //Act
            var resultadoNome = Nome.Criar(input);

            //Assert
            Assert.IsTrue(resultadoNome.EhFalha);
            Assert.AreEqual(Falha.Nova(400, "Nome muito grande"), resultadoNome.Falha);
        }

        [TestMethod]
        public void Nomes_Iguais_Devem_Ser_Os_Mesmos()
        {
            var nome1 = Nome.Criar("Gabriel").Sucesso;
            var nome2 = Nome.Criar("Gabriel").Sucesso;

            var resultado = nome1 == nome2;
            
            Assert.IsTrue(resultado);
            Assert.AreEqual(nome1.GetHashCode(), nome2.GetHashCode());
        }
    }
}
