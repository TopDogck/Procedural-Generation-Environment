using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Habrador_Computational_Geometry;
public class TectonicPlate
    {
        public Vector3 center;
        public List<TectonicPlateEdge> edges;
        
        //Which plate type continental or oceanic.
        public PlateType plateType;

        //All vertices in the sphere mesh for the plate.
        public List<int> vertices = new List<int>();

        public enum PlateType
        {
            oceanic = 0,
            continental = 1
        }

        public TectonicPlate(Vector3 center, PlateType plateType, List<VoronoiEdge3> voronoiEdges)
        {

            this.center = center;
            this.plateType = plateType;
            edges = new List<TectonicPlateEdge>();

            // Set edges.
            foreach (VoronoiEdge3 voronoiEdge in voronoiEdges)
            {
                TectonicPlateEdge plateEdge = new TectonicPlateEdge(voronoiEdge.p1.ToVector3(), voronoiEdge.p2.ToVector3(), this);
                edges.Add(plateEdge);
            }
        }
    }