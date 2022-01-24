using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Service.Fireblocks.Settlement.ServiceBus.Transfers
{
    [DataContract]
    public class StartTransferMessage
    {
        [DataMember(Order = 1)]
        public string DestinationVaultAccountId { get; set; }

        [DataMember(Order = 2)]
        public string AsssetSymbol { get; set; }

        [DataMember(Order = 3)]
        public string AsssetNetwork { get; set; }

        [DataMember(Order = 4)]
        public decimal Threshold { get; set; }

        [DataMember(Order = 5)]
        public string FireblocksAssetId { get; set; }
    }
}
