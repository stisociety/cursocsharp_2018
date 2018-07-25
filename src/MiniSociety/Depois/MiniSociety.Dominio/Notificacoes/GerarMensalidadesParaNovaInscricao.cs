using MediatR;
using MiniSociety.Dominio.Entitidades.EventosDominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSociety.Dominio.Notificacoes
{
    public class GerarMensalidadesParaNovaInscricao : INotificationHandler<InscricaoRealizadaNotificacao>
    {
        public Task Handle(InscricaoRealizadaNotificacao notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Gerando mensalidades {notification.Matricula}");
            return Task.CompletedTask;
        }
    }
}
