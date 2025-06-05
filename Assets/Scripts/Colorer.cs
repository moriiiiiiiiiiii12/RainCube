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
}
