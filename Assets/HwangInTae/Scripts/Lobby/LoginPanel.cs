using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LoginPanel : MonoBehaviour
{
    public InputField playerNameInput;

    void Start()
    {
        playerNameInput.text = "난" + Random.Range(1000, 10000) + "번째데구리얌";
    }

    public void OnLoginButtonClicked()
    {
        string playerName = playerNameInput.text;

        if (playerName == "")
        {
            LobbyManager.instance.ShowError("Invalid Player Name");
            return;
        }

        PhotonNetwork.LocalPlayer.NickName = playerName;
        PhotonNetwork.ConnectUsingSettings();
    }
}
