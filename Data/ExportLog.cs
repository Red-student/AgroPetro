using System;

namespace OpenEcologyApp.Data
{
    public class ExportLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime ExportedAt { get; set; } = DateTime.UtcNow;
        public string ExportType { get; set; } = string.Empty;
    }
} 