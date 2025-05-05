using OpenEcologyApp.Data;
using Microsoft.EntityFrameworkCore;

namespace OpenEcologyApp.Services
{
    public class StatisticsService
    {
        private readonly EcologyDbContext _context;
        public StatisticsService(EcologyDbContext context)
        {
            _context = context;
        }

        // Логирование экспорта
        public async Task LogExport(int userId, string exportType)
        {
            var log = new ExportLog
            {
                UserId = userId,
                ExportType = exportType,
                ExportedAt = DateTime.UtcNow
            };
            _context.ExportLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        // Логирование отчёта
        public async Task LogReport(int userId, string reportType)
        {
            var log = new ReportLog
            {
                UserId = userId,
                ReportType = reportType,
                ReportedAt = DateTime.UtcNow
            };
            _context.ReportLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        // Получить статистику
        public async Task<AdminStatsDto> GetAdminStats()
        {
            var usersCount = await _context.Users.CountAsync();
            var exportsCount = await _context.ExportLogs.CountAsync();
            var reportsCount = await _context.ReportLogs.CountAsync();
            return new AdminStatsDto
            {
                UsersCount = usersCount,
                ExportsCount = exportsCount,
                ReportsCount = reportsCount
            };
        }

        public async Task<List<DailyStatsDto>> GetDailyStats(int days = 30)
        {
            var fromDate = DateTime.UtcNow.Date.AddDays(-days + 1);
            var exportStats = await _context.ExportLogs
                .Where(e => e.ExportedAt >= fromDate)
                .GroupBy(e => e.ExportedAt.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToListAsync();
            var reportStats = await _context.ReportLogs
                .Where(r => r.ReportedAt >= fromDate)
                .GroupBy(r => r.ReportedAt.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToListAsync();

            var allDates = Enumerable.Range(0, days)
                .Select(i => fromDate.AddDays(i))
                .ToList();

            var result = allDates.Select(date => new DailyStatsDto
            {
                Date = date,
                ExportCount = exportStats.FirstOrDefault(x => x.Date == date)?.Count ?? 0,
                ReportCount = reportStats.FirstOrDefault(x => x.Date == date)?.Count ?? 0
            }).ToList();

            return result;
        }
    }

    public class AdminStatsDto
    {
        public int UsersCount { get; set; }
        public int ExportsCount { get; set; }
        public int ReportsCount { get; set; }
    }

    public class DailyStatsDto
    {
        public DateTime Date { get; set; }
        public int ExportCount { get; set; }
        public int ReportCount { get; set; }
    }
} 