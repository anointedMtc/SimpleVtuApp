using MediatR;
using VtuApp.Shared.DTO.VtuNationApi.UserServices;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyAirtimeVtuNation;

public sealed class BuyAirtimeVtuNationCommand : IRequest<BuyAirtimeVtuNationResponse>
{
    public BuyAirtimeVtuNationCommand()
    {
        BuyAirtimeRequestVtuNation = new();
    }
    public BuyAirtimeRequestVtuNation BuyAirtimeRequestVtuNation { get; set; }
}
