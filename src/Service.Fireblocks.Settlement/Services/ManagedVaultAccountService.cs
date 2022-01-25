using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.Service;
using MyNoSqlServer.Abstractions;
using Service.Fireblocks.Api.Grpc;
using Service.Fireblocks.Settlement.Grpc;
using Service.Fireblocks.Settlement.Grpc.Models.VaultAccounts;
using Service.Fireblocks.Settlement.MyNoSql.Addresses;

namespace Service.Fireblocks.Settlement.Services
{
    public class ManagedVaultAccountService : IManagedVaultAccountService
    {
        private readonly ILogger<TransferService> _logger;
        private readonly IMyNoSqlServerDataWriter<VaultAccountNoSql> _vaultAccountNoSql;
        private readonly IVaultAccountService _vaultAccountService;

        public ManagedVaultAccountService(ILogger<TransferService> logger,
            IMyNoSqlServerDataWriter<VaultAccountNoSql> vaultAccountNoSql,
            IVaultAccountService vaultAccountService)
        {
            _logger = logger;
            _vaultAccountNoSql = vaultAccountNoSql;
            _vaultAccountService = vaultAccountService;
        }

        public async Task<CreateManagedVaultAccountResponse> CreateManagedVaultAccountAsync(CreateManagedVaultAccountRequest request)
        {
            try
            {
                var existing = await _vaultAccountService.GetVaultAccountAsync(new Api.Grpc.Models.VaultAccounts.GetVaultAccountRequest()
                {
                    VaultAccountId = request.VaultAccountId,
                });

                if (existing == null || existing.VaultAccount == null || !existing.VaultAccount.Any())
                    return new CreateManagedVaultAccountResponse
                    {
                        Error = new Grpc.Models.Common.ErrorResponse
                        {
                            Message = "Vault account does not exist",
                            ErrorCode = Grpc.Models.Common.ErrorCode.DoesNotExist
                        }
                    };

                var first = existing.VaultAccount.First();

                await _vaultAccountNoSql.InsertOrReplaceAsync(VaultAccountNoSql.Create(first.Id, first.Name));

                return new CreateManagedVaultAccountResponse 
                {
                    VaultAccountId = first.Id,
                    VaultAccountName = first.Name,
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating VaultAccount {context}", request.ToJson());

                return new CreateManagedVaultAccountResponse
                {
                    Error = new Grpc.Models.Common.ErrorResponse
                    {
                        ErrorCode = Grpc.Models.Common.ErrorCode.Unknown,
                        Message = e.Message
                    }
                };
            }
        }

        public async Task<DeleteManagedVaultAccountResponse> DeleteManagedVaultAccountAsync(DeleteManagedVaultAccountRequest request)
        {
            try
            {
                await _vaultAccountNoSql.DeleteAsync(
                    VaultAccountNoSql.GeneratePartitionKey(request.VaultAccountId),
                    VaultAccountNoSql.GenerateRowKey(request.VaultAccountId));

                return new DeleteManagedVaultAccountResponse
                {
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting VaultAccount {context}", request.ToJson());

                return new DeleteManagedVaultAccountResponse
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
