using SharedKernel.Application.DTO;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Complaint;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Complaint.Commands.AddComplaintVtuNation;

public sealed class AddComplaintVtuNationResponse : ApiBaseResponse
{
    public AddComplaintResponseVtuNation? AddComplaintResponseVtuNation { get; set; }
}
