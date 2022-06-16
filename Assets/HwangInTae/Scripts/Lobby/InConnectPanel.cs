using UnityEngine;
using Photon.Pun;

public class InConnectPanel : MonoBehaviour
{
    public void OnCreateRoomButtonClicked()
    {
        LobbyManager.instance.SetActivePanel(LobbyManager.PANEL.CreateRoom);
    }

    public void OnRandomMatchingButtonClicked()
    {
        PhotonNetwork.JoinRandomRoom();
        PhotonNetwork.LoadLevel("PlayLobbyScene");
        Debug.Log("방랜덤들어감");
    }

    public void OnLobbyButtonClicked()
    {
        PhotonNetwork.JoinLobby();
        LobbyManager.instance.SetActivePanel(LobbyManager.PANEL.Lobby);
    }

    public void OnLogoutButtonClicked()
    {
        PhotonNetwork.Disconnect();
        LobbyManager.instance.SetActivePanel(LobbyManager.PANEL.Login);
    }
    public void ExitGameClicked()
    {
        Application.Quit();
    }
    public void OnOptionButtonClicked()
    {
        LobbyManager.instance.SetActivePanel(LobbyManager.PANEL.Option);
    }
}
