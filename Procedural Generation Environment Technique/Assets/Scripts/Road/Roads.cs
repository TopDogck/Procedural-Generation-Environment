using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roads : MonoBehaviour
{
    public GameObject roadForward, roadT, road3, road4, roadEnd;
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
}
