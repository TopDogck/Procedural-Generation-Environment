using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
//using Habrador_Computational_Geometry;
using static Habrador_Computational_Geometry.Delaunay3DToVoronoiAlgorithm;
public class VoronoiFibionacci : MonoBehaviour
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
            Color colour = Color.black;
            Gizmos.color = colour;

            // Draw sphere at the position of this point relative to the transform position.
            Vector3 point = points[i];
            Vector3 drawPos = point + transform.position;
            drawPos = transform.rotation * drawPos;
            Gizmos.DrawSphere(drawPos, 0.1f);
        }
    }
}
