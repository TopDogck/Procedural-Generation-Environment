using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Habrador_Computational_Geometry;


public class FibonacciSphere
{
    public static Vector3[] GeneratePoints(int n, float radius, float jitter = 0)
    {
        Vector3[] points = new Vector3[n];

        //Generate amount fibonacci sphere points.
        for (int i = 0; i < n; i++)
        {
            Vector3 point = FibonacciSpherePoint(i, n, radius, jitter);
            points[i] = point;
        }

        return points;
    }

    public static HashSet<MyVector3> GeneratePointSet(int n, float radius, float jitter = 0)
    {
        //Hash points
        HashSet<MyVector3> points = new HashSet<MyVector3>();

        //Generate the Hash amount of fibionacci sphere points.
        for (int i = 0; i < n; i++)
        {
            Vector3 p = FibonacciSpherePoint(i, n, radius, jitter);
            MyVector3 point = p.ToMyVector3();
            points.Add(point);
        }

        return points;
    }

    private static Vector3 FibonacciSpherePoint(int i, int n, float radius, float jitter)
    {
        //MATHS
        var j = i + .5f;

        var phi = Mathf.Acos(1f - 2f * j / n);
        var theta = Mathf.PI * (1 + Mathf.Sqrt(5)) * j;

        var x = Mathf.Cos(theta) * Mathf.Sin(phi);
        var y = Mathf.Sin(theta) * Mathf.Sin(phi);
        var z = Mathf.Cos(phi);
        Vector3 SpherePoint = new Vector3(x, y, z);

        float randX = Random.Range(-1, 1);
        float randY = Random.Range(-1, 1);
        float randZ = Random.Range(-1, 1);
        Vector3 pointJitter = new Vector3(randX, randY, randZ).normalized * jitter / (n / 100);

        return (SpherePoint + pointJitter).normalized * radius;
    }
}
