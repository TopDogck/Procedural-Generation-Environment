using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderTextUI : MonoBehaviour
{
    public Slider slider;
    public Text sliderText;
    void Start()
    {
        slider.onValueChanged.AddListener((s) => { sliderText.text = s.ToString("Plate Amount:" + "0.00"); });
    }
}
