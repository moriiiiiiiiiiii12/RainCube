using System;
using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private float _lifeTime = 5f;

    private bool _isFirstTouch = true;

    public event Action<Cube> Touch;

    private void Start() => ResetParameters();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Platform>(out Platform platform) && _isFirstTouch == true)
        {
            _isFirstTouch = false;

            Colorer.ChangeRandomColor(_renderer);

            StartCoroutine(ReturnCube(_lifeTime));
        }
    }

    public void ResetParameters()
    {
        _isFirstTouch = true;

        if (TryGetComponent(out Rigidbody rigidbody))
            rigidbody.velocity = Vector3.zero;

        transform.rotation = Quaternion.identity;

        Colorer.ChangeColor(_renderer, _defaultColor);
    }

    private IEnumerator ReturnCube(float delay)
    {
        yield return new WaitForSeconds(delay);

        Touch?.Invoke(this);
    }
}