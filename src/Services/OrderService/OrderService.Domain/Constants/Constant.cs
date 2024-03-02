using OrderService.Domain.Aggregate.OrderAggregate;

namespace OrderService.Domain.Constants
{
    public static class Constant
    {
        public static class TableNames
        {
            public static string Orders = $"{nameof(Order)}s";
        }
        public static class InMemory
        {
            public static string InMemoryOrderKey = "InMemoryOrderKey";
            public static Guid InMemoryOrderKeyId = Guid.Parse("acce90d7-421e-4e90-b7ff-1f0e20cdff43");
        }

        public static class App
        {
            public static string ApplicationName = "OrderService";
            public static string Swagger = "Swagger";
            public static string Version = "v1";
            public static string Description = "This is order service place";
        }

        public static class OrderConsul
        {
            public const string Tag = "Order";
            public const string ID = "OrderService";
            public const string Name = "OrderService";
            public const string Host = "localhost";
            public const string Port = "7018";
        }
    }
}
