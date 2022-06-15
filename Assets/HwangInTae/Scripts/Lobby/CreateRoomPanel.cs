using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoomPanel : MonoBehaviour
{
    public InputField roomNameInputField;

    public void OnCreateRoomCancelButtonClicked()
    {
        LobbyManager.instance.SetActivePanel(LobbyManager.PANEL.Connect);
    }

    public void OnCreateRoomConfirmButtonClicked()
    {
        string roomName = roomNameInputField.text;

        if (roomName == "")
            roomName = "Room" + Random.Range(1000, 10000);

        byte maxPlayer = 4;
        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayer };
        Debug.Log("이방의 맥스인원"+options.MaxPlayers);
        PhotonNetwork.CreateRoom(roomName, options, null);
    }
}
