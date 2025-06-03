using System;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Color _defaultColor;

    public event Action<Cube> Touch;

    public Renderer Renderer => _renderer;
    public Color DefaultColor => _defaultColor;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Platform>(out Platform platform))
        {
            Touch?.Invoke(this);
        }
    }
}
