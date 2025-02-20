using MediatR;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation.Mtn.Buy10GB;

public sealed class Buy10GBVtuNationCommand : IRequest<Buy10GBVtuNationResponse>
{
    public string PhoneNumber { get; set; }
}
