using System.Runtime.Serialization;

namespace Service.Fireblocks.Settlement.Grpc.Models.VaultAssets
{
    [DataContract]
    public class GetTransfersRequest
    {
        [DataMember(Order = 1)] 
        public long LastId { get; set; }
        
        [DataMember(Order = 2)] 
        public int BatchSize { get; set; }
    }
}