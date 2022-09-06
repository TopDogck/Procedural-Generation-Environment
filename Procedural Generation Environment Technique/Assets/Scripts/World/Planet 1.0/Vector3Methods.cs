using Habrador_Computational_Geometry;
using MIConvexHull;
using System.Collections.Generic;
using UnityEngine;

//Similar to Convex Hull MyVector3 
public static class Vector3Methods
{
    public static HashSet<MyVector3> ToMyVector3(this HashSet<Vector3> vectors)
    {
        HashSet<MyVector3> myVectors = new HashSet<MyVector3>();
        foreach (Vector3 vector in vectors)
        {
            myVectors.Add(vector.ToMyVector3());
        }
        return myVectors;
    }
}
