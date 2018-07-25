using MediatR;

namespace MiniSociety.Dominio.Entitidades.EventosDominio
{
    public class InscricaoRealizadaNotificacao: INotification
    {
        public InscricaoRealizadaNotificacao(int alunoId, string matricula)
        {
            AlunoId = alunoId;
            Matricula = matricula;
        }

        public int AlunoId { get;  }
        public string Matricula { get;  }
    }
}
