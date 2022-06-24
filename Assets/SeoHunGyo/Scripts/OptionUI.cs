using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Slider slider;

    private void OnEnable() 
    {
        slider.value = PlayerPrefs.GetFloat("Volume");
    }


    public void SetVolume()
    {
        GameManager.Instance.SetVolume(slider.value);
    }
}
