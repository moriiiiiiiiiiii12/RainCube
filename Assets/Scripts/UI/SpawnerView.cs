using TMPro;
using UnityEngine;

public abstract class SpawnerView<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("Ссылка на спавнер")]
    [SerializeField] protected Spawner<T> _spawner;

    [Header("UI Элементы")]
    [SerializeField] protected TextMeshProUGUI _statsText;

    private void Start()
    {
        UpdateStats();
    }

    private void OnEnable()
    {
        _spawner.OnStatsChange += UpdateStats;
    }

    private void OnDisable()
    {
        _spawner.OnStatsChange -= UpdateStats;
    }

    protected virtual void UpdateStats()
    {
        _statsText.text = $"{typeof(T).Name}:\n" +
                          $"Создано: {_spawner.CountCreatedObjects}\n" +
                          $"Заспавнено: {_spawner.CountSpawnedObjects}\n" +
                          $"Активные: {_spawner.CountActiveObjects}";
    }
}
