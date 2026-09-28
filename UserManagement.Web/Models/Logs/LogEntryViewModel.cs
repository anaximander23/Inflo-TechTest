using System;

namespace UserManagement.Web.Models.Logs;

public class LogEntryViewModel
{
    public String Action { get; init; } = String.Empty;
    public String Description { get; init; } = String.Empty;
    public String? ActorEmail { get; init; }
    public DateTime Timestamp { get; init; }
}