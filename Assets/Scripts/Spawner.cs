using System;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("Необходимые компоненты: ")]
    [SerializeField] protected T Prefab;

    [Header("Настройки пула: ")]
    [SerializeField] protected int PoolSize = 5;

    protected ObjectPool<T> Pool;

    public int CountSpawnedObjects { get; private set; } = 0;
    public int CountCreatedObjects { get; private set; } = 0;
    public int CountActiveObjects { get; private set; } = 0;

    public event Action OnStatsChange;

    protected virtual void Awake()
    {
        Pool = new ObjectPool<T>
        (
            createFunc: () =>
            {
                T prefab = Instantiate(Prefab);
                prefab.gameObject.SetActive(false);

                CountCreatedObjects++;
                OnStatsChange?.Invoke();

                return prefab;
            },
            actionOnGet: (prefab) =>
            {
                CountSpawnedObjects++;
                CountActiveObjects++;
                OnStatsChange?.Invoke();

                ActionOnGet(prefab);
            },
            actionOnRelease: (prefab) =>
            {
                CountActiveObjects--;
                OnStatsChange?.Invoke();

                ActionOnRelease(prefab);
            },
            actionOnDestroy: (prefab) =>
            {
                CountCreatedObjects--;
                OnStatsChange?.Invoke();

                Destroy(prefab);
            },
            collectionCheck: true,
            defaultCapacity: PoolSize,
            maxSize: PoolSize
        );
    }

    protected void ActionOnGet(T prefab)
    {
        prefab.gameObject.SetActive(true);
    }

    protected void ActionOnRelease(T prefab)
    {
        prefab.gameObject.SetActive(false);
    }

    protected abstract void DestroyObject(T prefab);
}
