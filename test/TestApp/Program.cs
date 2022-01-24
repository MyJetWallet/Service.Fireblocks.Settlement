using System;
using System.IO;
using System.Linq;
using System.Text.Unicode;
using System.Threading.Tasks;
using ProtoBuf.Grpc.Client;
using Service.Fireblocks.Settlement.Client;
using Service.Fireblocks.Settlement.Grpc.Models;

namespace TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            GrpcClientFactory.AllowUnencryptedHttp2 = true;

            Console.Write("Press enter to start");
            Console.ReadLine();


            var factory = new FireblocksSettlementClientFactory("http://localhost:5001");
            var client = factory.GetTransferServiceService();

            var transfer = await client.CreateTransferToBrokerAsync(new Service.Fireblocks.Settlement.Grpc.Models.VaultAssets.CreateTransferRequest
            {
                AsssetNetwork = "fireblocks-eth-test",
                AsssetSymbol = "ETH",
                DestinationVaultAccountId = "60",//"16",
                Threshold = 0.1m
            });

            //Console.WriteLine(resp?.VaultAccount?.FirstOrDefault().Id);

            Console.WriteLine("End");
            Console.ReadLine();
        }
    }
}
