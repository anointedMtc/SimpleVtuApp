using AutoMapper;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using Notification.Application.Features.SendEmailToSingleUser;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Shared.DTO;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using SharedKernel.Domain.Interfaces;

namespace Notification.Application.Features.SendEmailToMultipleUsers;

public class SendEmailToMultipleUsersCommandHandler : IRequestHandler<SendEmailToMultipleUsersCommand, SendEmailToMultipleUsersResponse>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<SendEmailToMultipleUsersCommandHandler> _logger;
    private readonly IRepository<EmailEntity> _emailRepository;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public SendEmailToMultipleUsersCommandHandler(IEmailService emailService,
        ILogger<SendEmailToMultipleUsersCommandHandler> logger,
        IRepository<EmailEntity> emailRepository, IMapper mapper,
        IUserContext userContext, IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _emailService = emailService;
        _logger = logger;
        _emailRepository = emailRepository;
        _mapper = mapper;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }
    public async Task<SendEmailToMultipleUsersResponse> Handle(SendEmailToMultipleUsersCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(SendEmailToSingleUserCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var sendEmailToMultipleUsersResponse = new SendEmailToMultipleUsersResponse();

        // you can just send it straight up like this... or ???
        //await _emailService.SendMultiple(request.ListOfEmailDtos);


        List<EmailDto> emailMetadataList = new();

        foreach (var emailDto in request.ListOfEmailDtos)
        {
            var emailMetadata = new EmailDto(
                emailDto.ToAddress,
                emailDto.Subject,
                emailDto.Body,
                emailDto.AttachmentPath,
                emailDto.CC,
                emailDto.BCC,

                //emailDto.Sender,
                userExecutingCommand!.Email,

                typeof(SendEmailToSingleUserCommand).Name
                //emailDto.EventType
            );

            emailMetadataList.Add(emailMetadata);

        }

        await _emailService.SendMultiple(emailMetadataList);


        foreach (var item in emailMetadataList)
        {
            //var emailToSave = _mapper.Map<EmailEntity>(item);

            var emailToSave = new EmailEntity(
                item.ToAddress,
                item.Subject,
                item.Body,
                item.AttachmentPath,
                item.CC,
                item.BCC,
                item.Sender,
                item.EventType);

            await _emailRepository.AddAsync(emailToSave);
        }

        sendEmailToMultipleUsersResponse.Success = true;
        sendEmailToMultipleUsersResponse.Message = $"Email Sent Successfully";

        return sendEmailToMultipleUsersResponse;

    }
}
