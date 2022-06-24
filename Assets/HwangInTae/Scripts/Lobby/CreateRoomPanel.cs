using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

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
            roomName = "나는데구리야앙기모디" + Random.Range(1000, 10000);
        string roomPW = roomPWInputField.text;

        if (roomPW == "")//비밀번호방 아닐시
        {
            string roomInfo = roomName + "_" + roomPW;

            RoomOptions roomOptions = new RoomOptions { MaxPlayers = 4, IsVisible = true, IsOpen = true };

            roomOptions.CustomRoomProperties = new Hashtable()
        {
            {"roominfo",roomInfo },     //roomName + PW
            {"password",roomPW },       //PW
            {"displayname",roomName }   //roomName
        };

            roomOptions.CustomRoomPropertiesForLobby = new string[]
            {
            "roominfo",
            "password",
            "displayname",
            };//TODO:정보 좀 더 알아보기
            Debug.Log(roomPW);
            PhotonNetwork.CreateRoom(roomInfo, roomOptions, null);
        }
        else//비밀번호가 달린 방일시
        {
            string roomInfo = roomName + "_" + roomPW;

            RoomOptions roomOptions = new RoomOptions { MaxPlayers = 4, IsVisible = true, IsOpen = true, };
            roomOptions.CustomRoomProperties = new Hashtable()
        {
            {"roominfo",roomInfo },     //roomName + PW
            {"password",roomPW },       //PW
            {"displayname",roomName }   //roomName
        };

            roomOptions.CustomRoomPropertiesForLobby = new string[]
            {
            "roominfo",
            "password",
            "displayname",
            };
            //이새끼가 가는것 개같은 커스텀프로퍼티
            PhotonNetwork.CreateRoom(roomInfo, roomOptions, null);
        }
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
        //   /* PhotonNetwork.CurrentRoom.SetCustomProperties()*///TODO:방에 비밀방 추가
        //}
        //string displayName = "test";
        //string roomName = displayName + "_" + "1234";
        //RoomOptions roomOptions = new RoomOptions { MaxPlayers = 4, IsVisible = true, IsOpen = true };
        //roomOptions.CustomRoomProperties = new Hashtable() { { "displayname", displayName } };
        //roomOptions.CustomRoomPropertiesForLobby = new string[] { "displayname" };
        ////

        //PhotonNetwork.CreateRoom(roomName, roomOpㅇtions, null);
    }
}
