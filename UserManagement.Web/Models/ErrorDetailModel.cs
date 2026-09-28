using System;

namespace UserManagement.Web.Models;

public record ErrorDetailModel
{
    public required String Message { get; init; }
    public String? Details { get; init; }
}
