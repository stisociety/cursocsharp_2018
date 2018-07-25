using MediatR;
using MiniSociety.Dominio.Entitidades.EventosDominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSociety.Dominio.Notificacoes
{
    public class EnviarEmailBoasVindaParaNovaInscricao : INotificationHandler<InscricaoRealizadaNotificacao>
    {
        public EnviarEmailBoasVindaParaNovaInscricao()
        {

        }
        public Task Handle(InscricaoRealizadaNotificacao notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Enviando email {notification.Matricula}");
            return Task.CompletedTask;
        }
    }
}
