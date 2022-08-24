using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Habrador_Computational_Geometry;

//Code is very messy
public class FlatCrust
{
    public int numberPlates;
    public float radius;
    public int seed;

    public List<FlatPlate> plates;

    //End values called
    public FlatCrust(int numberPlates, float radius, Mesh sampleMesh, int seed = 1)
    {
        this.numberPlates = numberPlates;
        this.radius = radius;
        this.seed = seed;
        plates = GeneratePlates();
        SampleMesh(sampleMesh);
    }

    //Generate Hash VoronoiCell
    private HashSet<VoronoiCell3> GenerateVoronoiCells(HashSet<Vector3> points)
    {
        HashSet<MyVector3> convertedPoints = points.ToMyVector3();
        List<MyVector3> unitBoundingBox = new List<MyVector3>(convertedPoints);
        Normalizer3 normalizer = new Normalizer3(unitBoundingBox);

        // Normalize all points
        HashSet<MyVector3> normalizedPoints = normalizer.Normalize(convertedPoints);

        // Generate the convex hull using normalized points
        HalfEdgeData3 convexHull = IterativeHullAlgorithm3D.GenerateConvexHull(normalizedPoints, false);

        HashSet<VoronoiCell3> voronoiCells = Delaunay3DToVoronoiAlgorithm.GenerateVoronoiDiagram(convexHull);

        // De-normalize the voronoi diagram.
        voronoiCells = normalizer.UnNormalize(voronoiCells);

        return voronoiCells;
    }

    //Creat point cluster on sphere
    public static HashSet<Vector3> GeneratePointClustersOnSphere(int numPoints, float radius)
    {
        float clusterRadius = 20f; //20 good number found
        int maxClusterSize = numPoints / 2;
        int numPointsLeft = numPoints;
        HashSet<Vector3> points = new HashSet<Vector3>();

        while (numPointsLeft > 0)
        {
            Vector3 point = Random.onUnitSphere * radius;

            // Get number of points wanted in cluster.
            int clusterSize = Mathf.Min(maxClusterSize, numPointsLeft) + 1;
            int numClusterPoints = Random.Range(1, clusterSize);

            for (int i = 0; i < numClusterPoints; i++)
            {
                Vector3 clusterPoint = GetterPointOnUnit(point, clusterRadius) * radius;
                points.Add(clusterPoint);
                numPointsLeft--;
            }
        }

        return points;
    }

    //Getter
    public static Vector3 GetterPointOnUnit(Vector3 targetDirection, float angle)
    {
        return PointSphereCap(Quaternion.LookRotation(targetDirection), angle);
    }

    //
    public static Vector3 PointSphereCap(Quaternion targetDirection, float angle)
    {
        float angleInRad = Random.Range(0.0f, angle) * Mathf.Deg2Rad;
        Vector2 PointOnCircle = (Random.insideUnitCircle.normalized) * Mathf.Sin(angleInRad);
        Vector3 V = new Vector3(PointOnCircle.x, PointOnCircle.y, Mathf.Cos(angleInRad));

        return targetDirection * V;
    }

    //random points on sphere to raise
    public static HashSet<Vector3> GenerateRandomPoints(int n, float radius)
    {
        HashSet<Vector3> points = new HashSet<Vector3>();

        for (int i = 0; i < n; i++)
        {
            Vector3 point = Random.onUnitSphere * radius;
            points.Add(point);
        }

        return points;
    }

    //mixed the point on sphere
    public static HashSet<Vector3> MixedPointCluster(int n, float radius)
    {
        int quotient = n / 3;
        int remainder = n % 3;

        int numBigPlates = quotient;
        int numSmallPlates = 2 * quotient + remainder;

        HashSet<Vector3> randomPoints = GenerateRandomPoints(numBigPlates, radius);
        HashSet<Vector3> randomClusters = GeneratePointClustersOnSphere(numSmallPlates, radius);
        HashSet<Vector3> points = new HashSet<Vector3>(randomPoints);
        points.UnionWith(randomClusters);

        return points;
    }


    //START Plate
    private List<FlatPlate> GeneratePlates()
    {
        Random.InitState(seed);
        HashSet<Vector3> points = MixedPointCluster(numberPlates, radius);

        HashSet<VoronoiCell3> voronoiCells = GenerateVoronoiCells(points);

        List<FlatPlate> plates = new List<FlatPlate>();

        foreach (VoronoiCell3 cell in voronoiCells)
        {
            FlatPlate plate = new FlatPlate(cell.sitePos.ToVector3(), (FlatPlate.PlateType)Random.Range(0, 2), cell.edges);
            plates.Add(plate);
        }

        foreach (FlatPlate plate in plates)
        {
            foreach (PlateEdge plateEdge in plate.edges)
            {
                // If this plate already has all its parents, move on.
                if (plateEdge.parents[1] != null)
                    continue;

                FlatPlate plateNeighbor = FindOtherEdgeParent(plateEdge, plate, plates);
                plateEdge.parents[1] = plateNeighbor;
            }
        }

        return plates;
    }

    //Edge of plates
    private FlatPlate FindOtherEdgeParent(PlateEdge cEdge, FlatPlate pPlate, List<FlatPlate> plates)
    {
        foreach (FlatPlate plate in plates)
        {
            if (plate == pPlate)
                continue;

            FlatPlate neighborPlate = null;

            foreach (PlateEdge plateEdge in plate.edges)
            {
                // Compare both ways.
                bool forwardSameEdge = cEdge.start == plateEdge.start && cEdge.end == plateEdge.end;
                bool backwardSameEdge = cEdge.start == plateEdge.end && cEdge.end == plateEdge.start;

                // Move on if there is no match
                if (!forwardSameEdge && !backwardSameEdge)
                    continue;

                neighborPlate = plate;
            }

            // If this plate is my neighbor, return it.
            if (neighborPlate != null)
                return neighborPlate;
        }

        // Code should not reach this point.
        throw new System.Exception("No neighbor found!");
    }

    //MESH breaks a few times
    private void SampleMesh(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;

        // Loop over all vertices.
        for (int p = 0; p < vertices.Length; p++)
        {
            Vector3 vertex = vertices[p];


            //Nearest plate to this vertex
            FlatPlate nearestPlate = plates[0];
            float shortestDistance = Vector3.Distance(vertex, nearestPlate.center);

            //Search through ALL the plates.
            foreach (FlatPlate plate in plates)
            {
                //Compare distances
                float currentDistance = Vector3.Distance(vertex, plate.center);
                if (currentDistance > shortestDistance)
                    continue;

                // This plate is the nearest plate.
                nearestPlate = plate;
                shortestDistance = currentDistance;
            }

            nearestPlate.vertices.Add(p);
        }
    }
}
