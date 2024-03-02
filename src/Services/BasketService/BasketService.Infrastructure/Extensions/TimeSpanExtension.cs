namespace BasketService.Infrastructure.Extensions
{
    public static class TimeSpanExtension
    {
        public static TimeSpan GetDefaultTimeSpan(int day = 1, int hours = 0, int minute = 0, int second = 0)
        {
            return new TimeSpan(day, hours, minute, second);
        }
    }
}
