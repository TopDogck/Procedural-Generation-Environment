using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LsystemGeneration : MonoBehaviour
{
    public Rule[] rules;
    public string axiom; //root sentence
    [Range(0,10)]
    public int limt = 1; //iteration limt

    public bool randomRuleIgnore = true; //random rule ignore modifer
    [Range(0, 1)]
    public float changeIgnoreRule = 0.3f; 

    private void Start()
    {
        Debug.Log(GenerateAxiom());   
    }

    public string GenerateAxiom(string word = null)
    {
        if (word == null)
        {
            word = axiom;    
        }
        return GrownRecursive(word);
    }

    private string GrownRecursive(string word, int currentIndex = 0) //iteration index
    {
        if (currentIndex >= limt)
        {
            return word;
        }

        StringBuilder newWord = new StringBuilder();

        foreach (var c in word) //memory saver
        {
            newWord.Append(c);
            ProcessRulesRecursivelly(newWord, c, currentIndex);
        }

        return newWord.ToString();
    }

    private void ProcessRulesRecursivelly(StringBuilder newWord, char c, int currentIndex)
    {
        foreach (var rule in rules)
        {
            if (rule.letter == c.ToString())
            {
                if (randomRuleIgnore && currentIndex > 1)
                {
                    if (UnityEngine.Random.value < changeIgnoreRule)
                    {
                        return;
                    }
                }
                newWord.Append(GrownRecursive(rule.GetResults(),currentIndex + 1));
            }
        }
    }
}
