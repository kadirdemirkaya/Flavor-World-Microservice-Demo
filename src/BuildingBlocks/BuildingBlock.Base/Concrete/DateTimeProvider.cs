using BuildingBlock.Base.Abstractions;

namespace BuildingBlock.Base.Concrete
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
