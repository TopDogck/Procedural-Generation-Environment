using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Town/Rule")]
public class Rule : ScriptableObject
{
    public string letter;
    [SerializeField]
    private string[] results = null;
    [SerializeField]
    private bool randomResults = false;

    public string GetResults()
    {
        if (randomResults)
        {
            int randomIndex = UnityEngine.Random.Range(0, results.Length);
            return results[randomIndex];
        }
        return results[0];
    }
}
