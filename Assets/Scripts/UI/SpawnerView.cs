using TMPro;
using UnityEngine;

public abstract class SpawnerView<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("Ссылка на спавнер")]
    [SerializeField] protected Spawner<T> Spawner;

    [Header("UI Элементы")]
    [SerializeField] protected TextMeshProUGUI StatsText;

    private void Start()
    {
        UpdateStats();
    }

    private void OnEnable()
    {
        Spawner.CreatedObjectChange += UpdateCreatedStats;
        Spawner.SpawnedObjectChange += UpdateSpawnedStats;
        Spawner.ActiveObjectChange += UpdateActiveStats;
    }

    private void OnDisable()
    {
        Spawner.CreatedObjectChange -= UpdateCreatedStats;
        Spawner.SpawnedObjectChange -= UpdateSpawnedStats;
        Spawner.ActiveObjectChange -= UpdateActiveStats;
    }

    protected virtual void UpdateCreatedStats(int count)
    {
        UpdateStats();
    }

    protected virtual void UpdateSpawnedStats(int count)
    {
        UpdateStats();
    }

    protected virtual void UpdateActiveStats(int count)
    {
        UpdateStats();
    }

    protected virtual void UpdateStats()
    {
        StatsText.text = $"{typeof(T).Name}:\n" +
                          $"Создано: {Spawner.CountCreatedObjects}\n" +
                          $"Заспавнено: {Spawner.CountSpawnedObjects}\n" +
                          $"Активные: {Spawner.CountActiveObjects}";
    }
}

