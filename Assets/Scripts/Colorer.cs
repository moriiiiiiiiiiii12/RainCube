using UnityEngine;

public class Colorer : MonoBehaviour
{
    public void ChangeRandomColor(Renderer renderer)
    {
        renderer.material.color = Random.ColorHSV();
    }

    public void ChangeColor(Renderer renderer, Color color)
    {
        renderer.material.color = color;
    }
}
