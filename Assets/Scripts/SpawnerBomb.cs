using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;


public class SpawnerBomb : Spawner<Bomb>
{
    [Header("Доп компоненты: ")]
    [SerializeField] private SpawnerCube _spawnerCube;

    private void OnEnable()
    {
        _spawnerCube.OnDestroyCube += Spawn;
    }

    private void OnDisable()
    {
        _spawnerCube.OnDestroyCube -= Spawn;
    }

    private void Spawn(Vector3 position)
    {
        if (_countActiveObjects < _poolSize)
        {
            Bomb bomb = _pool.Get();
            bomb.transform.position = position;
            bomb.ExecuteExplode();

            bomb.OnExplode += DestroyObject;
        }
    }

    protected override void DestroyObject(Bomb bomb)
    {
        if (bomb != null)
        {
            bomb.ResetParameters();

            bomb.OnExplode -= DestroyObject;

            _pool.Release(bomb);
        }
    }
}