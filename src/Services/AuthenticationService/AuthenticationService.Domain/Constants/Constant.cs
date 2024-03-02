using AuthenticationService.Domain.Aggregate;
using AuthenticationService.Domain.Aggregate.Entities;

namespace AuthenticationService.Domain.Constants
{
    public static class Constant
    {
        public static class TableNames
        {
            public static string Users = $"{nameof(User)}s";
            public static string Roles = $"{nameof(Role)}s";
            public static string RolUsers = $"{nameof(RoleUser)}s";
        }

        public static class App
        {
            public const string ApplicationName = "AuthenticationService";
            public const string Version = "v1";
            public const string Description = "This is User Validation service place";
        }

        public static class AuthenticationConsul
        {
            public const string Tag = "Authentication";
            public const string ID = "AuthenticationService";
            public const string Name = "AuthenticationService";
            public const string Host = "localhost";
            public const string Port = "5285";
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
