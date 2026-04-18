using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facturacion.Contracts;

namespace Facturacion.Api.Services;

public class UsuarioPublisher
{
    private const string EXCHANGE = "usuarios";
    private readonly IEventPublisher _publisher;

    public UsuarioPublisher(IEventPublisher publisher)
    {
        _publisher = publisher;
    }

    public Task UsuarioCreado(UsuarioCreado evento)
        => _publisher.PublishAsync(EXCHANGE, "usuario.creado", evento);

    public Task UsuarioModificado(UsuarioModificado evento)
        => _publisher.PublishAsync(EXCHANGE, "usuario.modificado", evento);
}
