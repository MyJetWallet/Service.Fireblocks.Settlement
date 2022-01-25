using MyNoSqlServer.Abstractions;

namespace Service.Fireblocks.Settlement.MyNoSql.Addresses
{
    public class VaultAccountNoSql : MyNoSqlDbEntity
    {
        public const string TableName = "myjetwallet-blockchain-wallets-vaultaccount";
        public static string GeneratePartitionKey(string vaultAccountId) => $"{vaultAccountId}";
        public static string GenerateRowKey(string vaultAccountId) => $"{vaultAccountId}";
        public string VaultAccountId { get; set; }
        public string VaultAccountName { get; set; }

        public static VaultAccountNoSql Create(string vaultAccountId, string vaultAccountName)
        {
            return new VaultAccountNoSql()
            {
                PartitionKey = GeneratePartitionKey(vaultAccountId),
                RowKey = GenerateRowKey(vaultAccountId),
                VaultAccountId = vaultAccountId,
                VaultAccountName = vaultAccountName,
            };
        }
    }
}
