using System.Runtime.Serialization;

namespace Service.Fireblocks.Settlement.Grpc.Models.VaultAccounts
{
    [DataContract]
    public class CreateManagedVaultAccountRequest
    {
        [DataMember(Order = 1)]
        public string VaultAccountId { get; set; }
    }
}