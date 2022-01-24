using Autofac;
using Service.Fireblocks.Settlement.Grpc;

// ReSharper disable UnusedMember.Global

namespace Service.Fireblocks.Settlement.Client
{
    public static class AutofacHelper
    {
        public static void RegisterFireblocksSettlementClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new FireblocksSettlementClientFactory(grpcServiceUrl);
        }
    }
}
