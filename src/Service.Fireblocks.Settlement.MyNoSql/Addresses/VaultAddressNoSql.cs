using MyNoSqlServer.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Fireblocks.Settlement.MyNoSql.Addresses
{
    //public class VaultAddressNoSql : MyNoSqlDbEntity
    //{
    //    public const string TableName = "myjetwallet-blockchain-wallets-vaultaddresses";
    //    public static string GeneratePartitionKey(string walletId) => $"{walletId}";
    //    public static string GenerateRowKey(string assetSymbol, string assetNetwork) =>
    //        $"{assetSymbol}_{assetNetwork}";

    //    public string WalletId { get; set; }

    //    public string AssetSymbol { get; set; }

    //    public string AssetNetwork { get; set; }

    //    public VaultAddress Address { get; set; }

    //    public static VaultAddressNoSql Create(string userId, string assetSymbol, string assetNetwork, VaultAddress vaultAddress)
    //    {
    //        return new VaultAddressNoSql()
    //        {
    //            PartitionKey = GeneratePartitionKey(userId),
    //            RowKey = GenerateRowKey(assetSymbol, assetNetwork),
    //            AssetSymbol = assetSymbol,
    //            AssetNetwork = assetNetwork,
    //            WalletId = userId,
    //            Address = vaultAddress
    //        };
    //    }

    //}
}
