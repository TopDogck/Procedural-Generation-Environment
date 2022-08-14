using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MIConvexHull;
using Habrador_Computational_Geometry;

public class ConvexHullBase : MonoBehaviour
{
    //Tried out ranges 
    [Range(100, 2500)]
    public int numPoints = 100;

    [Range(2f, 6f)]
    public float radius = 2f;

    [Range(-0.5f,0.5f)]
    public float jitter = 0f;

    private ConvexHull<DefaultVertex, DefaultConvexFace<DefaultVertex>> convexhull;
    private HalfEdgeData3 convexHull;
    private HashSet<MyVector3> Hpoints;

    //FibonacciSphere points with Convex Hull start 
    void OnValidate()
    {
        Vector3[] points = FibonacciSphere.GeneratePoints(numPoints, radius, jitter);
        List<double[]> converted = ConvertPoints(points);

        convexhull = GenerateConvexHull(converted);
    }

    //Mesh render and Mesh filter
    public void Update()
    {
        OnValidate();
        Hpoints = FibonacciSphere.GeneratePointSet(numPoints, radius, jitter);
        convexHull = IterativeHullAlgorithm3D.GenerateConvexHull(Hpoints, true);

        Mesh mesh = convexHull.ConvertToMyMesh("Planet", MyMesh.MeshStyle.HardAndSoftEdges).ConvertToUnityMesh(true, "Planet");

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    //Convex Hull
    private ConvexHull<DefaultVertex, DefaultConvexFace<DefaultVertex>> GenerateConvexHull(List<double[]> points)
    {
        var convexhull = ConvexHull.Create(points).Result;
        return convexhull;
    }

    //Vector3 to DefaultVertex
    private DefaultVertex ToDefaultVertex(Vector3 point)
    {
        DefaultVertex convertedPoint = new DefaultVertex();
        convertedPoint.Position = new double[] { point.x, point.y, point.z };
        return convertedPoint;
    }

    //Create the convert points
    private List<double[]> ConvertPoints(Vector3[] points)
    {
        List<double[]> converted = new List<double[]>();
        foreach (Vector3 point in points)
        {
            double[] pData = ToDefaultVertex(point).Position;
            converted.Add(pData);
        }

        return converted;
    }

    //Draw
    void OnDrawGizmos()
    {
        DrawConvexHull();
    }

    //DefaultVertex to Vector3
    private Vector3 ToVector3(DefaultVertex point)
    {
        double[] pos = point.Position;
        Vector3 converted = new Vector3((float)pos[0], (float)pos[1], (float)pos[2]);
        return converted;
    }

    void DrawConvexHull() // Draw all the points.
    {
        //Point colour
        Gizmos.color = Color.gray;
        foreach (DefaultVertex point in convexhull.Points)
        {
            Vector3 convertedPoint = ToVector3(point);
            Gizmos.DrawSphere(convertedPoint.normalized * radius, 0.1f);
        }

        //Convex lines colour
        Gizmos.color = Color.white;
        int[][] vertexCombo = new int[][] { new int[] { 0, 1 }, new int[] { 1, 2 }, new int[] { 2, 0 } };

        foreach (DefaultConvexFace<DefaultVertex> face in convexhull.Faces)
        {
            foreach (int[] faces in vertexCombo)
            {
                Vector3 f1 = ToVector3(face.Vertices[faces[0]]);
                Vector3 f2 = ToVector3(face.Vertices[faces[1]]);
                f1 = f1.normalized * radius;
                f2 = f2.normalized * radius;

                Gizmos.DrawLine(f1, f2);
            }
        }

    }
}
