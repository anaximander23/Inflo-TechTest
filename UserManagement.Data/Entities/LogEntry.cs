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

    public Int64? UserId { get; init; }
    public ApplicationUser? User { get; init; }

    public String? TargetType { get; init; }
    public Int64? TargetId { get; init; }
    public String? TargetLabel { get; init; }

    public DateTime Timestamp { get; init; }
}
