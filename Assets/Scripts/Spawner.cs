using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("Необходимые компоненты: ")]
    [SerializeField] protected T _prefab;

    [Header("Настройки пула: ")]
    [SerializeField] protected int _poolSize = 5;

    protected int _countActiveObjects = 0;
    protected ObjectPool<T> _pool;

    public int CountSpawnedObjects { get; private set; } = 0;
    public int CountCreatedObjects { get; private set; } = 0;
    public int CountActiveObjects => _countActiveObjects;

    public event Action OnStatsChange;

    protected virtual void Awake()
    {
        _pool = new ObjectPool<T>
        (
            createFunc: () =>
            {
                T prefab = Instantiate(_prefab);
                prefab.gameObject.SetActive(false);

                CountCreatedObjects++;
                OnStatsChange?.Invoke();

                return prefab;
            },
            actionOnGet: (prefab) =>
            {
                CountSpawnedObjects++;
                _countActiveObjects++;
                OnStatsChange?.Invoke();

                ActionOnGet(prefab);
            },
            actionOnRelease: (prefab) =>
            {
                _countActiveObjects--;
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
            defaultCapacity: _poolSize,
            maxSize: _poolSize
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
