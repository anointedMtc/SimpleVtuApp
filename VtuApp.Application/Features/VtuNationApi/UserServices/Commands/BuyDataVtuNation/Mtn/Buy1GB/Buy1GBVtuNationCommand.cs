using MediatR;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation.Mtn.Buy1GB;

public sealed class Buy1GBVtuNationCommand : IRequest<Buy1GBVtuNationResponse> 
{
    public string PhoneNumber { get; set; }
}
