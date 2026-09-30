using System;

namespace UserManagement.Data.Entities;

public readonly record struct LogTarget(String Type, Int64 Id, String? Label = null);

public static class LogTargets
{
    public const String User = "user";
}
