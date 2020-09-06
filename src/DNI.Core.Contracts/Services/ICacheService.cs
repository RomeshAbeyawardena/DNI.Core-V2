namespace DNI.Core.Contracts.Services
{
    public interface ICacheService
    {
        bool TryGet<T>(string key, out T Value);
        bool TrySet<T>(T value);
    }
}
