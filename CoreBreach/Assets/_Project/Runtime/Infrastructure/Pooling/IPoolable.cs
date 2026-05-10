namespace CoreBreach.Infrastructure.Pooling
{
    /// <summary>
    /// Pool tarafından yeniden kullanılan nesnelerin sözleşmesi.
    /// OnSpawn  — pool'dan alındığında state init + subscribe
    /// OnDespawn — pool'a iade edilirken unsubscribe + reset
    /// (Bkz Debug Report #003 — Ghost Subscriber önleme.)
    /// </summary>
    public interface IPoolable
    {
        void OnSpawn();
        void OnDespawn();
    }
}
