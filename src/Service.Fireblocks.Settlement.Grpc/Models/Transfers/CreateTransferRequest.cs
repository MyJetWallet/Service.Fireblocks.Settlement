using System.Runtime.Serialization;

namespace Service.Fireblocks.Settlement.Grpc.Models.VaultAssets
{
    [DataContract]
    public class CreateTransferRequest
    {
        [DataMember(Order = 1)]
        public string DestinationVaultAccountId { get; set; }

        [DataMember(Order = 2)]
        public string AsssetSymbol { get; set; }

        [DataMember(Order = 3)]
        public string AsssetNetwork { get; set; }

        [DataMember(Order = 4)]
        public decimal Threshold { get; set; }
    }
}