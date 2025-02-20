using MediatR;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Account;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.GenerateOtpVtuNation;

public sealed class GenerateOtpVtuNationCommand : IRequest<GenerateOtpVtuNationResponse>
{
    public GenerateOtpRequestVtuNation GenerateOtpRequestVtuNation { get; set; }
}
