using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roads : MonoBehaviour
{
    public GameObject roadForward, roadTurn, roadTri, roadCross, roadEnd;
    Dictionary<Vector3Int, GameObject> roadDic = new Dictionary<Vector3Int, GameObject>();
    HashSet<Vector3Int> fixRoads = new HashSet<Vector3Int>();

    public void PlaceStreetPos(Vector3 startPos, Vector3Int direction, int length)
    {
        var roation = Quaternion.identity;
        if (direction.x == 0)
        {
            roation = Quaternion.Euler(0, 90, 0);
        }
        for (int i = 0; i < length; i++)
        {
            var postion = Vector3Int.RoundToInt(startPos + direction * i);
            if (roadDic.ContainsKey(postion))
            {
                continue;
            }
            var road = Instantiate(roadForward, postion, roation, transform);
            roadDic.Add(postion, road);
            if (i == 0 || i == length - 1)
            {
                fixRoads.Add(postion);
            }
        }
    }

    public void FixRoad()
    {
        foreach (var postion in fixRoads)
        {
            List<RoadDirections> nextDirections = RoadPlacement.FindNext(postion, roadDic.Keys);

            Quaternion roation = Quaternion.identity;

            if (nextDirections.Count == 1)// dead end road
            {
                Destroy(roadDic[postion]);
                if (nextDirections.Contains(RoadDirections.Down))
                {
                    roation = Quaternion.Euler(0, 90, 0);
                }
                else if (nextDirections.Contains(RoadDirections.Left))
                {
                    roation = Quaternion.Euler(0, 180, 0);
                }
                else if (nextDirections.Contains(RoadDirections.Up))
                {
                    roation = Quaternion.Euler(0, -90, 0);
                }
                roadDic[postion] = Instantiate(roadEnd, postion, roation, transform);
            }
            else if (nextDirections.Count == 2) // L turn road
            {
                if (nextDirections.Contains(RoadDirections.Up) && nextDirections.Contains(RoadDirections.Down) || nextDirections.Contains(RoadDirections.Right) && nextDirections.Contains(RoadDirections.Left))
                {
                    continue;
                }
                Destroy(roadDic[postion]);
                if (nextDirections.Contains(RoadDirections.Up) && (nextDirections.Contains(RoadDirections.Right)))
                {
                    roation = Quaternion.Euler(0, 90, 0);
                }
                else if (nextDirections.Contains(RoadDirections.Right) && (nextDirections.Contains(RoadDirections.Down)))
                {
                    roation = Quaternion.Euler(0, 180, 0);
                }
                else if (nextDirections.Contains(RoadDirections.Down) && (nextDirections.Contains(RoadDirections.Left)))
                {
                    roation = Quaternion.Euler(0, -90, 0);
                }
                roadDic[postion] = Instantiate(roadTurn, postion, roation, transform);
            }
            else if (nextDirections.Count == 3) // 3 way road
            {
                Destroy(roadDic[postion]);
                if (nextDirections.Contains(RoadDirections.Right) 
                    && (nextDirections.Contains(RoadDirections.Down)) 
                    && (nextDirections.Contains(RoadDirections.Left)))
                {
                    roation = Quaternion.Euler(0, 90, 0);
                }
                else if (nextDirections.Contains(RoadDirections.Down)
                    && (nextDirections.Contains(RoadDirections.Left))
                    && (nextDirections.Contains(RoadDirections.Up)))
                {
                    roation = Quaternion.Euler(0, 180, 0);
                }
                else if (nextDirections.Contains(RoadDirections.Left)
                    && (nextDirections.Contains(RoadDirections.Up))
                    && (nextDirections.Contains(RoadDirections.Right)))
                {
                    roation = Quaternion.Euler(0, -90, 0);
                }
                roadDic[postion] = Instantiate(roadTri, postion, roation, transform);
            }
            else // 4 way road
            {
                Destroy(roadDic[postion]);
                roadDic[postion] = Instantiate(roadCross, postion, roation, transform);
            }
        }
    }
}
