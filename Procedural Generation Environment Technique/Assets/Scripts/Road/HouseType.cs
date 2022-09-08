using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class HouseType
{
    [SerializeField]
    private GameObject[] prefabs;
    public int sizeReq;
    public int amount;
    public int amountPlaced;

    public GameObject GetPrefab()
    {
        amountPlaced++;
        if (prefabs.Length > 1)
        {
            var random = UnityEngine.Random.Range(0, prefabs.Length);
            return prefabs[random];
        }
        else
        {
            return prefabs[0];
        }
    }

    public bool IsHouseAllowed()
    {
        return amountPlaced < amount;
    }

    public void Reset()
    {
        amountPlaced = 0;
    }
}
