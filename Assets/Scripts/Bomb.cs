using System;
using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float _minFadeTime = 2f;
    [SerializeField] private float _maxFadeTime = 5f;
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private float _explosionForce = 700f;

    [SerializeField] private Renderer _bombRenderer;
    
    private Color _originalColor;
    private float _fadeTime;

    public event Action<Bomb> OnExplode;

    public void ExecuteExplode()
    {
        if (_originalColor.a == 0)
        {
            _originalColor = _bombRenderer.material.color; 
        }

        _fadeTime = UnityEngine.Random.Range(_minFadeTime, _maxFadeTime);

        StartCoroutine(FadeAndExplode());
    }

    private IEnumerator FadeAndExplode()
    {
        yield return Colorer.FadeOut(_bombRenderer, _originalColor, _fadeTime);

        Exploder.Explode(transform.position, _explosionRadius, _explosionForce);
        OnExplode?.Invoke(this);

    }

    public void ResetParameters()
    {
        _bombRenderer.material.color = _originalColor;
        _fadeTime = UnityEngine.Random.Range(_minFadeTime, _maxFadeTime);
    }
}
