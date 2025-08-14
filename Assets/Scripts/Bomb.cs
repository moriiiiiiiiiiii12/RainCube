using System;
using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float _minFadeTime = 2f;
    [SerializeField] private float _maxFadeTime = 5f;
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private float _explosionForce = 700f;

    [SerializeField] private Renderer _renderer;

    private Exploder _exploder = new Exploder();
    private Colorer _colorer;
    private Color _originalColor;
    private float _fadeTime;

    public event Action<Bomb> Explode;

    private void Start()
    {
        _colorer = new Colorer(_renderer);
    }

    public void ExecuteExplode()
    {
        if (_originalColor.a == 0)
        {
            _originalColor = _renderer.material.color;
        }

        _fadeTime = UnityEngine.Random.Range(_minFadeTime, _maxFadeTime);

        StartCoroutine(FadeAndExplode());
    }

    private IEnumerator FadeAndExplode()
    {
        yield return _colorer.FadeOut(_originalColor, _fadeTime);

        _exploder.Explode(transform.position, _explosionRadius, _explosionForce);
        Explode?.Invoke(this);

    }

    public void ResetParameters()
    {
        _renderer.material.color = _originalColor;
        _fadeTime = UnityEngine.Random.Range(_minFadeTime, _maxFadeTime);
    }
}
