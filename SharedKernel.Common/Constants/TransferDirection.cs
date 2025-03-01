using System.Text.Json.Serialization;

namespace SharedKernel.Common.Constants;


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
