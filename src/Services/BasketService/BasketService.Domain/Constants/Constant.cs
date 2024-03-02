using BasketService.Domain.Aggregate;
using BasketService.Domain.Aggregate.Entities;

namespace BasketService.Domain.Constants
{
    public static class Constant
    {
        public static class TableNames
        {
            public static string Baskets = $"{nameof(Basket)}s";
            public static string BasketItems = $"{nameof(BasketItem)}s";
        }

        public static class InMemory
        {
            public static string InMemoryBasketItemKey = "InMemoryBasketItemKey";
            public static Guid InMemoryBasketItemKeyId = Guid.Parse("5e78cffd-8986-4dd4-8ea7-6b342ad8ab99");

            public static string InMemoryBasketKey = "InMemoryBasketKey";
            public static Guid InMemoryBasketKeyId = Guid.Parse("5e78cffd-8986-4dd4-8ea7-6b342ad8ab88");
        }

        public static class App
        {
            public static string ApplicationName = "BasketService";
            public static string Version = "v1";
            public static string Description = "This is User Validation service place";
        }

        public static class BasketConsul
        {
            public const string Tag = "Basket";
            public const string ID = "BasketService";
            public const string Name = "BasketService";
            public const string Host = "localhost";
            public const string Port = "7198";
        }
    }
}
