﻿namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Account;

public record ValidateOtpResponseVtuNation
{
    public bool IsSuccessful { get; init; }
    public string Message { get; init; }
    public int ResponseCode { get; init; }
}
