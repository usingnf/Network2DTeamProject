using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyPotal : MonoBehaviourPun
{
    public GameObject[] savePlayer;
    public Text startCount;

    int enterPlayers = 0;
    bool ischeck = false;
    int readyPlayer = 0;

    Collider2D playerColl;
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            enterPlayers++;
            playerColl = other;
            if (enterPlayers > 0)
                ischeck = true;
        }
    }
    private void Update()
    {
        if (ischeck)
            Ready();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") { 
            enterPlayers--;
            if (enterPlayers <= 0)
                ischeck = false;
            playerColl = null;
        }
    }
    public void Ready()
    {
        if (Input.GetKey(KeyCode.R))
        {
            readyPlayer++;
            HInLobby.Instance.PlayersLoadLevel();
            //savePlayer[readyPlayer] = other.gameObject;
            Debug.Log(PhotonNetwork.CurrentRoom.MaxPlayers);
            if (/*PhotonNetwork.CurrentRoom.MaxPlayers*/1 == readyPlayer) 
                StartCoroutine(StartCountDown());
            PhotonNetwork.Destroy(playerColl.gameObject);
        }
    }
    private IEnumerator StartCountDown()
    {

        HInLobby.Instance.PrintInfo("All Player Loaded, Start Count Down");
        yield return new WaitForSeconds(1.0f);

        for (int i = GameData.COUNTDOWN; i > 0; i--)
        {
            HInLobby.Instance.PrintInfo("Count Down " + i);
            yield return new WaitForSeconds(1.0f);
        }
        HInLobby.Instance.PrintInfo("Start Game!");
        yield return new WaitForSeconds(0.3f);
        if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel(2);

        //int playerNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber();
        //PhotonNetwork.Instantiate("TestPlayer", spawnPos[playerNumber].position, spawnPos[playerNumber].rotation, 0);
    }
   
}
