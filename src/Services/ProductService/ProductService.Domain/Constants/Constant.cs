using ProductService.Domain.Aggregate.ProductAggregate;

namespace ProductService.Domain.Constants
{
    public static class Constant
    {
        public static class TableNames
        {
            public static string Products = $"{nameof(Product)}s";
        }

        public static string ProductService = $"{nameof(ProductService)}s";

        public static class ResponseMessage
        {
            public const string ServerError = "Something went wrong. Please try again later.";
            public const string InvalidRequest = "Invalid Request";
            public const string SampleNotFound = "Sample Not Found";
            public const string AddedSuccessfully = "Successfully Added";
            public const string AddedFailed = "Added Failed";
        }

        public static class ProductConsul
        {
            public const string Tag = "Product";
            public const string ID = "ProductService";
            public const string Name = "ProductService";
            public const string Host = "localhost";
            public const string Port = "5217";
        }

        public static class App
        {
            public static string ApplicationName = "ProductService";
            public static string Version = "v1";
            public static string Description = "This is User Validation service place";
            public static string DefaultImage = "cefd5882-a6d7-44b2-99d0-d5d85d7fbdfd";
        }

        public static class Fields
        {
            public const string Id = "Id";
            public const string ProductName = "name";
            public const string Description = "description";
            public const string CreatedDate = "CreatedDate";
            public const string ProductStatus = "ProductStatus";
            public const string ProductCategory = "productCategory";
            public const string Price = "Price";
            public const string StockCount = "StockCount";
        }

        public static class InMemory
        {
            public static string InMemoryProductKey = "InMemoryProductKey";
            public static Guid InMemoryProductKeyId = Guid.Parse("199c908c-a131-43bd-992c-cfbcc2dbfb53");
        }

        public static class Application
        {
            public static string ApplicationName = "ReminderApp.Api";
            public static string Version = "v1";
            public static string Description = "ProductApi version1 project";
        }

        public static class Roles
        {
            public static string Guest = "Guest";
            public static string User = "User";
            public static string Moderator = "Moderator";
            public static string Admin = "Admin";
            public static string Boss = "Boss";
        }

        public static class FilePaths
        {
            private static string currentDirectory = Directory.GetCurrentDirectory();
            private static string parentDirectory = Directory.GetParent(currentDirectory).FullName;
            private static string logsPath = Path.Combine(parentDirectory, "Logs".TrimStart('\\', '/'));
            private static IEnumerable<string> txtFiles = Directory.EnumerateFiles(logsPath, "*.txt");

            public static List<string> txtLogFiles = txtFiles.ToList();
        }
    }
}
