using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public GameObject   optionUI;
    public Slider       slider;


    private void OnEnable() 
    {
        slider.value = PlayerPrefs.GetFloat("Volume");
    }


    public void SetVolume()
    {
        PlayerPrefs.SetFloat("Volume", slider.value);
    }
}
