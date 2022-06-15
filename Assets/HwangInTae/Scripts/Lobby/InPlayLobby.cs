using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class InPlayLobby : MonoBehaviourPun
{
    public Text curPlayer;
    public Text playerCount;

    CreateRoomPanel createRoom;

    public GameObject playerPrefab;

    public Transform spawnPos;

    int playerMax;
    int curPlayerCount;

    private void Start()
    {
        curPlayerCount = PhotonNetwork.CountOfPlayersInRooms;
    }

    public void EnterPlayer()
    {
        GameObject entry = PhotonNetwork.Instantiate("player플레이어 프리펩", spawnPos.position, playerPrefab.transform.rotation);
    }
    public void PlayButtonClicker()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }
    public void LeaveButtonClicker()
    {
        PhotonNetwork.LeaveRoom();
    }
}
