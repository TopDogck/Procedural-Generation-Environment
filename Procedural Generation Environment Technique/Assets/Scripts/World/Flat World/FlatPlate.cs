using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Habrador_Computational_Geometry;

//Code is very messy
public class FlatPlate
{
    public Vector3 center;
    public List<PlateEdge> edges;

    //land or  ocean.
    public PlateType plateType;

    //All vertices in the sphere mesh belonging to this plate?
    public List<int> vertices = new List<int>();

    public enum PlateType
    {
        water = 0,
        ocean = 1
    }

    public FlatPlate(Vector3 center, PlateType plateType, List<VoronoiEdge3> voronoiEdges)
    {

        this.center = center;
        this.plateType = plateType;
        edges = new List<PlateEdge>();

        //Set edges.
        foreach (VoronoiEdge3 voronoiEdge in voronoiEdges)
        {
            PlateEdge plateEdge = new PlateEdge(voronoiEdge.p1.ToVector3(), voronoiEdge.p2.ToVector3(), this);
            edges.Add(plateEdge);
        }
    }
}

//Plate edges
public class PlateEdge
{
    public Vector3 start;
    public Vector3 end;
    public FlatPlate[] parents = new FlatPlate[2];

    public PlateEdge(Vector3 start, Vector3 end, FlatPlate parent)
    {
        this.start = start;
        this.end = end;
        parents[0] = parent;
    }
}
