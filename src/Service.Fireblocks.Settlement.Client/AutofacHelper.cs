using Autofac;
using MyJetWallet.Sdk.NoSql;
using MyNoSqlServer.DataReader;
using Service.Fireblocks.Settlement.Grpc;
using Service.Fireblocks.Settlement.MyNoSql.Addresses;

// ReSharper disable UnusedMember.Global

namespace Service.Fireblocks.Settlement.Client
{
    public static class AutofacHelper
    {
        public static void RegisterFireblocksSettlementClient(this ContainerBuilder builder, string grpcServiceUrl, MyNoSqlTcpClient myNoSqlTcpClient)
        {
            var factory = new FireblocksSettlementClientFactory(grpcServiceUrl);

            builder.RegisterInstance<ITransferService>(factory.GetTransferServiceService());
            builder.RegisterInstance<IManagedVaultAccountService>(factory.GetManagedVaultAccountServiceService());
            builder.RegisterMyNoSqlReader<VaultAccountNoSql>(myNoSqlTcpClient, VaultAccountNoSql.TableName);
        }
    }
}
