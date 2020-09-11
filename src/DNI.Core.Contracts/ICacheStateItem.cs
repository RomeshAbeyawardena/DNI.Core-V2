namespace DNI.Core.Contracts
{
    public interface ICacheStateItem<TState>
    {
        public string Key { get; }
        public TState State  { get; }
    }
}
