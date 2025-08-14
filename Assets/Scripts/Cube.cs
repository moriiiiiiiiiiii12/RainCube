using System;
using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private float _lifeTime = 5f;

    private Colorer _colorer;
    private bool _isFirstTouch = true;

    public event Action<Cube> Touch;

    private void Start()
    {
        ResetParameters();
        _colorer = new Colorer(_renderer);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Platform>(out Platform platform) && _isFirstTouch == true)
        {
            _isFirstTouch = false;

            _colorer.ChangeRandomColor();

            StartCoroutine(Return(_lifeTime));
        }
    }

    public void ResetParameters()
    {
        _isFirstTouch = true;

        _rigidbody.velocity = Vector3.zero;

        transform.rotation = Quaternion.identity;

        _colorer.ChangeColor(_renderer, _defaultColor);
    }

    private IEnumerator Return(float delay)
    {
        yield return new WaitForSeconds(delay);

        Touch?.Invoke(this);
    }
}