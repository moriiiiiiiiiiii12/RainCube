using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class RainCubes : MonoBehaviour
{
    [Header("Необходимые компоненты: ")]
    [SerializeField] private Collider _platformCollider;
    [SerializeField] private Cube _prefabCube;
    [SerializeField] private Colorer _colorer;

    [Header("Настройки пула: ")]
    [SerializeField] private float _spawnHeight = 2f;
    [SerializeField] private int _poolSize = 5;
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private float _lifeTime = 5f;

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

    private void Start() => InvokeRepeating(nameof(SpawnCube), 0.0f, _spawnInterval);

    private void SpawnCube()
    {
        if (_countActiveCubes >= _poolSize)
            return;

        _countActiveCubes++;

        Cube cube = _pool.Get();
        cube.Touch += DestroyCube;
    }

    private void DestroyCube(Cube cube)
    {
        cube.Touch -= DestroyCube;
        _colorer.ChangeRandomColor(cube.Renderer);

        StartCoroutine(ReturnCube(cube, _lifeTime));
    }

    private IEnumerator ReturnCube(Cube cube, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (cube != null)
        {
            _pool.Release(cube);
            _countActiveCubes--;
        }
    }

    private void ActionOnGet(Cube cube)
    {
        cube.transform.position = GetRandomPositionPlatform();

        if (cube.TryGetComponent(out Rigidbody rigidbody))
            rigidbody.velocity = Vector3.zero;

        _colorer.ChangeColor(cube.Renderer, cube.DefaultColor);

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
