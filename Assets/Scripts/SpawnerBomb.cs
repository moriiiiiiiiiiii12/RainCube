using UnityEngine;


public class SpawnerBomb : Spawner<Bomb>
{
    [Header("Доп компоненты: ")]
    [SerializeField] private SpawnerCube _spawnerCube;

    private void OnEnable()
    {
        _spawnerCube.DestroyCube += Spawn;
    }

    private void OnDisable()
    {
        _spawnerCube.DestroyCube -= Spawn;
    }

    private void Spawn(Vector3 position)
    {
        if (CountActiveObjects < PoolSize)
        {
            Bomb bomb = Pool.Get();
            bomb.transform.position = position;
            bomb.ExecuteExplode();

            bomb.Explode += DestroyObject;
        }
    }

    protected override void DestroyObject(Bomb bomb)
    {
        if (bomb != null)
        {
            bomb.ResetParameters();

            bomb.Explode -= DestroyObject;

            Pool.Release(bomb);
        }
    }
}