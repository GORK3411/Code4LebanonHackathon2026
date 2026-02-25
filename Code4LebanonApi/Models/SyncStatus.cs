using System;
using System.ComponentModel.DataAnnotations;

namespace Code4LebanonApi.Models
{
    public class SyncStatus
    {
        [Key]
        public int Id { get; set; }

        // Unique name for the syncing job/service (e.g. "NumuResponses")
        public string ServiceName { get; set; } = string.Empty;

        // Last time we fully processed responses up to (inclusive)
        public DateTime? LastProcessedAt { get; set; }

        // Optional id of last processed item
        public string? LastProcessedId { get; set; }

        public DateTime? LastRunAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
