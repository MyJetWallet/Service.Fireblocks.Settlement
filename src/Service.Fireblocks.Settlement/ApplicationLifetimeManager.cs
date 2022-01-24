using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.NoSql;
using MyJetWallet.Sdk.Service;
using MyJetWallet.Sdk.ServiceBus;

namespace Service.Fireblocks.Settlement
{
    public class ApplicationLifetimeManager : ApplicationLifetimeManagerBase
    {
        private readonly ILogger<ApplicationLifetimeManager> _logger;
        private readonly MyNoSqlClientLifeTime _myNoSqlClient;
        private readonly ServiceBusLifeTime _myServiceBusTcpClient;

        public ApplicationLifetimeManager(
            IHostApplicationLifetime appLifetime, 
            ILogger<ApplicationLifetimeManager> logger,
            MyNoSqlClientLifeTime myNoSqlClient,
            ServiceBusLifeTime myServiceBusTcpClient)
            : base(appLifetime)
        {
            _logger = logger;
            _myNoSqlClient = myNoSqlClient;
            _myServiceBusTcpClient = myServiceBusTcpClient;
        }

        protected override void OnStarted()
        {
            _logger.LogInformation("OnStarted has been called.");
            _myNoSqlClient.Start();
            _myServiceBusTcpClient.Start();
        }

        protected override void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called.");
            _myNoSqlClient.Stop();
            _myServiceBusTcpClient.Stop();
        }

        protected override void OnStopped()
        {
            _logger.LogInformation("OnStopped has been called.");
        }
    }
}
