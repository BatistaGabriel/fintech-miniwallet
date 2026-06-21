using Fintech.MiniWallet.Api.Requests;
using Fintech.MiniWallet.Application.Accounts.Commands.Deposit;
using Fintech.MiniWallet.Application.Accounts.Commands.Transfer;
using Fintech.MiniWallet.Application.Accounts.Commands.Withdraw;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintech.MiniWallet.Api.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;
    public AccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{accountId:guid}/deposit")]
    public async Task<IActionResult> Deposit([FromRoute] Guid accountId, [FromBody] DepositRequest depositRequest)
    {
        DepositCommand deposit = new DepositCommand(accountId, depositRequest.Amount);

        await _mediator.Send(deposit);

        return NoContent();
    }

    [HttpPost("{accountId:guid}/withdraw")]
    public async Task<IActionResult> Withdraw([FromRoute] Guid accountId, [FromBody] WithdrawRequest withdrawRequest)
    {
        WithdrawCommand withdraw = new WithdrawCommand(accountId, withdrawRequest.Amount);

        await _mediator.Send(withdraw);

        return NoContent();
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer([FromBody] TransferRequest transferRequest)
    {
        TransferCommand transfer = new TransferCommand(transferRequest.OriginAccountId, transferRequest.DestinationAccountId, transferRequest.Amount);

        await _mediator.Send(transfer);

        return NoContent();
    }
}