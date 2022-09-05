using Habrador_Computational_Geometry;
using System.Collections.Generic;
using UnityEngine;

public static class HelperMethods
{
    //Helpful answers unity from had similar funcations for random onUnitSphere 
    public static Vector3 GetPointOnUnitSphereCap(Quaternion targetDirection, float angle)
    {
        float angleRad = Random.Range(0.0f, angle) * Mathf.Deg2Rad;
        Vector2 pointCircle = (Random.insideUnitCircle.normalized) * Mathf.Sin(angleRad);
        Vector3 V = new Vector3(pointCircle.x, pointCircle.y, Mathf.Cos(angleRad));

        return targetDirection * V;
    }
    public static Vector3 GetPointOnUnitSphereCap(Vector3 targetDirection, float angle)
    {
        return GetPointOnUnitSphereCap(Quaternion.LookRotation(targetDirection), angle);
    }

    public static HashSet<Vector3> RandomPointsOnSphere(int n, float radius)
    {
        HashSet<Vector3> points = new HashSet<Vector3>();

        for (int i = 0; i < n; i++)
        {
            Vector3 point = Random.onUnitSphere * radius;
            points.Add(point);
        }

        return points;
    }

    public static HashSet<Vector3> GeneratePointClustersOnSphere(int numPoints, float radius)
    {
        float clusterRadius = 20f;
        int maxSize = numPoints / 2;
        int remainPoints = numPoints;
        HashSet<Vector3> points = new HashSet<Vector3>();

        while (remainPoints > 0)
        {
            Vector3 point = Random.onUnitSphere * radius;
            int clusterSize = Mathf.Min(maxSize, remainPoints) + 1;
            int numcusterPoints = Random.Range(1, clusterSize);

            for (int i = 0; i < numcusterPoints; i++)
            {
                Vector3 clusterPoint = GetPointOnUnitSphereCap(point, clusterRadius) * radius;
                points.Add(clusterPoint);
                remainPoints--;
            }
        }

        return points;
    }

    public static HashSet<Vector3> MixedPointClustersOnSphere(int n, float radius)
    {
        int q = n / 3;
        int r = n % 3;

        int numBigPlates = q;
        int numSmallPlates = 2 * q + r;

        HashSet<Vector3> randomPoints = RandomPointsOnSphere(numBigPlates, radius);
        HashSet<Vector3> randomClust = GeneratePointClustersOnSphere(numSmallPlates, radius);
        HashSet<Vector3> points = new HashSet<Vector3>(randomPoints);
        points.UnionWith(randomClust);

        return points;
    }
}