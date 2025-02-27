using MediatR;

namespace VtuApp.Application.Features.Commands.DeleteVtuCustomer;

public sealed class DeleteVtuCustomerCommand : IRequest<DeleteVtuCustomerResponse>
{
    public string Email { get; set; }
}
