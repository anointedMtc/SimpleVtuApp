
// UserServicesVtuNationController
global using Microsoft.AspNetCore.Mvc;
global using SharedKernel.Api.Controllers;
global using VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyAirtimeVtuNation;
global using VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation;
global using VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation.Mtn.Buy10GB;
global using VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation.Mtn.Buy1GB;
global using VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation.Mtn.Buy2GB;
global using VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation.Mtn.Buy3GB;
global using VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation.Mtn.Buy500MB;
global using VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation.Mtn.Buy5GB;
global using VtuApp.Application.Features.VtuNationApi.UserServices.Queries.DataPrices.Get9MobileDataPrices;
global using VtuApp.Application.Features.VtuNationApi.UserServices.Queries.DataPrices.GetAirtelDataPrices;
global using VtuApp.Application.Features.VtuNationApi.UserServices.Queries.DataPrices.GetGloDataPrices;
global using VtuApp.Application.Features.VtuNationApi.UserServices.Queries.DataPrices.GetMtnDataPrices;
global using VtuApp.Application.Features.VtuNationApi.UserServices.Queries.GetAvailableAirtimeNetworks;
global using VtuApp.Application.Features.VtuNationApi.UserServices.Queries.GetAvailableDataNetworks;
global using VtuApp.Shared.DTO.VtuNationApi.UserServices;
global using Microsoft.AspNetCore.Authorization;
global using Asp.Versioning;



// AdminServicesVtuNationController
global using VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.ConfirmEmailVtuNation;
global using VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.GenerateOtpVtuNation;
global using VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.SendEmailVerificationLinkVtuNation;
global using VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.SetUpdateTransactionPassVtuNation;
global using VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.SubmitBvnVtuNation;
global using VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.UpdatePasswordVtuNation;
global using VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.UpdateProfileVtuNation;
global using VtuApp.Application.Features.VtuNationApi.AdminServices.Account.Commands.ValidateOtpVtuNation;
global using VtuApp.Application.Features.VtuNationApi.AdminServices.Auth.Commands.ForgotPasswordVtuNation;
global using VtuApp.Application.Features.VtuNationApi.AdminServices.Auth.Commands.LogOutVtuNation;
global using VtuApp.Application.Features.VtuNationApi.AdminServices.Auth.Commands.RefreshTokenVtuNation;
global using VtuApp.Application.Features.VtuNationApi.AdminServices.Auth.Commands.RegisterWithVtuNationApi;
global using VtuApp.Application.Features.VtuNationApi.AdminServices.Auth.Queries.GetProfileVtuNation;
global using VtuApp.Application.Features.VtuNationApi.AdminServices.Complaint.Commands.AddComplaintVtuNation;
global using VtuApp.Application.Features.VtuNationApi.AdminServices.Earnings.Commands.TransferBonusToMainWalletVtuNation;
global using VtuApp.Application.Features.VtuNationApi.AdminServices.Earnings.Queries.GetEarningsHistoryVtuNation;
global using VtuApp.Application.Features.VtuNationApi.AdminServices.Funding.Commands.SubmitPaymentNotificationVtuNation;
global using VtuApp.Application.Features.VtuNationApi.AdminServices.Funding.Queries.GetFundNotificationsVtuNation;
global using VtuApp.Application.Features.VtuNationApi.AdminServices.Funding.Queries.GetPaymentAccountInfoVtuNation;
global using VtuApp.Application.Features.VtuNationApi.AdminServices.Transactions.Queries.GetSingleTransactionVtuNation;
global using VtuApp.Application.Features.VtuNationApi.AdminServices.Transactions.Queries.GetTransactionHistoryVtuNation;
global using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Account;
global using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Auth;
global using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Complaint;
global using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Earnings;
global using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Funding;