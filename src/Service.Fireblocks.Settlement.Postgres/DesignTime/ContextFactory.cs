using MyJetWallet.Sdk.Postgres;

namespace Service.Fireblocks.Settlement.Postgres.DesignTime
{
    public class ContextFactory : MyDesignTimeContextFactory<DatabaseContext>
    {
        public ContextFactory() : base(options => new DatabaseContext(options))
        {
        }
    }
}