using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notification.Application.Features.SendEmailToMultipleUsers;
using Notification.Application.Features.SendEmailToSingleUser;
using Notification.Application.Features.SendEmailUsingLiquidSyntax;
using Notification.Application.Features.SendEmailWithAttachment;
using Notification.Application.Features.SendEmailWithRazorTemplate;
using SharedKernel.Api.Controllers;

namespace Notification.Api.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
public class Admin_EmailController : ApiBaseController
{

    [HttpPost("send-single-email")]
    public async Task<ActionResult<SendEmailToSingleUserResponse>> SendEmailToSingleUser([FromBody] SendEmailToSingleUserCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }


    [HttpPost("send-email-with-razor-template")]
    public async Task<ActionResult<SendEmailWithRazorTemplateResponse>> SendEmailWithRazorTemplatedFile([FromBody] SendEmailWithRazorTemplateCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }


    [HttpPost("send-email-with-liquid-syntax")]
    public async Task<ActionResult<SendEmailUsingLiquidSyntaxResponse>> SendEmailUsingLiquidFormat([FromBody] SendEmailUsingLiquidSyntaxCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }


    [HttpPost("send-email-with-attachment")]
    public async Task<ActionResult<SendEmailWithAttachmentResponse>> SendEmailWithAttachment([FromBody] SendEmailWithAttachmentCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }


    [HttpPost("send-email-to-multiple-users")]
    public async Task<ActionResult<SendEmailToMultipleUsersResponse>> EmailMultipleUsers([FromBody] SendEmailToMultipleUsersCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }

}
