using DotNetCoreDecorators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.Service;
using MyNoSqlServer.Abstractions;
using Service.Blockchain.Wallets.MyNoSql.Addresses;
using Service.Blockchain.Wallets.MyNoSql.AssetsMappings;
using Service.Fireblocks.Api.Grpc;
using Service.Fireblocks.Settlement.Postgres;
using Service.Fireblocks.Settlement.Postgres.Models;
using Service.Fireblocks.Settlement.ServiceBus.Transfers;
using Service.Fireblocks.Signer.Grpc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Fireblocks.Webhook.Subscribers
{
    public class FireblocksSettlementInternalSubscriber
    {
        private readonly ILogger<FireblocksSettlementInternalSubscriber> _logger;
        private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;
        private readonly IVaultAccountService _vaultAccountService;
        private readonly ITransactionService _transactionService;
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1);

        public FireblocksSettlementInternalSubscriber(
            ISubscriber<StartTransferMessage> subscriber,
            ILogger<FireblocksSettlementInternalSubscriber> logger,
            DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder,
            IVaultAccountService vaultAccountService,
            ITransactionService transactionService)
        {
            subscriber.Subscribe(HandleMessagel);
            _logger = logger;
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
            _vaultAccountService = vaultAccountService;
            _transactionService = transactionService;
        }

        private async ValueTask HandleMessagel(StartTransferMessage message)
        {
            using var activity = MyTelemetry.StartActivity("Handle StartTransferMessage Command");
            var logContext = message.ToJson();
            _logger.LogInformation("Processing StartTransferMessage: {@context}", logContext);

            try
            {
                await _semaphoreSlim.WaitAsync();
                await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);
                var transfer = await context.Transfers.FirstOrDefaultAsync(x =>
                             x.AsssetSymbol == message.AsssetSymbol &&
                             x.AsssetNetwork == message.AsssetNetwork &&
                             x.Status == Settlement.Domain.Models.Transfers.TransferStatus.Started);

                if (transfer == null)
                {
                    transfer = new TransferEntity()
                    {
                        AsssetNetwork = message.AsssetNetwork,
                        AsssetSymbol = message.AsssetSymbol,
                        AccountsInTransfers = 0,
                        TotalBalance = 0m,
                        StartedAt = DateTime.UtcNow,
                        CompletedAt = null,
                        DestinationVaultAccountId = message.DestinationVaultAccountId,
                        Threshold = message.Threshold,
                        Status = Settlement.Domain.Models.Transfers.TransferStatus.Started,
                        Type = Settlement.Domain.Models.Transfers.TransferType.FromIntermediateToBroker,
                        FireblocksAssetId = message.FireblocksAssetId,
                        UserId = message.UserId,
                    };

                    await context.Transfers.AddAsync(transfer);
                    await context.SaveChangesAsync();
                }

                if (transfer.Status == Settlement.Domain.Models.Transfers.TransferStatus.Completed)
                    return;

                var streamBalances = _vaultAccountService.GetBalancesForAssetAsync(new()
                {
                    BatchSize = 100,
                    FireblocksAssetId = transfer.FireblocksAssetId,
                    NamePrefix = "client_",
                    Threshold = transfer.Threshold,
                });

                await foreach (var balances in streamBalances)
                {
                    _logger.LogInformation("Processing stream StartTransferMessage: {@context}", new
                    {
                        Balances = balances.ToJson(),
                        Message = logContext,
                    });

                    if (balances.Error != null)
                    {
                        if (balances.Error.ErrorCode == Api.Grpc.Models.Common.ErrorCode.DoesNotExist)
                            break;
                        else
                            throw new Exception(balances.Error.Message);
                    }

                    var balance = 0m;
                    foreach (var vaultAccount in balances.VaultAccounts)
                    {
                        var vaultAsset = vaultAccount.VaultAssets.FirstOrDefault();
                        balance += vaultAsset.Available;
                        var transaction = await _transactionService.CreateTransactionAsync(new()
                        {
                            Amount = vaultAsset.Available,
                            AssetNetwork = transfer.AsssetNetwork,
                            AssetSymbol = transfer.AsssetSymbol,
                            TreatAsGrossAmount = true,
                            ExternalTransactionId = $"settl_{transfer.Id}_{vaultAccount.Id}_{message.DestinationVaultAccountId}",
                            DestinationVaultAccountId = transfer.DestinationVaultAccountId,
                            FromVaultAccountId = vaultAccount.Id,
                        });

                        if (transaction.Error != null)
                        {
                            _logger.LogError("Error creating transaction StartTransferMessage: {@context}", new
                            {
                                Error = transaction.ToJson(),
                                Message = logContext,
                            });
                            throw new Exception(transaction.Error.Message);
                        }
                    }

                    transfer.TotalBalance += balance;
                    transfer.AccountsInTransfers += balances.VaultAccounts.Count;
                    context.Transfers.Update(transfer);
                    await context.SaveChangesAsync();
                }

                _logger.LogInformation("Saving result StartTransferMessage: {@context}", logContext);
                transfer.Status = Settlement.Domain.Models.Transfers.TransferStatus.Completed;
                transfer.CompletedAt = DateTime.UtcNow;
                context.Transfers.Update(transfer);
                await context.SaveChangesAsync();

                _logger.LogInformation("Completed StartTransferMessage: {@context}", logContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing StartTransferMessage {@context}", logContext);
                ex.FailActivity();
                throw;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
    }
}
