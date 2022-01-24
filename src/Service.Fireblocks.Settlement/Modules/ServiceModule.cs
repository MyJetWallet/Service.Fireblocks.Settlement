using Autofac;
using MyJetWallet.Sdk.NoSql;
using Service.Blockchain.Wallets.MyNoSql.AssetsMappings;
using Service.Blockchain.Wallets.MyNoSql.Addresses;
using MyJetWallet.Sdk.ServiceBus;
using Service.Fireblocks.Settlement.ServiceBus;
using Service.Fireblocks.Settlement.ServiceBus.Transfers;
using Service.Fireblocks.Api.Client;
using Service.Fireblocks.Signer.Client;
using Service.Fireblocks.Webhook.Subscribers;

namespace Service.Fireblocks.Settlement.Modules
{
    public class ServiceModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var serviceBusClient = builder.RegisterMyServiceBusTcpClient(
                Program.ReloadedSettings(e => e.SpotServiceBusHostPort),
                Program.LogFactory);
            var myNoSqlClient = builder.CreateNoSqlClient(Program.ReloadedSettings(e => e.MyNoSqlReaderHostPort));

            builder.RegisterMyNoSqlReader<AssetMappingNoSql>(myNoSqlClient, AssetMappingNoSql.TableName);
            builder.RegisterMyNoSqlReader<VaultAssetNoSql>(myNoSqlClient, VaultAssetNoSql.TableName);

            builder.RegisterMyServiceBusPublisher<StartTransferMessage>(serviceBusClient, Topics.StartTransferInternalTopic, false);

            builder.RegisterMyServiceBusSubscriberSingle<StartTransferMessage>(serviceBusClient,
                Topics.StartTransferInternalTopic,
                "service-fireblocks-settlement", MyServiceBus.Abstractions.TopicQueueType.Permanent);

            builder.RegisterFireblocksApiClient(Program.Settings.FireblocksApiUrl);
            builder.RegisterFireblocksSignerClient(Program.Settings.FireblocksSignerGrpcServiceUrl);

            builder
                .RegisterType<FireblocksSettlementInternalSubscriber>()
                .SingleInstance()
                .AutoActivate();
        }
    }
}