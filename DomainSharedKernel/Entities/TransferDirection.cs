using System.Text.Json.Serialization;

namespace SharedKernel.Domain.Entities;


[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransferDirection
{
    In,
    Out
}



//public partial class Transfer
//{
//    [JsonConverter(typeof(JsonStringEnumConverter))]
//    public enum TransferDirection
//    {
//        In,
//        Out
//    }
//}
