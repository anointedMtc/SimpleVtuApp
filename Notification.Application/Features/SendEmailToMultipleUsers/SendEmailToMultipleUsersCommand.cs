using MediatR;
using Notification.Shared.DTO;

namespace Notification.Application.Features.SendEmailToMultipleUsers;

public class SendEmailToMultipleUsersCommand : IRequest<SendEmailToMultipleUsersResponse>
{
    public List<EmailDto> ListOfEmailDtos { get; set; }
}
