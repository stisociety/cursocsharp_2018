﻿using Microsoft.AspNetCore.Mvc;
using MiniSociety.Dominio.Dtos;
using MiniSociety.Dominio.Entitidades;
using MiniSociety.Dominio.Repositorios;
using MiniSociety.Dominio.Servicos;
using System.Linq;

namespace MiniSociety.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AlunosController : Controller
    {
        private readonly AlunosRepositorio _alunosRepositorio;
        private readonly TurmasRepositorio _turmasRepositorio;
        private readonly InscricaoServico _servicoInscricao;
        private readonly InscricaoRepositorio _inscricaoRepositorio;

        public AlunosController(
            AlunosRepositorio alunosRepositorio,
            TurmasRepositorio turmasRepositorio,
            InscricaoServico servicoInscricao,
            InscricaoRepositorio inscricaoRepositorio)
        {
            _alunosRepositorio = alunosRepositorio;
            _turmasRepositorio = turmasRepositorio;
            _servicoInscricao = servicoInscricao;
            _inscricaoRepositorio = inscricaoRepositorio;
        }

        [HttpGet]
        public IActionResult Consultar()
            => Ok(_alunosRepositorio.Recuperar());

        [HttpGet("{id}")]
        public IActionResult ConsultarPorId(int id)
            => Ok(_alunosRepositorio.Recuperar(id));

        [HttpPost]
        public IActionResult Inserir([FromBody]CriarAlunoDto model)
        {
            try
            {
                if (_alunosRepositorio.RecuperarPorEmail(model.Email) != null)
                    return BadRequest("Email já está em uso: " + model.Email);

                var nome = Nome.Criar(model.Nome);
                if (nome.EhFalha)
                    return StatusCode(nome.Falha.Codigo, nome.Falha.Mensagem);

                var aluno = new Aluno(nome.Sucesso, model.Email, model.DataNascimento);
                var alunoInserido = _alunosRepositorio.Inserir(aluno);

                return CreatedAtAction(nameof(ConsultarPorId), new { id = alunoInserido.Id }, alunoInserido);
            }
            catch (System.Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        [HttpPost("{id}/inscricoes")]
        public IActionResult NovaInscricao(int id, [FromBody]int turmaId)
        {
            try
            {
                var turma = _turmasRepositorio.Recuperar(turmaId);
                if (turma == null)
                    return BadRequest("Turma inválida");
                var aluno = _alunosRepositorio.Recuperar(id);
                if (aluno == null)
                    return BadRequest("Aluno inválido");
                if (aluno.Inscricoes.Any(i => i.Turma.Id == turma.Id))
                    return BadRequest("Aluno já inscrito para esta turma");

                var inscricao = _servicoInscricao.RealizarInscricao(aluno, turma);

                var inscricaoInserida = _inscricaoRepositorio.Inserir(inscricao);
                return CreatedAtAction(nameof(ConsultarPorId), new { id }, aluno);
            }
            catch (System.Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }
    }
}
