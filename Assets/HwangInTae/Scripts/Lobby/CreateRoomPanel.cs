using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using ExitGames.Client.Photon;

public class CreateRoomPanel : MonoBehaviour
{
    public InputField roomNameInputField;
    public InputField roomPWInputField;

    public void OnCreateRoomCancelButtonClicked()   
    {
        LobbyManager.instance.SetActivePanel(LobbyManager.PANEL.Connect);
    }

    public void OnCreateRoomConfirmButtonClicked()
    {
        string roomName = roomNameInputField.text;
        if (roomName == "")
            roomName = "���µ������߾ӱ���" + Random.Range(1000, 10000);

        string roomPW = "";//�ʰ� ���ϴ� ��й�ȣ �ƹ��ų�

        string roomInfo = roomName + "_" + roomPW;

        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 4, IsVisible = true, IsOpen = true, };
        roomOptions.CustomRoomProperties = new Hashtable() 
        { 
            {"roominfo",roomInfo },     //roomName + PW
            {"password",roomPW },       //PW
            {"displayname",roomName }   //roomName
        };

        roomOptions.CustomRoomPropertiesForLobby = new string[] //�� �ù� ���ž� �迭�� �־����ϱ� �迭�� �����ؾ���
        {
            "roominfo",
            "password",
            "displayname",
        };
        //�̻����� ���°� ������ Ŀ����������Ƽ

        PhotonNetwork.CreateRoom(roomInfo, roomOptions, null);

        //if (roomName == "")
        //    roomName = "Room" + Random.Range(1000, 10000);

        //RoomOptions options = new RoomOptions { MaxPlayers = 4, IsVisible = true, IsOpen = true };
        //options.CustomRoomProperties = new Hashtable() { {"roomname"},roomName };
        //PhotonNetwork.CreateRoom(roomName + "_" + 111, options, null);
        //Debug.Log(PhotonNetwork.CurrentRoom.Name);

        //Hashtable props = new Hashtable() { { GameData.PRIVATE_ROOM, "Private" } };
        //PhotonNetwork.CurrentRoom.SetCustomProperties(props);

        //if (PhotonNetwork.CreateRoom(roomName + "_" + 111, options, null))
        //{
        //   /* PhotonNetwork.CurrentRoom.SetCustomProperties()*///TODO:�濡 ��й� �߰�
        //}
        //string displayName = "test";
        //string roomName = displayName + "_" + "1234";
        //RoomOptions roomOptions = new RoomOptions { MaxPlayers = 4, IsVisible = true, IsOpen = true };
        //roomOptions.CustomRoomProperties = new Hashtable() { { "displayname", displayName } };
        //roomOptions.CustomRoomPropertiesForLobby = new string[] { "displayname" };
        ////

        //PhotonNetwork.CreateRoom(roomName, roomOp��tions, null);
    }
}
