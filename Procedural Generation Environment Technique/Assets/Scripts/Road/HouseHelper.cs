using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseHelper : MonoBehaviour
{
    public HouseType[] houseTypes;
    public Dictionary<Vector3Int, GameObject> houseDic = new Dictionary<Vector3Int, GameObject>();

    public void PlaceHousesAroundRoad(List<Vector3Int> roadPos)
    {
        Dictionary<Vector3Int, RoadDirections> freeSpace = FindFreeSpaces(roadPos);

        List<Vector3Int> blockedPos = new List<Vector3Int>();
        foreach (var freeSpot in freeSpace)
        {
            if (blockedPos.Contains(freeSpot.Key))
            {
                continue;
            }
            var roation = Quaternion.identity;
            switch (freeSpot.Value)
            {
                case RoadDirections.Up:
                    roation = Quaternion.Euler(0, 90, 0);
                    break;
                case RoadDirections.Down:
                    roation = Quaternion.Euler(0, -90, 0);
                    break;
                //case RoadDirections.Left: //House is already facing left by default
                //    roation = Quaternion.Euler(0, 0, 0);
                //    break;
                case RoadDirections.Right:
                    roation = Quaternion.Euler(0, 180, 0);
                    break;
                default:
                    break;
            }

            for (int h = 0; h < houseTypes.Length; h++)
            {
                if (houseTypes[h].amount == -1)
                {
                    var house = SpawnHouse(houseTypes[h].GetPrefab(), freeSpot.Key, roation);
                    houseDic.Add(freeSpot.Key, house);
                    break;
                }
                if (houseTypes[h].IsHouseAllowed())
                {
                    if (houseTypes[h].sizeReq > 1)
                    {
                        var halfSize = Mathf.FloorToInt(houseTypes[h].sizeReq / 2.0f); //floor to int will get true half value
                        List<Vector3Int> tempPosBlocked = new List<Vector3Int>();
                        if (DoesHouseFit(halfSize, freeSpace, freeSpot,blockedPos, ref tempPosBlocked))
                        {
                            blockedPos.AddRange(tempPosBlocked);
                            var house = SpawnHouse(houseTypes[h].GetPrefab(), freeSpot.Key, roation);
                            houseDic.Add(freeSpot.Key, house);
                            break;
                        }        
                    }
                    else
                    {
                        var house = SpawnHouse(houseTypes[h].GetPrefab(), freeSpot.Key, roation); //Instantiate(prefab, freeSpot.Key, roation, transform);
                        houseDic.Add(freeSpot.Key, house);
                        break;
                    }
                }
            }
        }
    }

    private bool DoesHouseFit(int halfSize, Dictionary<Vector3Int, RoadDirections> freeSpace, KeyValuePair<Vector3Int, RoadDirections> freeSpot, List<Vector3Int> blockedPos, ref List<Vector3Int> tempPosBlocked)
    {
        Vector3Int direction = Vector3Int.zero;
        //so houses dont go onto the roads but stay on the side of the road
        if (freeSpot.Value == RoadDirections.Down || freeSpot.Value == RoadDirections.Up)
        {
            direction = Vector3Int.right;
        }
        else
        {
            direction = new Vector3Int(0, 0, 1);
        }
        for (int s = 1; s <= halfSize; s++)
        {
            var pos1 = freeSpot.Key + direction * s;
            var pos2 = freeSpot.Key - direction * s;
            if (!freeSpace.ContainsKey(pos1) || !freeSpace.ContainsKey(pos2) || blockedPos.Contains(pos1) || blockedPos.Contains(pos2))
            {
                // house will not fit
                return false;
            }
            tempPosBlocked.Add(pos1);
            tempPosBlocked.Add(pos2);
        }
        //either pos 1 or pos 2 will fit
        return true;

    }

    private GameObject SpawnHouse(GameObject houseprefab, Vector3Int pos, Quaternion roation)
    {
        var newHouse = Instantiate(houseprefab, pos, roation, transform);
        return newHouse;
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

    public void Reset()
    {
        foreach (var house in houseDic.Values)
        {
            Destroy(house);
        }
        houseDic.Clear();
        foreach (var types in houseTypes)
        {
            types.Reset();
        }
    }
}
