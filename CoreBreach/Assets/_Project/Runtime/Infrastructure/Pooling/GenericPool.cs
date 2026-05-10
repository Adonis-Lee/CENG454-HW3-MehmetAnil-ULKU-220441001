using UnityEngine;
using UnityEngine.Pool;

namespace CoreBreach.Infrastructure.Pooling
{
    /// <summary>
    /// MonoBehaviour + IPoolable tipleri için generic pool wrapper.
    /// Unity'nin built-in ObjectPool'unu sarar; OnSpawn/OnDespawn lifecycle'ını
    /// disiplinli şekilde tetikler — Debug Report #003 (Ghost Subscriber) önlemi.
    /// </summary>
    public class GenericPool<T> : IPool<T> where T : MonoBehaviour, IPoolable
    {
        private readonly ObjectPool<T> pool;
        private readonly T prefab;
        private readonly Transform parent;

        public GenericPool(T prefab, Transform parent, int defaultCapacity = 16, int maxSize = 256)
        {
            this.prefab = prefab;
            this.parent = parent;
            pool = new ObjectPool<T>(
                createFunc: Create,
                actionOnGet: OnGet,
                actionOnRelease: OnRelease,
                actionOnDestroy: OnDestroyItem,
                collectionCheck: true,
                defaultCapacity: defaultCapacity,
                maxSize: maxSize);
        }

        public T Get() => pool.Get();
        public void Release(T item) => pool.Release(item);

        private T Create()
        {
            T instance = Object.Instantiate(prefab, parent);
            instance.gameObject.SetActive(false);
            return instance;
        }

        private void OnGet(T item)
        {
            item.gameObject.SetActive(true);
            item.OnSpawn();
        }

        private void OnRelease(T item)
        {
            item.OnDespawn();
            item.gameObject.SetActive(false);
        }

        private void OnDestroyItem(T item)
        {
            if (item != null) Object.Destroy(item.gameObject);
        }
    }
}
