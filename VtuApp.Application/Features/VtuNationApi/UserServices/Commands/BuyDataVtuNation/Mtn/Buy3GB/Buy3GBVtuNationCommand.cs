using MediatR;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation.Mtn.Buy3GB;

public sealed class Buy3GBVtuNationCommand : IRequest<Buy3GBVtuNationResponse> 
{
    public string PhoneNumber { get; set; }

}
