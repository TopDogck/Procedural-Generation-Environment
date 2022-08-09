using System;
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

    internal static Vector3Int GetOffsetFromDirections(RoadDirections directions)
    {
        switch (directions)
        {
            case RoadDirections.Up:
                return new Vector3Int(0, 0, 1);
            case RoadDirections.Down:
                return new Vector3Int(0, 0, -1);
            case RoadDirections.Left:
                return Vector3Int.left;
            case RoadDirections.Right:
                return Vector3Int.right;
            default:
                break;
        }
        throw new System.Exception("No directions in" + directions); //error helper
    }

    public static RoadDirections GetReverseDirection(RoadDirections directions)
    {
        switch (directions)
        {
            case RoadDirections.Up:
                return RoadDirections.Down;
            case RoadDirections.Down:
                return RoadDirections.Up;
            case RoadDirections.Left:
                return RoadDirections.Right;
            case RoadDirections.Right:
                return RoadDirections.Left;
            default:
                break;
        }
        throw new System.Exception("No reverse directions in" + directions); //error helper
    }
}
