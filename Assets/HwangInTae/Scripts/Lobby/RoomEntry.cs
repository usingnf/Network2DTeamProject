using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using ExitGames.Client.Photon;
using System.Collections;

public class RoomEntry : MonoBehaviour
{
    public Text roomNameText;
    public Text roomPlayersText;
    public Button joinRoomButton;
    public Sprite lockImage;
    public InputField PWfield;

    private string roomName;
    private string PWkey;
    private string roomPW=null;

    public void Initialize(string name,string PW, byte currentPlayers, byte maxPlayers)
    {
        roomName = name;

        roomNameText.text = name;
        roomPlayersText.text = currentPlayers + " / " + maxPlayers;

        joinRoomButton.enabled = currentPlayers < maxPlayers;

        if(PW !=null)
        roomPW = PW;
    }

    public void JoinRoom()
    {
        if (roomPW != null)
        {
            PWfield.ActivateInputField();
            Debug.Log("Password입력");

            //if (roomPW == PWfield.text)
            //{
                PhotonNetwork.LeaveLobby();
                PhotonNetwork.JoinRoom(roomName+"_"+PWfield.text);
            //}
            //else
            //{
            //    Debug.Log("PW오류!");
            //    return;
            //}
        }
        else
        {
            PhotonNetwork.LeaveLobby();
            PhotonNetwork.JoinRoom(roomName);
        }
    }
}
