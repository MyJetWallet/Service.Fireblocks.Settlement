using System;
using System.Runtime.Serialization;

namespace Service.Fireblocks.Settlement.Domain.Models.Transfers
{
    [DataContract]
    public class Transfer
    {
        [DataMember(Order = 1)]
        public long Id { get; set; }

        [DataMember(Order = 2)]
        public string DestinationVaultAccountId { get; set; }


        [DataMember(Order = 3)]
        public string AsssetSymbol { get; set; }


        [DataMember(Order = 4)]
        public string AsssetNetwork { get; set; }

        [DataMember(Order = 5)]
        public string FireblocksAssetId { get; set; }

        [DataMember(Order = 6)]
        public int AccountsInTransfers { get; set; }

        [DataMember(Order = 7)]
        public decimal Threshold { get; set; }

        [DataMember(Order = 8)]
        public TransferType Type { get; set; }

        [DataMember(Order = 9)]
        public TransferStatus Status { get; set; }

        [DataMember(Order = 10)]
        public DateTime StartedAt { get; set; }

        [DataMember(Order = 11)]
        public DateTime? CompletedAt { get; set; }

        [DataMember(Order = 12)]
        public decimal TotalBalance { get; set; }

        [DataMember(Order = 13)]
        public string UserId { get; set; }
    }
}
