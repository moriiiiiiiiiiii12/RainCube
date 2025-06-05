using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [Header("Необходимые компоненты: ")]
    [SerializeField] private Collider _platformCollider;
    [SerializeField] private Cube _prefabCube;

    [Header("Настройки пула: ")]
    [SerializeField] private float _spawnHeight = 2f;
    [SerializeField] private int _poolSize = 5;
    [SerializeField] private float _spawnInterval = 1f;

    private int _countActiveCubes = 0;
    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>
        (
        createFunc: () =>
        {
            Cube cube = Instantiate(_prefabCube);
            cube.gameObject.SetActive(false);

            return cube;
        },
        actionOnGet: (cube) => ActionOnGet(cube),
        actionOnRelease: (cube) => cube.gameObject.SetActive(false),
        actionOnDestroy: (cube) => Destroy(cube),
        collectionCheck: true,
        defaultCapacity: _poolSize,
        maxSize: _poolSize
        );
    }

    private void Start() => StartCoroutine(nameof(SpawnCube));

    private IEnumerator SpawnCube()
    {
        while (true)
        {
            if (_countActiveCubes <= _poolSize)
            {
                _countActiveCubes++;
                Cube cube = _pool.Get();

                cube.Touch += DestroyCube;
            }

            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private void DestroyCube(Cube cube)
    {
        cube.Touch -= DestroyCube;

        _pool.Release(cube);
        _countActiveCubes--;
    }

    private void ActionOnGet(Cube cube)
    {
        cube.transform.position = GetRandomPositionPlatform();

        cube.gameObject.SetActive(true);
    }

    private Vector3 GetRandomPositionPlatform()
    {
        Bounds boundsPlatform = _platformCollider.bounds;

        float randomX = Random.Range(boundsPlatform.min.x, boundsPlatform.max.x);
        float randomZ = Random.Range(boundsPlatform.min.z, boundsPlatform.max.z);
        float spawnHeight = boundsPlatform.max.y + _spawnHeight;

        return new Vector3(randomX, spawnHeight, randomZ);
    }
}
