using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoadPlacement
{
    public static List<RoadDirections> FindNext(Vector3Int position, ICollection<Vector3Int> collection)
    {
        List<RoadDirections> nextDirections = new List<RoadDirections>();
        if (collection.Contains(position + Vector3Int.right))
        {
            nextDirections.Add(RoadDirections.Right);
        }
        if (collection.Contains(position - Vector3Int.right))
        {
            nextDirections.Add(RoadDirections.Left); ;
        }
        if (collection.Contains(position + new Vector3Int(0,0,1)))
        {
            nextDirections.Add(RoadDirections.Up);
        }
        if (collection.Contains(position - new Vector3Int(0, 0, 1)))
        {
            nextDirections.Add(RoadDirections.Down); ;
        }
        return nextDirections;
    }
}
