using Service.Fireblocks.Settlement.Grpc.Models.Common;
using System.Runtime.Serialization;

namespace Service.Fireblocks.Settlement.Grpc.Models.VaultAccounts
{
    [DataContract]
    public class DeleteManagedVaultAccountResponse
    {
        [DataMember(Order = 1)]
        public ErrorResponse Error { get; set; }
    }
}