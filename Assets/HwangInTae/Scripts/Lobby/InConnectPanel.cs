using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class InConnectPanel : MonoBehaviour
{
    public void OnCreateRoomButtonClicked()
    {
        LobbyManager.instance.SetActivePanel(LobbyManager.PANEL.CreateRoom);
    }

    public void OnRandomMatchingButtonClicked()
    {

        ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable() { { "password" , "" } };
        PhotonNetwork.JoinRandomRoom(customProperties, 0, MatchmakingMode.FillRoom, null, null);
        //PhotonNetwork.JoinRandomRoom();

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
    public void OptionPanelClicked()
    {
        LobbyManager.instance.SetActivePanel(LobbyManager.PANEL.Option);
    }
}
