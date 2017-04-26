using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class SetSliders : MonoBehaviour {

    public Slider fxSlider;
    public Slider muSlider;

    public Toggle musicToggle; 
    public Toggle fxToggle;

    void Awake()
    {
        fxSlider.value = PlayerPrefs.GetFloat("FXVol");
        muSlider.value = PlayerPrefs.GetFloat("MusicVol"); 
    }

    void Update()
    {
        if (musicToggle.isOn == false)
        {
            muSlider.value = 0; 
        }

        if (fxToggle.isOn == false)
        {
            fxSlider.value = 0; 
        }
    }
}
