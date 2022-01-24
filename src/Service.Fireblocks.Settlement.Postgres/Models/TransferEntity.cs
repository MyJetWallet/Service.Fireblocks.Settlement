using Service.Fireblocks.Settlement.Domain.Models.Transfers;
using System;

namespace Service.Fireblocks.Settlement.Postgres.Models
{
    public class TransferEntity
    {
        public long Id { get; set; }
        public string DestinationVaultAccountId { get; set; }

        public string AsssetSymbol { get; set; }

        public string AsssetNetwork { get; set; }

        public string FireblocksAssetId { get; set; }

        public int AccountsInTransfers { get; set; }

        public decimal Threshold { get; set; }

        public TransferType Type { get; set; }

        public TransferStatus Status { get; set; }

        public DateTimeOffset StartedAt { get; set; }

        public DateTimeOffset? CompletedAt { get; set; }
    }
}