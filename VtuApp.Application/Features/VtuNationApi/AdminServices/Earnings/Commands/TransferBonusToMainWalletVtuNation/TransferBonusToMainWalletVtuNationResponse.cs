using ApplicationSharedKernel.DTO;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Earnings;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Earnings.Commands.TransferBonusToMainWalletVtuNation;

public sealed class TransferBonusToMainWalletVtuNationResponse : ApiBaseResponse
{
    public TransferBonusToMainWalletResponseVtuNation? TransferBonusToMainWalletResponseVtuNation { get; set; }
}
