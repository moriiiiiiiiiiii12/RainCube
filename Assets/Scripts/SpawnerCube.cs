using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnerCube : Spawner<Cube>
{
    [SerializeField] protected float SpawnInterval = 1f;
    [SerializeField] protected float SpawnHeight = 2f;

    [Header("Доп компоненты: ")]
    [SerializeField] private Collider _platformCollider;

    public event Action<Vector3> OnDestroyCube;

    private void Start() => StartCoroutine(nameof(Spawn));

    private IEnumerator Spawn()
    {
        while (enabled)
        {
            if (CountActiveObjects < PoolSize)
            {
                Cube cube = Pool.Get();

                cube.transform.position = GetRandomPositionPlatform();

                cube.Touch += DestroyObject;
            }

            yield return new WaitForSeconds(SpawnInterval);
        }
    }
    
    protected override void DestroyObject(Cube cube)
    {
        if (cube != null)
        {
            cube.ResetParameters();
            cube.Touch -= DestroyObject;

            Vector3 position = cube.transform.position;             

            Pool.Release(cube);

            OnDestroyCube?.Invoke(position);
        }
    }

    private Vector3 GetRandomPositionPlatform()
    {
        Bounds boundsPlatform = _platformCollider.bounds;

        float randomX = UnityEngine.Random.Range(boundsPlatform.min.x, boundsPlatform.max.x);
        float randomZ = UnityEngine.Random.Range(boundsPlatform.min.z, boundsPlatform.max.z);
        float spawnHeight = boundsPlatform.max.y + SpawnHeight;

        return new Vector3(randomX, spawnHeight, randomZ);
    }
}