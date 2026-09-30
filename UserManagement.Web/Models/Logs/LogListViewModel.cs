using System;
using System.Collections.Generic;
using UserManagement.Web.Models.Logs;

namespace UserManagement.Web.Models.Logs;

public class LogListViewModel
{
    public IReadOnlyList<LogEntryViewModel> Items { get; set; } = [];

    public String PagingAction { get; init; } = "List";

    public Int64? RouteId { get; init; }
}
