using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace FirmwareServer.EntityLayer
{
    public class DatabaseServices : IDatabaseServices
    {
        private readonly Database _db;
        private readonly ILogger<DatabaseServices> _logger;
        private readonly FirmwareServerConfiguration _configuration;

        public DatabaseServices(Database db, ILogger<DatabaseServices> logger, IOptions<FirmwareServerConfiguration> configuration)
        {
            _db = db;
            _logger = logger;
            _configuration = configuration.Value;
        }

        public void Backup()
        {
            var dataSource = Path.GetFullPath(Path.Combine(_configuration.AppData, $"FirmwareServer.{DateTime.Now:yyyyMMdd'T'HHmmss}.db"));

            var options = new DbContextOptionsBuilder<Database>();
            options.UseSqlite($"Data source={dataSource}");

            _logger.LogWarning($"Backing up database to {dataSource}");
            var sw = System.Diagnostics.Stopwatch.StartNew();

            using (var backup = new Database(options.Options))
            {
                backup.Database.Migrate();

                _db.BackupTo<Models.Device>(backup);
                _db.BackupTo<Models.DeviceLog>(backup);
                _db.BackupTo<Models.Firmware>(backup);
                _db.BackupTo<Models.DeviceType>(backup);
                _db.BackupTo<Models.Application>(backup);
            }

            _logger.LogWarning($"Backup took {sw.Elapsed.TotalSeconds} seconds");
        }

        public void Vacuum()
        {
            _logger.LogWarning("Running vacuumn on database");
            var sw = System.Diagnostics.Stopwatch.StartNew();
            _db.Database.ExecuteSqlCommand("VACUUM");
            _logger.LogWarning($"Vacuum took {sw.Elapsed.TotalSeconds} seconds");
        }
    }

    public interface IDatabaseServices
    {
        void Backup();

        void Vacuum();
    }

    public static class DbContextExtensions
    {
        public static void BackupTo<TEntity>(this DbContext from, DbContext to) where TEntity : class
        {
            foreach (var item in from.Set<TEntity>().AsNoTracking())
            {
                to.Set<TEntity>().Add(item);
                to.SaveChanges();
            }

        }
    }
}
