﻿namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Account;

public record UpdateProfileResponseVtuNation
{
    public bool IsSuccessful { get; init; }
    public string Message { get; init; }
    public string ResponseCode { get; init; }
}
