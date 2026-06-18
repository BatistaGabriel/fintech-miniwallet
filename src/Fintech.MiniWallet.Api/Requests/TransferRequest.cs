namespace Fintech.MiniWallet.Api.Requests;

public record TransferRequest(Guid OriginAccountId, Guid DestinationAccountId, decimal Amount);