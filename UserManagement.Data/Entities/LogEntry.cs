using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Data.Entities;

public record LogEntry
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Int64 Id { get; init; }

    public required String Description { get; init; }

    public required String Action { get; init;  }

    public required ApplicationUser User { get; init;  }

    public DateTime Timestamp { get; init; }
}
