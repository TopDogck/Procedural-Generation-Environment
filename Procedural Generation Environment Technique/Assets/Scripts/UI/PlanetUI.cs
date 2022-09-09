using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetUI : MonoBehaviour
{
    public int seed;
    int tempSeed;
    public InputField seedInput;
    public Text seedText;
    public Slider seedSlider;

    public int numberOfPlates;
    int tempPlates;
    public Text plateText;
    public Slider plateSlider;

    public float radiusSize;
    float tempRadius;
    public Text radiusText;
    public Slider radiusSlider;

    public float amplitude;
    float tempAmplitude;
    public Text amplitudeText;
    public Slider amplitudeSlider;

    public bool randomColour = true;
    bool tempColour = false;
    public Text colourText;

    public List<TectonicPlate> plates;
    public PlanetCrustGenerator crustGenerator;

    float changeDelay = 0.1f;
    float timePassed = 0;

    // Start is called before the first frame update
    void Start()
    {
        UpdatePlanet();
    }

    // Update is called once per frame
    void Update()
    {
        //Value changed
        if (tempSeed != seed || tempPlates != numberOfPlates || tempRadius != radiusSize || tempAmplitude != amplitude || tempColour != randomColour)
        {
            tempSeed = seed; 
            tempPlates = numberOfPlates; 
            tempRadius = radiusSize; 
            tempAmplitude = amplitude; 
            tempColour = randomColour;

            //Update
            UpdatePlanet();

            //Time past
            //timePassed += Time.deltaTime;
            //if (timePassed > changeDelay)
            //{
            //}
        }
        //UI stuff
        UpdateUI();
        //Text.text = seed.ToString("00"+": Seed");
    }

    void UpdatePlanet()
    {
        crustGenerator.UpdatePlanet(seed, numberOfPlates, radiusSize, amplitude, randomColour);
    }

    void UpdateUI()
    {
        seedInput.onValueChanged.AddListener((s) => { int.TryParse(s, out seed); }); 
        seedText.text = seed.ToString("000");

        plateSlider.onValueChanged.AddListener((p) => { numberOfPlates = (int)p; });
        plateText.text = numberOfPlates.ToString("000");

        radiusSlider.onValueChanged.AddListener((r) => { radiusSize = r; });
        radiusText.text = radiusSize.ToString("000.00");

        amplitudeSlider.onValueChanged.AddListener((a) => { amplitude = a; });
        amplitudeText.text = amplitude.ToString("000.00");

        if (randomColour == true)
        {
            //Random colour
            colourText.text = "Random Colour: Yes";
        }
        else
        {
            //else Earth colours
            colourText.text = "Random Colour: No";
        }
    }

    public void ChangeButtonColour()
    {
        randomColour = !randomColour;
    }
}
