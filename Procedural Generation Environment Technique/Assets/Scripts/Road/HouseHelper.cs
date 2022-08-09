using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseHelper : MonoBehaviour
{
    public GameObject prefab;
    public Dictionary<Vector3Int, GameObject> houseDic = new Dictionary<Vector3Int, GameObject>();

    public void PlaceHousesAroundRoad(List<Vector3Int> roadPos)
    {
        Dictionary<Vector3Int, RoadDirections> freeSpace = FindFreeSpaces(roadPos);
        foreach (var freeSpot in freeSpace)
        {
            var roation = Quaternion.identity;
            switch (freeSpot.Value)
            {
                case RoadDirections.Up:
                    roation = Quaternion.Euler(0, 90, 0);
                    break;
                case RoadDirections.Down:
                    roation = Quaternion.Euler(0, -90, 0);
                    break;
                //case RoadDirections.Left:
                //    roation = Quaternion.Euler(0, 0, 0);
                //    break;
                case RoadDirections.Right:
                    roation = Quaternion.Euler(0, 180, 0);
                    break;
                default:
                    break;
            }
            Instantiate(prefab, freeSpot.Key, roation, transform);
        }
    }

    private Dictionary<Vector3Int, RoadDirections> FindFreeSpaces(List<Vector3Int> roadPos) //algorith
    {
        Dictionary<Vector3Int, RoadDirections> freeSpaces = new Dictionary<Vector3Int, RoadDirections>();
        foreach (var postion in roadPos)
        {
            var nextDirections = RoadPlacement.FindNext(postion, roadPos);
            foreach (RoadDirections directions in Enum.GetValues(typeof(RoadDirections)))
            {
                if (nextDirections.Contains(directions) == false)
                {
                    var newPos = postion + RoadPlacement.GetOffsetFromDirections(directions);
                    if (freeSpaces.ContainsKey(newPos))
                    {
                        continue;
                    }
                    freeSpaces.Add(newPos, RoadPlacement.GetReverseDirection(directions));
                }
            }
        }
        return freeSpaces;
    }
}
