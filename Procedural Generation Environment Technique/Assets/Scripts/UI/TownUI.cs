using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class TownUI : MonoBehaviour
{
    public int limitAmount;
    public Text limitText;
    public Slider limitSlider;

    public string axiom;
    public InputField axiomInput;
    public Text axiomText;
    public bool changeAxiom = false;
    public GameObject inputField;

    public bool ignoreRule;
    public Text ignoreRuleText;
    public LsystemGeneration lsystem;
    // Start is called before the first frame update
    void Start()
    {
        UpdateTown();
        axiomText.text = "[F]--[F]--[F]--[F]--F";
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTown();
        UpdateUI();
    }

    void UpdateTown()
    {
        lsystem.UpdateTown(limitAmount, axiom, ignoreRule);
    }

    public void ChangeAxiomValue()
    {
        changeAxiom = !changeAxiom;
    }
    public void ChangeRandomRule()
    {
        ignoreRule = !ignoreRule;

        if (ignoreRule == true)
        {
            ignoreRuleText.text = "Random Rule Ignore: Yes";
        }
        else if (ignoreRule == false)
        {
            ignoreRuleText.text = "Random Rule Ignore: No";
        }
    }

    void UpdateUI()
    {
        if (changeAxiom == true)
        {
            inputField.SetActive(true);
            axiomInput.onValueChanged.AddListener(inputValueChanged);
            axiom = axiomInput.text;
            axiomText.text = axiom;
        }
        else if (changeAxiom == false)
        {
            inputField.SetActive(false);
            axiom = "[F]--[F]--[F]--[F]--F";
            axiomText.text = "[F]--[F]--[F]--[F]--F";
        }

        limitSlider.onValueChanged.AddListener((l) => { limitAmount = (int)l; });
        limitText.text = limitAmount.ToString("00");
    }

    void inputValueChanged(string attemptText)
    {
        axiomInput.text = CleanInput(attemptText);

    }

    static string CleanInput(string stringAttempt)
    {
        //Only allow these input letter and symbols
        return Regex.Replace(stringAttempt,
              @"[^]F+[-]", "");
    }

    //void OnDisable()
    //{
    //    //Un-Register InputField Events
    //    axiomInput.onValueChanged.RemoveAllListeners();
    //}
}
