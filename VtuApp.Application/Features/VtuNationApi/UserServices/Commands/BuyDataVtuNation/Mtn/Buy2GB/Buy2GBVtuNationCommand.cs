using MediatR;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation.Mtn.Buy2GB;

public sealed class Buy2GBVtuNationCommand : IRequest<Buy2GBVtuNationResponse>
{
    public string PhoneNumber { get; set; }
}
