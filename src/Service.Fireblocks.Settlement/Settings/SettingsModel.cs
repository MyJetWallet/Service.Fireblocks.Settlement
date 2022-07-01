using MyJetWallet.Sdk.Service;
using MyYamlParser;

namespace Service.Fireblocks.Settlement.Settings
{
    public class SettingsModel
    {
        [YamlProperty("FireblocksSettlement.SeqServiceUrl")]
        public string SeqServiceUrl { get; set; }

        [YamlProperty("FireblocksSettlement.ZipkinUrl")]
        public string ZipkinUrl { get; set; }

        [YamlProperty("FireblocksSettlement.ElkLogs")]
        public LogElkSettings ElkLogs { get; set; }

        [YamlProperty("FireblocksSettlement.MyNoSqlWriterUrl")]
        public string MyNoSqlWriterUrl { get; internal set; }

        [YamlProperty("FireblocksSettlement.MyNoSqlReaderHostPort")]
        public string MyNoSqlReaderHostPort { get; internal set; }

        [YamlProperty("FireblocksSettlement.SpotServiceBusHostPort")]
        public string SpotServiceBusHostPort { get; internal set; }

        [YamlProperty("FireblocksSettlement.FireblocksApiUrl")]
        public string FireblocksApiUrl { get; internal set; }

        [YamlProperty("FireblocksSettlement.FireblocksSignerGrpcServiceUrl")]
        public string FireblocksSignerGrpcServiceUrl { get; internal set; }

        [YamlProperty("FireblocksSettlement.PostgresConnectionString")]
        public string PostgresConnectionString { get; internal set; }

        [YamlProperty("FireblocksSettlement.GasStationNetworks")]
        public string GasStationNetworks { get; internal set; }
    }
}
