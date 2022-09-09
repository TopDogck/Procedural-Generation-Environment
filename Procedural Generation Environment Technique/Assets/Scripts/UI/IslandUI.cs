using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandUI : MonoBehaviour
{
    public int xSize = 40;
    public Text xSizeText;
    public Slider xSizeSlider;

    public int zSize = 40;
    public Text zSizeText;
    public Slider zSizeSlider;

    public float mFrequencies = 4;
    public Text mountainFText;
    public Slider MountainFSlider;

    public float mAmplitude = 2;
    public Text mountainAText;
    public Slider MountainASlider;

    public float octave = 1;
    public Text octaveText;
    public Slider octaveSlider;

    public float l1F = 1;
    public Text frequency1Text;
    public Slider frequency1Slider;

    public float l1A = 1;
    public Text amplitude1Text;
    public Slider amplitude1Slider;

    public float l2F = 1;
    public Text frequency2Text;
    public Slider frequency2Slider;

    public float l2A = 1;
    public Text amplitude2Text;
    public Slider amplitude2Slider;

    public TerrainGenerator terrainGenerator;
    public GameObject ocean;
    bool waterOn = true;
    public Text waterText;
    // Start is called before the first frame update
    void Start()
    {
        UpdateIsland();
        UpdateUI();
        waterOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateIsland();
        UpdateUI();
    }
    void UpdateIsland()
    {
        terrainGenerator.UpdateIsland(xSize, zSize, mFrequencies, mAmplitude, octave, l1F, l1A, l2F, l2A);
    }

    void UpdateUI()
    {
        xSizeSlider.onValueChanged.AddListener((x) => { xSize = (int)x; });
        xSizeText.text = xSize.ToString("000");

        zSizeSlider.onValueChanged.AddListener((z) => { zSize = (int)z; });
        zSizeText.text = zSize.ToString("000");

        MountainFSlider.onValueChanged.AddListener((mf) => { mFrequencies = mf; });
        mountainFText.text = mFrequencies.ToString("00.00");

        MountainASlider.onValueChanged.AddListener((ma) => { mAmplitude = ma; });
        mountainAText.text = mAmplitude.ToString("00.00");

        octaveSlider.onValueChanged.AddListener((o) => { octave = o; });
        octaveText.text = octave.ToString("0.00");

        frequency1Slider.onValueChanged.AddListener((f1) => { l1F = f1; });
        frequency1Text.text = l1F.ToString("0.00");

        amplitude1Slider.onValueChanged.AddListener((a1) => { l1A = a1; });
        amplitude1Text.text = l1A.ToString("00.00");

        frequency2Slider.onValueChanged.AddListener((f2) => { l2F = f2; });
        frequency2Text.text = l2F.ToString("0.00");

        amplitude2Slider.onValueChanged.AddListener((a2) => { l2A = a2; });
        amplitude2Text.text = l2A.ToString("00.00");
    }

    public void ChangeOceanVisibility()
    {
        waterOn = !waterOn;
        if (waterOn == true)
        {
            ocean.SetActive(false);
            waterText.text = "Ocean: OFF";
        }
        else if (waterOn == false)
        {
            ocean.SetActive(true);
            waterText.text = "Ocean: ON";
        }
    }
}
