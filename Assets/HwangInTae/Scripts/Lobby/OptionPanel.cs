using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    public Toggle soundCheack;
    AudioSource audioSource;

    public void ExitButtonClicked()
    {
        LobbyManager.instance.SetActivePanel(LobbyManager.PANEL.Connect);
    }
    public void SoundToggleClicked()
    {
        if (soundCheack.isOn)
        {
            audioSource.volume = 1.0f;
        }
        else
            audioSource.volume = 0f;
    }
}
