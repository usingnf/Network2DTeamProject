using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUILobby : MonoBehaviour
{
    public GameObject optionUI;
    public Slider slider;


    private void OnEnable() 
    {
        slider.value = PlayerPrefs.GetFloat("Volume");
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
