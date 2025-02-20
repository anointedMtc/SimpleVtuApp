using MediatR;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Earnings;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Earnings.Commands.TransferBonusToMainWalletVtuNation;

public sealed class TransferBonusToMainWalletVtuNationCommand : IRequest<TransferBonusToMainWalletVtuNationResponse>
{
    public TransferBonusToMainWalletRequestVtuNation TransferBonusToMainWalletRequestVtuNation { get; set; }
}
