using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.Service;
using MyJetWallet.Sdk.ServiceBus;
using MyNoSqlServer.Abstractions;
using Service.Blockchain.Wallets.MyNoSql.AssetsMappings;
using Service.Fireblocks.Settlement.Grpc;
using Service.Fireblocks.Settlement.Grpc.Models.VaultAssets;
using Service.Fireblocks.Settlement.Postgres;
using Service.Fireblocks.Settlement.ServiceBus.Transfers;

namespace Service.Fireblocks.Settlement.Services
{
    public class TransferService : ITransferService
    {
        private readonly ILogger<TransferService> _logger;
        private readonly IServiceBusPublisher<StartTransferMessage> _startTransferMessagePublisher;
        private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;
        private readonly IMyNoSqlServerDataReader<AssetMappingNoSql> _assetMappingNoSql;

        public TransferService(ILogger<TransferService> logger,
            IServiceBusPublisher<StartTransferMessage> startTransferMessagePublisher,
            DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder,
            IMyNoSqlServerDataReader<AssetMappingNoSql> assetMappingNoSql)
        {
            _logger = logger;
            _startTransferMessagePublisher = startTransferMessagePublisher;
            this._dbContextOptionsBuilder = dbContextOptionsBuilder;
            this._assetMappingNoSql = assetMappingNoSql;
        }

        public async Task<CreateTransferResponse> CreateTransferToBrokerAsync(CreateTransferRequest request)
        {
            try
            {
                var assetMapping = _assetMappingNoSql.Get(
                    AssetMappingNoSql.GeneratePartitionKey(request.AsssetSymbol), 
                    AssetMappingNoSql.GenerateRowKey(request.AsssetNetwork));

                if (assetMapping == null)
                    return new CreateTransferResponse
                    {
                        Error = new Grpc.Models.Common.ErrorResponse
                        {
                            Message = "Asset mapping does not exist",
                            ErrorCode = Grpc.Models.Common.ErrorCode.DoesNotExist
                        }
                    };

                if (assetMapping.AssetMapping.DepositType != MyJetWallet.Fireblocks.Domain.Models.AssetMappngs.DepositType.Intermediate)
                    return new CreateTransferResponse
                    {
                        Error = new Grpc.Models.Common.ErrorResponse
                        {
                            Message = $"Asset should be of deposit type {MyJetWallet.Fireblocks.Domain.Models.AssetMappngs.DepositType.Intermediate}",
                            ErrorCode = Grpc.Models.Common.ErrorCode.ApiError
                        }
                    };

                await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);

                var runningTransfer = await context.Transfers.FirstOrDefaultAsync(x =>
                                            x.FireblocksAssetId == assetMapping.AssetMapping.FireblocksAssetId &&
                                            x.Status == Domain.Models.Transfers.TransferStatus.Started);

                if (runningTransfer != null)
                    return new CreateTransferResponse
                    {
                        Error = new Grpc.Models.Common.ErrorResponse
                        {
                            Message = "Transfer is running",
                            ErrorCode = Grpc.Models.Common.ErrorCode.TransferIsRunning
                        }
                    };

                await _startTransferMessagePublisher.PublishAsync(new StartTransferMessage
                {
                    AsssetNetwork = request.AsssetNetwork,
                    AsssetSymbol = request.AsssetSymbol,
                    DestinationVaultAccountId = request.DestinationVaultAccountId,
                    Threshold = request.Threshold,
                    FireblocksAssetId = assetMapping.AssetMapping.FireblocksAssetId
                });

                return new CreateTransferResponse { };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating VaultAccount {context}", request.ToJson());

                return new CreateTransferResponse
                {
                    Error = new Grpc.Models.Common.ErrorResponse
                    {
                        ErrorCode = Grpc.Models.Common.ErrorCode.Unknown,
                        Message = e.Message
                    }
                };
            }
        }
    }
}
