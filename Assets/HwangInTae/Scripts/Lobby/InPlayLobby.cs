using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InPlayLobby : MonoBehaviourPunCallbacks
{
    public Text curPlayer;
    public Text playerCount;

    int p = 0;
    CreateRoomPanel createRoom;

    public GameObject[] playerPrefab;

    public Transform spawnPos;

    int playerMax;
    int curPlayerCount;

    private void Start()
    {
        curPlayerCount = PhotonNetwork.CountOfPlayersInRooms;
    }

    public void EnterPlayer()
    {
        GameObject entry = PhotonNetwork.Instantiate("playerPrefab 프리펩", spawnPos.position, spawnPos.rotation);
        p++;
        if (p < playerPrefab.Length)
            p = 0;
    }
    public void PlayButtonClicker()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }
    public void LeaveButtonClicker()
    {
        SceneManager.LoadScene("LobbyScene");
        PhotonNetwork.LeaveRoom();
    }
}
