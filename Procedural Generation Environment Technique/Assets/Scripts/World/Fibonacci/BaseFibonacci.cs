using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFibonacci : MonoBehaviour
{
    public int amount;
    public float radius;
    private Vector3[] points;

    private void OnValidate()
    {
        points = FibonacciSphere.GeneratePoints(amount, radius);
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Length; i++)
        {
            // Set Colour.
            //float hue = (i + 1f) / points.Length;
            Color colour = Color.white;//HSVToRGB(0, 1f, 1f);
            Gizmos.color = colour;

            // Draw sphere at the position of this point relative to the transform position.
            Vector3 point = points[i];
            Vector3 drawPos = point + transform.position;
            drawPos = transform.rotation * drawPos;
            Gizmos.DrawSphere(drawPos, 0.1f);
        }
    }
}
