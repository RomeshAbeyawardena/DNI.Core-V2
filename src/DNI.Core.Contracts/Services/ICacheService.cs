namespace DNI.Core.Contracts.Services
{
    public interface ICacheService
    {
        bool TryGet<T>(string key, out T Value)
            where T: class;

        bool TrySet<T>(string key, T value)
            where T: class;
    }
}
