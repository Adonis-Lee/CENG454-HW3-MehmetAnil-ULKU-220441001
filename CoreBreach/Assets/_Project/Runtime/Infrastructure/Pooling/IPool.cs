namespace CoreBreach.Infrastructure.Pooling
{
    /// <summary>
    /// Generic pool sözleşmesi. Domain bunu ister, somutu Bootstrap verir.
    /// </summary>
    public interface IPool<T> where T : class
    {
        T Get();
        void Release(T item);
    }
}
