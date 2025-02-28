using MediatR;

namespace VtuApp.Application.Features.Commands.TransferVtuBonusToMainWallet;

public sealed class TransferVtuBonusToMainWalletCommand : IRequest<TransferVtuBonusToMainWalletResponse>
{
    public decimal AmountToTransfer { get; set; }
}
