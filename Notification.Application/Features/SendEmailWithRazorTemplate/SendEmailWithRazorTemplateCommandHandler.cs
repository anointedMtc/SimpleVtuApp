﻿using AutoMapper;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Domain.Interfaces;
using Notification.Shared.DTO;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using SharedKernel.Domain.Interfaces;

namespace Notification.Application.Features.SendEmailWithRazorTemplate;

public class SendEmailWithRazorTemplateCommandHandler : IRequestHandler<SendEmailWithRazorTemplateCommand, SendEmailWithRazorTemplateResponse>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<SendEmailWithRazorTemplateCommandHandler> _logger;
    private readonly IEmailRepository<EmailEntity> _emailRepository;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public SendEmailWithRazorTemplateCommandHandler(IEmailService emailService, 
        ILogger<SendEmailWithRazorTemplateCommandHandler> logger,
        IEmailRepository<EmailEntity> emailRepository, IMapper mapper, 
        IUserContext userContext, IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _emailService = emailService;
        _logger = logger;
        _emailRepository = emailRepository;
        _mapper = mapper;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<SendEmailWithRazorTemplateResponse> Handle(SendEmailWithRazorTemplateCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(SendEmailWithRazorTemplateCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var sendEmailWithRazorTemplateResponse = new SendEmailWithRazorTemplateResponse();

        var razorModelToUse = new UserEmailDto(
            request.RazorModel.Name,
            request.RazorModel.Email,
            request.RazorModel.MemberType);

        var emailMetadata = new EmailDto(
            request.EmailDto.ToAddress,
            request.EmailDto.Subject,

            // NOT REQUIRED
            //request.EmailDto.Body,
            //request.EmailDto.AttachmentPath,

            request.EmailDto.CC,
            request.EmailDto.BCC,

             userExecutingCommand!.Email,
             //request.EmailDto.Sender,

            typeof(SendEmailWithRazorTemplateCommand).Name
            //request.EmailDto.EventType
        );

        // the file path should be similar to this
        // var razorFilePath = $"{Directory.GetCurrentDirectory()}/EmailTemplates/MyTemplate.cshtml";      // this is the path of the template file itself not attachment

        await _emailService.SendUsingTemplate(emailMetadata, razorModelToUse, request.RazorFilePath);

        // for all these to work, we need to modify the project file by adding a
        // new property <PreserveCompilationContext>true</PreserveCompilationContext>


        //var emailToSave = _mapper.Map<EmailEntity>(request.EmailDto);

        var emailToSave = new EmailEntity(
           request.EmailDto.ToAddress,
           request.EmailDto.Subject,
           request.EmailDto.Body,
           request.EmailDto.AttachmentPath,
           request.EmailDto.CC,
           request.EmailDto.BCC,
           request.EmailDto.Sender,
           request.EmailDto.EventType);

        await _emailRepository.AddAsync(emailToSave);

        sendEmailWithRazorTemplateResponse.Success = true;
        sendEmailWithRazorTemplateResponse.Message = $"Email Sent Successfully";

        return sendEmailWithRazorTemplateResponse;

    }
}
