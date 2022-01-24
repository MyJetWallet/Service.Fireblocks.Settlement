using JetBrains.Annotations;
using MyJetWallet.Sdk.Grpc;
using Service.Fireblocks.Settlement.Grpc;

namespace Service.Fireblocks.Settlement.Client
{
    [UsedImplicitly]
    public class FireblocksSettlementClientFactory: MyGrpcClientFactory
    {
        public FireblocksSettlementClientFactory(string grpcServiceUrl) : base(grpcServiceUrl)
        {
        }

        public ITransferService GetTransferServiceService() => CreateGrpcService<ITransferService>();
    }
}
