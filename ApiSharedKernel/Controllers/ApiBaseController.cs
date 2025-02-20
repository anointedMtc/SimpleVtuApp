using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;


namespace SharedKernel.Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public abstract class ApiBaseController : ControllerBase
{
    private ISender? _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

}


// https://localhost:7023/api/v3/{ControllerName}/{ActionName}
