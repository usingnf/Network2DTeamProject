using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public AudioSource bgm_Source;
    public Slider bgm_Slider;
    public GameObject bgm_GameObject;

    public GameObject optionUI;
    public Slider slider;

    private void Start()
    {
        bgm_Source = bgm_GameObject.GetComponent<AudioSource>();
        bgm_Source.volume = 0.3f;
        bgm_Slider.value = bgm_Source.volume;
    }

    private void OnEnable()
    {
        slider.value = PlayerPrefs.GetFloat("Volume");
    }

    public void SetBgmVolume()
    {
        bgm_Source.volume = bgm_Slider.value;
    }

    public void SetVolume()
    {
        PlayerPrefs.SetFloat("Volume", slider.value);
    }
}
