namespace Facturacion.Contracts;

public record FacturaCreada(Guid Id, decimal Precio);
public record FacturaPagada(Guid Id, decimal Precio);
public record FacturaCancelada(Guid Id, decimal Precio);