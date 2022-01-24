using System.ServiceModel;
using System.Threading.Tasks;
using Service.Fireblocks.Settlement.Grpc.Models.VaultAssets;

namespace Service.Fireblocks.Settlement.Grpc
{
    [ServiceContract]
    public interface ITransferService
    {
        [OperationContract]
        Task<CreateTransferResponse> CreateTransferToBrokerAsync(CreateTransferRequest request);
    }
}