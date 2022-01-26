using Service.Fireblocks.Settlement.Domain.Models.Transfers;
using Service.Fireblocks.Settlement.Grpc.Models.Common;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Service.Fireblocks.Settlement.Grpc.Models.VaultAssets
{
    [DataContract]
    public class GetTransfersResponse
    {
        [DataMember(Order = 1)] 
        public ErrorResponse Error { get; set; }
        
        [DataMember(Order = 2)] 
        public long IdForNextQuery { get; set; }
        
        [DataMember(Order = 3)] 
        public IReadOnlyCollection<Transfer> Transfers { get; set; }
    }
}