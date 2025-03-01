using SharedKernel.Application.DTO;
using VtuApp.Shared.DTO;

namespace VtuApp.Application.Features.Queries.GetAllVtuCustomers;

public sealed class GetAllVtuCustomersResponse : ApiBaseResponse
{
    public List<CustomerShortResponseDto>? CustomerShortResponseDto { get; set; }
}
