using Service.Fireblocks.Settlement.Grpc.Models.Common;
using System.Runtime.Serialization;

namespace Service.Fireblocks.Settlement.Grpc.Models.VaultAccounts
{
    [DataContract]
    public class CreateManagedVaultAccountResponse
    {
        [DataMember(Order = 1)]
        public ErrorResponse Error { get; set; }

        [DataMember(Order = 2)]
        public string VaultAccountId { get; set; }

        [DataMember(Order = 3)]
        public string VaultAccountName { get; set; }

    }
}