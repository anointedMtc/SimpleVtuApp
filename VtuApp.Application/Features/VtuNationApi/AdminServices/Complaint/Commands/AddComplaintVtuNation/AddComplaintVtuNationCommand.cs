using MediatR;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Complaint;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Complaint.Commands.AddComplaintVtuNation;

public sealed class AddComplaintVtuNationCommand : IRequest<AddComplaintVtuNationResponse>
{
    public AddComplaintRequestVtuNation AddComplaintRequestVtuNation { get; set; }
}
