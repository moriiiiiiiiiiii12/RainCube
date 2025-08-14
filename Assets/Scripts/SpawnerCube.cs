using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnerCube : Spawner<Cube>
{
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private float _spawnHeight = 2f;

    [Header("Доп компоненты: ")]
    [SerializeField] private Collider _platformCollider;

    public event Action<Vector3> DestroyCube;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_spawnInterval);

        while (enabled)
        {
            if (CountActiveObjects < PoolSize)
            {
                Cube cube = Pool.Get();

                cube.transform.position = GetRandomPositionPlatform();

                cube.Touch += DestroyObject;
            }

            yield return waitForSeconds;
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

            DestroyCube?.Invoke(position);
        }
    }

    private Vector3 GetRandomPositionPlatform()
    {
        Bounds boundsPlatform = _platformCollider.bounds;

        float randomX = UnityEngine.Random.Range(boundsPlatform.min.x, boundsPlatform.max.x);
        float randomZ = UnityEngine.Random.Range(boundsPlatform.min.z, boundsPlatform.max.z);
        float spawnHeight = boundsPlatform.max.y + _spawnHeight;

        return new Vector3(randomX, spawnHeight, randomZ);
    }
}