namespace ProductService.Application.Abstractions
{
    public interface ISessionService<T> where T : notnull
    {
        string GetSessionValue(string key);
    }
}
