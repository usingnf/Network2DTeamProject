using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUILobby : MonoBehaviour
{
    public Slider bgm_Slider;

    public GameObject optionUI;
    public Slider slider;

    private void OnEnable() 
    {
        slider.value = PlayerPrefs.GetFloat("Volume");
        bgm_Slider.value = PlayerPrefs.GetFloat("MusicVolume");
        SoundManager.Instance.SetMusicVolume(bgm_Slider.value);
    }
    
    public void SetBgmVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", bgm_Slider.value);
        SoundManager.Instance.SetMusicVolume(bgm_Slider.value);
    }

    public void SetVolume()
    {
        PlayerPrefs.SetFloat("Volume", slider.value);
    }

    public void ClickOptionExit()
    {
        LobbyManager.instance.SetActivePanel(LobbyManager.PANEL.Connect);
    }
}
