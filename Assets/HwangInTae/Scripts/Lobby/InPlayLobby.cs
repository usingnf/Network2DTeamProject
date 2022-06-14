using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class InPlayLobby : MonoBehaviourPun
{
    public Text curPlayer;
    public Text playerCount;

    public GameObject playerPrefab;

    public Button playButton;
    public Button leaveButton;

    public Transform spawnPos;

    int playerMax;
    int curPlayerCount;

    private void Start()
    {

    }

    private void EnterPlayer()
    {
        GameObject entry = PhotonNetwork.Instantiate("player플레이어 프리펩", spawnPos.position, playerPrefab.transform.rotation);
    }
    private void PlayButtonClicker()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }
    private void LeaveButtonClicker()
    {
        PhotonNetwork.LeaveRoom();
    }
}
