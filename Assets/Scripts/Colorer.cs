using System.Collections;
using UnityEngine;

public class Colorer : MonoBehaviour
{
    private Renderer _renderer;

    public Colorer(Renderer renderer)
    {
        _renderer = renderer;
    }

    public void ChangeRandomColor()
    {
        _renderer.material.color = Random.ColorHSV();
    }

    public void ChangeColor(Renderer renderer, Color color)
    {
        renderer.material.color = color;
    }

    public IEnumerator FadeOut(Color originalColor, float fadeTime)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / fadeTime);
            
            Color intermediateColor = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(originalColor.a, 0, t));
            _renderer.material.color = intermediateColor;

            yield return null;
        }
    }
}
