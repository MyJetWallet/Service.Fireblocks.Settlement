using System.ServiceModel;
using System.Threading.Tasks;
using Service.Fireblocks.Settlement.Grpc.Models.VaultAccounts;

namespace Service.Fireblocks.Settlement.Grpc
{
    [ServiceContract]
    public interface IManagedVaultAccountService
    {
        [OperationContract]
        Task<CreateManagedVaultAccountResponse> CreateManagedVaultAccountAsync(CreateManagedVaultAccountRequest request);

        [OperationContract]
        Task<DeleteManagedVaultAccountResponse> DeleteManagedVaultAccountAsync(DeleteManagedVaultAccountRequest request);
    }
}