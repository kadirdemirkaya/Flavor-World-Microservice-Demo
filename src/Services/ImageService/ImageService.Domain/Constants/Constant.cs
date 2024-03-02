using ImageService.Domain.Aggregate;
using ImageService.Domain.Aggregate.Entities;

namespace ImageService.Domain.Constants
{
    public static class Constant
    {
        public static class TableNames
        {
            public static string Images = $"{nameof(Image)}s";
            public static string ImageUsers = $"{nameof(ImageUser)}s";
            public static string ImageProducts = $"{nameof(ImageProduct)}s";
        }

        public static class App
        {
            public static string ApplicationName = "ImageService";
            public static string Version = "v1";
            public static string Description = "This is User Validation service place";

            public static string DefaultUserImageId = "e850aeb7-4a4e-4f93-9189-a72a61482da3";
            public static string DefaultProductImageId = "7fd1628b-43e6-4215-ad20-9c834cc20ab0";
            public static string DefaultUserImagePath = @"C:\Users\Casper\Desktop\GitHub Projects\FlavorWorldMic\src\Services\ImageService\ImageService.Api\wwwroot\Images\User\defaultProfileIcon.png";
            public static string DefaultProductImagePath = @"C:\Users\Casper\Desktop\GitHub Projects\FlavorWorldMic\src\Services\ImageService\ImageService.Api\wwwroot\Images\Product\product_image_2fbDQDk.png";
        }

        public static class ImageConsul
        {
            public const string Tag = "Image";
            public const string ID = "ImageService";
            public const string Name = "ImageService";
            public const string Host = "localhost";
            public const string Port = "5187";
        }

        public static class FilePaths
        {
            private static string currentDirectory = Directory.GetCurrentDirectory();
            private static string logsPath = Path.Combine(currentDirectory, "Logs".TrimStart('\\', '/'));
            private static IEnumerable<string> txtFiles = Directory.EnumerateFiles(logsPath, "*.txt");

            public static List<string> txtLogFiles = txtFiles.ToList();
        }
    }
}
