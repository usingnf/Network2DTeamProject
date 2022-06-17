using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InPlayLobby : MonoBehaviourPunCallbacks
{
    public Text curReayPlayer;
    public Text playerCount;

    int p = 0;

    public GameObject chatPrefab;
    public GameObject[] playerPrefab;

    public Transform spawnPos;

    public int playerMax;
    public int curReadyPlayerCount;

    private void Start()
    {
        if (null != chatPrefab)
        {
            chatPrefab.SetActive(true);
        }
    }

    public void EnterPlayer()
    {
        GameObject entry = PhotonNetwork.Instantiate("playerPrefab 프리펩", spawnPos.position, spawnPos.rotation);
        p++;
        if (p < playerPrefab.Length)
            p = 0;
    }
    public void GameStart()
    {
        PhotonNetwork.LoadLevel(2);
    }
    public void LeaveButtonClicker()
    {
        PhotonNetwork.LeaveRoom();
        
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
}
