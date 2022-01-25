using System.Runtime.Serialization;

namespace Service.Fireblocks.Settlement.Grpc.Models.VaultAccounts
{
    [DataContract]
    public class DeleteManagedVaultAccountRequest
    {
        [DataMember(Order = 1)]
        public string VaultAccountId { get; set; }
    }
}