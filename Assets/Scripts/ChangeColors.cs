using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColors : MonoBehaviour
{
    public Color32[] gameColors = new Color32[4];
    public void ColorMesh(Mesh mesh, int scoreCount)
    {
        Vector3[] vertices = mesh.vertices;
        Color32[] colors = new Color32[vertices.Length];
        float f = Mathf.Sin(scoreCount * 0.25f);

        for (int i = 0; i < vertices.Length; i++)
        {
            colors[i] = Lerp4(gameColors[0], gameColors[1], gameColors[2], gameColors[3], f);
        }
        mesh.colors32 = colors;
    }
    private Color32 Lerp4(Color32 a, Color32 b, Color32 c, Color32 d, float t)
    {
        if (t < 0.33f)
        {
            return Color.Lerp(a, b, t / 0.33f);
        }
        else if (t < 0.66f)
        {
            return Color.Lerp(b, c, (t - 0.33f) / 0.33f);
        }
        else
        {
            return Color.Lerp(c, d, (t - 0.66f) / 0.66f);
        }
    }//lerp4
}
