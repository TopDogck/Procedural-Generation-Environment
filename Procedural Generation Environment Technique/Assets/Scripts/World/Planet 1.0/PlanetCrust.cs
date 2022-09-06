using Habrador_Computational_Geometry;
using System.Collections.Generic;
using UnityEngine;

    public class PlanetCrust
    {
        public int numberOfPlates;
        public float radius;
        public int randomSeed;

        public List<TectonicPlate> plates;

        public PlanetCrust(int numberOfPlates, float radius, Mesh sampleMesh, int randomSeed = 42)
        {
            this.numberOfPlates = numberOfPlates;
            this.radius = radius;
            this.randomSeed = randomSeed;
            plates = GeneratePlates();
            SampleMesh(sampleMesh);
        }

        private HashSet<VoronoiCell3> GenerateVoronoiCells(HashSet<Vector3> points)
        {
            HashSet<MyVector3> convertedPoints = points.ToMyVector3();
            List<MyVector3> unitBoundingBox = new List<MyVector3>(convertedPoints);
            Normalizer3 normalizer = new Normalizer3(unitBoundingBox);

            //Normalize all points.
            HashSet<MyVector3> normPoints = normalizer.Normalize(convertedPoints);

            //Generate a convex hull using the normalized points
            HalfEdgeData3 convexHull = IterativeHullAlgorithm3D.GenerateConvexHull(normPoints, false);
            //Generate a voronoi cells from convex hull
            HashSet<VoronoiCell3> voronoiCells = Delaunay3DToVoronoiAlgorithm.GenerateVoronoiDiagram(convexHull);

            // De-normalize the voronoi diagram.
            voronoiCells = normalizer.UnNormalize(voronoiCells);

            return voronoiCells;
        }

        private List<TectonicPlate> GeneratePlates()
        {
            Random.InitState(randomSeed);
            HashSet<Vector3> points = OnUnitSphereMethods.MixedPointClustersOnSphere(numberOfPlates, radius);
            
            HashSet<VoronoiCell3> voronoiCells = GenerateVoronoiCells(points);

            List<TectonicPlate> plates = new List<TectonicPlate>();

            foreach (VoronoiCell3 cell in voronoiCells)
            {
                TectonicPlate plate = new TectonicPlate(cell.sitePos.ToVector3(), (TectonicPlate.PlateType)Random.Range(0, 2), cell.edges);
                plates.Add(plate);
            }

            foreach (TectonicPlate plate in plates)
            {
                foreach (TectonicPlateEdge plateEdge in plate.edges)
                {
                    // If this plate already has all its parents, move on.
                    if (plateEdge.parents[1] != null)
                        continue;

                    TectonicPlate plateNeighbor = FindOtherEdgeParent(plateEdge, plate, plates);
                    plateEdge.parents[1] = plateNeighbor;
                }
            }

            return plates;
        }

        private TectonicPlate FindOtherEdgeParent(TectonicPlateEdge childEdge, TectonicPlate parentPlate, List<TectonicPlate> plates)
        {

            foreach (TectonicPlate plate in plates)
            {
                if (plate == parentPlate)
                    continue;

                TectonicPlate neighborPlate = null;

                foreach (TectonicPlateEdge plateEdge in plate.edges)
                {
                    //Both ways incase
                    bool forwardSameEdge = childEdge.start == plateEdge.start && childEdge.end == plateEdge.end;
                    bool backwardSameEdge = childEdge.start == plateEdge.end && childEdge.end == plateEdge.start;

                    //Carry on
                    if (!forwardSameEdge && !backwardSameEdge)
                        continue;

                    neighborPlate = plate;
                }

                //Neighbor will return.
                if (neighborPlate != null)
                    return neighborPlate;
            }

            //Just in case?
            throw new System.Exception("No neighbor");
        }

        private void SampleMesh(Mesh mesh)
        {
            Vector3[] vertices = mesh.vertices;

            // Loop over all vertices.
            for (int v = 0; v < vertices.Length; v++)
            {
                Vector3 vertex = vertices[v];


                //Nearest plate to the vertex.
                TectonicPlate nearestPlate = plates[0];
                float smallestDistance = Vector3.Distance(vertex, nearestPlate.center);

                //Go through all plates.
                foreach (TectonicPlate plate in plates)
                {
                    //Compare distances
                    float currentDistance = Vector3.Distance(vertex, plate.center);
                    if (currentDistance > smallestDistance)
                        continue;

                    //Nearest plate.
                    nearestPlate = plate;
                    smallestDistance = currentDistance;
                }

                nearestPlate.vertices.Add(v);
            }
        }
    }