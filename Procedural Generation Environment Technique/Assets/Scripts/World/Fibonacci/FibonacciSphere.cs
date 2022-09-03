using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Habrador_Computational_Geometry;


public class FibonacciSphere
{
    //Generate Fibonacci Sphere Point for Base Fibonacci
    public static Vector3[] GeneratePoints(int amount, float radius, float jitter)
    {
        Vector3[] points = new Vector3[amount];

        //Generate amount fibonacci sphere points.
        for (int i = 0; i < amount; i++)
        {
            Vector3 point = FibonacciSpherePoint(i, amount, radius, jitter);
            points[i] = point;
        }

        return points;
    }

    //Generate Fibonacci Sphere Point for Convex Hull base
    public static HashSet<MyVector3> GeneratePointSet(int amount, float radius, float jitter = 0)
    {
        //Hash points
        HashSet<MyVector3> points = new HashSet<MyVector3>();

        //Generate the Hash amount of fibionacci sphere points.
        for (int i = 0; i < amount; i++)
        {
            Vector3 p = FibonacciSpherePoint(i, amount, radius, jitter);
            MyVector3 point = p.ToMyVector3();
            points.Add(point);
        }

        return points;
    }

    private static Vector3 FibonacciSpherePoint(int i, int amount, float radius, float jitter)
    {
        //MATHS
        var j = i + .5f;

        var a = Mathf.Acos(1f - 2f * j / amount);
        var p = Mathf.PI * (1 + Mathf.Sqrt(5)) * j;

        var x = Mathf.Cos(p) * Mathf.Sin(a);
        var y = Mathf.Sin(p) * Mathf.Sin(a);
        var z = Mathf.Cos(a);
        Vector3 SpherePoint = new Vector3(x, y, z);

        float rangeX = Random.Range(-1, 1);
        float rangeY = Random.Range(-1, 1);
        float rangeZ = Random.Range(-1, 1);
        Vector3 pointJitter = new Vector3(rangeX, rangeY, rangeZ).normalized * jitter / (amount / 100);

        return (SpherePoint + pointJitter).normalized * radius;
    }
}
