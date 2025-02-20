using System.Text.Json.Serialization;

namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Auth;

public record RefreshTokenResponseVtuNation
{
    public string? Token { get; init; }


    [JsonPropertyName("token_type")]
    public string TokenType { get; init; }


    [JsonPropertyName("token_validity")]
    public int TokenValidity { get; init; }

}
