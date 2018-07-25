using MediatR;
using MiniSociety.Dominio.Entitidades;
using MiniSociety.Dominio.Entitidades.EventosDominio;
using MiniSociety.Dominio.Repositorios;
using STI.Compartilhado.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSociety.Dominio.Aplicacao
{
    public class RealizarInscricaoComando: IRequest<Resultado<Inscricao, Falha>>
    {
        public RealizarInscricaoComando(int alunoId, int turmaId)
        {
            AlunoId = alunoId;
            TurmaId = turmaId;
        }

        public int AlunoId { get;  }
        public int TurmaId { get;  }
    }

    public class RealizarInscricaoHandler : IRequestHandler<RealizarInscricaoComando, Resultado<Inscricao, Falha>>
    {
        private readonly TurmasRepositorio _turmasRepositorio;
        private readonly AlunosRepositorio _alunosRepositorio;
        private readonly IMediator _mediator;

        public RealizarInscricaoHandler(
            TurmasRepositorio turmasRepositorio,
            AlunosRepositorio alunosRepositorio,
            IMediator mediator)
        {
            _turmasRepositorio = turmasRepositorio;
            _alunosRepositorio = alunosRepositorio;
            _mediator = mediator;
        }

        public Task<Resultado<Inscricao, Falha>> Handle(RealizarInscricaoComando request, CancellationToken cancellationToken)
        {
            try
            {
                var turma = _turmasRepositorio.Recuperar(request.TurmaId);
                if (turma == null)
                    return Task.FromResult(Resultado<Inscricao, Falha>.NovaFalha(Falha.Nova(404, "Turma inexistente")));
                var aluno = _alunosRepositorio.Recuperar(request.AlunoId);
                if (aluno == null)
                    return Task.FromResult(Resultado<Inscricao, Falha>.NovaFalha(Falha.Nova(404, "Aluno inexistente")));
                var inscricao = aluno.RealizarInscricao(turma);
                if (inscricao.EhFalha)
                    return Task.FromResult(Resultado<Inscricao, Falha>.NovaFalha(Falha.Nova(400, inscricao.Falha.Mensagem)));

                _mediator.Publish<InscricaoRealizadaNotificacao>(new InscricaoRealizadaNotificacao(aluno.Id, inscricao.Sucesso.Matricula));

                var inscricaoInserida = _alunosRepositorio.Atualizar(aluno);
                return Task.FromResult(inscricao);
            }
            catch (System.Exception e)
            {
                return Task.FromResult(Resultado<Inscricao,Falha>.NovaFalha(Falha.Nova(500, e.Message)));
            }
        }
        
    }
}
