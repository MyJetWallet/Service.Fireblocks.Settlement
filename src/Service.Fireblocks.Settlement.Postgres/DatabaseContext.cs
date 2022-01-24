using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyJetWallet.Sdk.Postgres;
using MyJetWallet.Sdk.Service;
using Service.Fireblocks.Settlement.Postgres.Models;

namespace Service.Fireblocks.Settlement.Postgres
{
    public class DatabaseContext : MyDbContext
    {
        public const string Schema = "fireblocks-settlements";

        private const string TransfersTableName = "transfers";

        private Activity _activity;

        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TransferEntity> Transfers { get; set; }

        public static DatabaseContext Create(DbContextOptionsBuilder<DatabaseContext> options)
        {
            var activity = MyTelemetry.StartActivity($"Database context {Schema}")?.AddTag("db-schema", Schema);

            var ctx = new DatabaseContext(options.Options) { _activity = activity };

            return ctx;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);

            SetWithdrawalEntry(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SetWithdrawalEntry(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransferEntity>().ToTable(TransfersTableName);
            modelBuilder.Entity<TransferEntity>().Property(e => e.Id).UseIdentityColumn();
            modelBuilder.Entity<TransferEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<TransferEntity>().HasIndex(e => new { e.AsssetSymbol, e.AsssetNetwork, e.Status});
        }

        //public async Task<int> InsertAsync(WithdrawalEntity entity)
        //{
        //    var result = await Transfers.Upsert(entity).On(e => e.TransactionId).NoUpdate().RunAsync();
        //    return result;
        //}

        //public async Task UpdateAsync(WithdrawalEntity entity)
        //{
        //    await UpdateAsync(new List<WithdrawalEntity>{entity});
        //}

        //public async Task UpdateAsync(IEnumerable<WithdrawalEntity> entities)
        //{
        //    Transfers.UpdateRange(entities);
        //    await SaveChangesAsync();
        //}
    }
}