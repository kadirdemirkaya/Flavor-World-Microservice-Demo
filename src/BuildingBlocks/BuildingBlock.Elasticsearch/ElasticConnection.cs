namespace BuildingBlock.Elasticsearch
{
    public class ElasticConnectionFactory
    {
        public string Host { get; set; }
        public string Port { get; set; }
    }
    public static class ElasticConnection
    {
        public static string Host { get; set; } = "localhost";
        public static string Port { get; set; } = "9200";
    }
}
