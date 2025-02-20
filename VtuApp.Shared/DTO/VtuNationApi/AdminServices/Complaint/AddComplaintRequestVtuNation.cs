using System.Text.Json.Serialization;

namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Complaint;

public record AddComplaintRequestVtuNation
{
    public string Subject { get; init; }
    public string Message { get; init; }

    // example = "complaint_category": "Technical"
    [JsonPropertyName("complaint_category")]
    public string ComplaintCategory { get; init; }

    // example = "channel": "Mobile"
    public string Channel {  get; init; }
}
