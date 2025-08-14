using System.Collections;
using UnityEngine;

public class Colorer : MonoBehaviour
{
    public static void ChangeRandomColor(Renderer renderer)
    {
        renderer.material.color = Random.ColorHSV();
    }

    public static void ChangeColor(Renderer renderer, Color color)
    {
        renderer.material.color = color;
    }

    public static IEnumerator FadeOut(Renderer renderer, Color originalColor, float fadeTime)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / fadeTime);
            
            Color intermediateColor = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(originalColor.a, 0, t));
            renderer.material.color = intermediateColor;

            yield return null;
        }
    }
}
