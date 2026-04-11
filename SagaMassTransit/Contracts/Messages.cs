// Contracts/Messages.cs
namespace Contracts;

public record CreateOrder(Guid OrderId, int ProductId, int Quantity);
public record ReserveStock(Guid OrderId, int ProductId, int Quantity);
public record ProcessPayment(Guid OrderId, decimal Amount);

public record StockReserved(Guid OrderId);
public record StockFailed(Guid OrderId, string Reason);

public record PaymentCompleted(Guid OrderId);
public record PaymentFailed(Guid OrderId, string Reason);

public record OrderCompleted(Guid OrderId);
public record OrderCancelled(Guid OrderId, string Reason);

public record ReleaseStock(Guid OrderId);