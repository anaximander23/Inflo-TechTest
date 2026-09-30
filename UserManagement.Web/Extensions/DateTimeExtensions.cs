namespace UserManagement.Web.Extensions;

public static class DateTimeExtensions
{
    extension(DateTime dateTime)
    {
        public DateTime AsUtc() => dateTime.Kind switch
        {
            DateTimeKind.Utc => dateTime,
            DateTimeKind.Local => dateTime.ToUniversalTime(),
            _ => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
        };
    }
}
