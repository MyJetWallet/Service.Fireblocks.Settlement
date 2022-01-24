using Service.Fireblocks.Settlement.Grpc.Models.Common;
using System.Runtime.Serialization;

namespace Service.Fireblocks.Settlement.Grpc.Models.VaultAssets
{
    [DataContract]
    public class CreateTransferResponse
    {
        [DataMember(Order = 1)]
        public ErrorResponse Error { get; set; }

    }
}