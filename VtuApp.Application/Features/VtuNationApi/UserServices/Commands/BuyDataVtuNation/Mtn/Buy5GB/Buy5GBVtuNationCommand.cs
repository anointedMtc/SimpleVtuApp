using MediatR;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation.Mtn.Buy5GB;

public sealed class Buy5GBVtuNationCommand : IRequest<Buy5GBVtuNationResponse>
{
    public string PhoneNumber { get; set; }

}
