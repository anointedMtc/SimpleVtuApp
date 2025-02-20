﻿using System.Text.Json.Serialization;

namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Auth;

public record RegisterResponseVtuNation
{
    public bool IsSuccessful { get; init; }
    public int ResponseCode { get; init; }
    public string ResponseMessage { get; init; }

    public UserVtuNation User { get; init; }


    [JsonPropertyName("token_type")]
    public string TokenType { get; init; }


    [JsonPropertyName("token_validity")]
    public int TokenValidity { get; init; }

    public string? Token { get; init; }
}
