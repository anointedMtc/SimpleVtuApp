using MediatR;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation.Mtn.Buy500MB;

public sealed class Buy500MBVtuNationCommand : IRequest<Buy500MBVtuNationResponse>
{
    public string PhoneNumber { get; set; }
}
