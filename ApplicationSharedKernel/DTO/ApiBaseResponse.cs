namespace SharedKernel.Application.DTO;

public class ApiBaseResponse
{
    public ApiBaseResponse()
    {
        Success = true;
    }
    public ApiBaseResponse(string message)
    {
        Success = true;
        Message = message;
    }
    public ApiBaseResponse(string message, bool success)
    {
        Success = success;
        Message = message;
    }

    public bool Success { get; set; }
    public string Message { get; set; }
    public List<string>? ValidationErrors { get; set; }



}
