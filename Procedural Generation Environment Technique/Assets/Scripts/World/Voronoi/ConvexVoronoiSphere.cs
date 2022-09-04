using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Habrador_Computational_Geometry;
using static Habrador_Computational_Geometry.Delaunay3DToVoronoiAlgorithm;

public class ConvexVoronoiSphere : MonoBehaviour
{
    //Tried out ranges 
    [Range(100, 2500)]
    public int numPoints = 500;

    [Range(2f, 6f)]
    public float radius = 2f;

    [Range(-0.5f, 0.5f)]
    public float jitter = 0f;
    private HalfEdgeData3 convexHull;

    public VoronoiCenter centreType;
    public bool removeUnwantedTriangle = true;
    private Vector3[] points;
    private HashSet<VoronoiCell3> voronoiCells;

    //Random seed and start generate sphere
    private void OnValidate()
    {
        GenerateSphere();
    }

    //new voronoi cells convex Hull sphere generator
    private void GenerateSphere()
    {
        // Convert the points to MyVector3 and then normalize every point.
        points = FibonacciSphere.GeneratePoints(numPoints, radius, jitter: jitter);

        HashSet<MyVector3> pointsSet = new HashSet<Vector3>(points).ToMyVector3();
        Normalizer3 normalizer = new Normalizer3(new List<MyVector3>(pointsSet));
        HashSet<MyVector3> normalizedPoints = normalizer.Normalize(pointsSet);

        // create a convex hull which is a 3D Delaunay Triangulation of the FP.
        convexHull = IterativeHullAlgorithm3D.GenerateConvexHull(normalizedPoints, removeUnwantedTriangle, null);

        // Set and denormalize voronoi cells.
        voronoiCells = Delaunay3DToVoronoiAlgorithm.GenerateVoronoiDiagram(convexHull, center: centreType); // US vs UK

        voronoiCells = normalizer.UnNormalize(voronoiCells);
        convexHull = normalizer.UnNormalize(convexHull);

        //It can use the MESH
        //Mesh mesh = convexHull.ConvertToMyMesh("Planet", MyMesh.MeshStyle.HardAndSoftEdges).ConvertToUnityMesh(true, "Sphere");
        //MeshFilter meshFilter = GetComponent<MeshFilter>();
        //meshFilter.mesh = mesh;
    }
    private void OnDrawGizmos()
    {
        foreach (VoronoiCell3 cell in voronoiCells)
        {
            Gizmos.DrawSphere(transform.rotation * cell.sitePos.ToVector3(), 0.01f);
            foreach (VoronoiEdge3 edge in cell.edges)
            {
                Vector3 p1 = transform.rotation * edge.p1.ToVector3();
                Vector3 p2 = transform.rotation * edge.p2.ToVector3();

                Gizmos.DrawLine(p1, p2);
            }
        }
    }
}
