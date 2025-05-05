using System;

namespace OpenEcologyApp.Data
{
    public class ReportLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime ReportedAt { get; set; } = DateTime.UtcNow;
        public string ReportType { get; set; } = string.Empty;
    }
} 