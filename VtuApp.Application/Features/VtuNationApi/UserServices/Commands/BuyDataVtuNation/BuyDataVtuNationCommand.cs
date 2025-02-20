using MediatR;
using VtuApp.Shared.DTO.VtuNationApi.UserServices;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation;

public sealed class BuyDataVtuNationCommand : IRequest<BuyDataVtuNationResponse>
{
    public BuyDataVtuNationCommand()
    {
        BuyDataRequestVtuNation = new();
    }
    public BuyDataRequestVtuNation BuyDataRequestVtuNation { get; set; }
}
