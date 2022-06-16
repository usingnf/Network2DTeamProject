using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyPotal : MonoBehaviourPun
{
    public Player[] savePlayers;
    public Button playButton;
  
    int number = 0;
    bool[] playerReady;
    /* foreach (Player p in PhotonNetwork.PlayerList)
      {
          GameObject entry = Instantiate(playerEntryPrefab);
          entry.transform.SetParent(playerListContent.transform);
          entry.transform.localScale = Vector3.one;
          entry.GetComponent<PlayerEntry>().Initialize(p.ActorNumber, p.NickName);

          object isPlayerReady;
          if (p.CustomProperties.TryGetValue(GameData.PLAYER_READY, out isPlayerReady))
          {
              entry.GetComponent<PlayerEntry>().SetPlayerReady((bool)isPlayerReady);
          }

          playerListEntries.Add(p.ActorNumber, entry);
      }*/
    private void OnTriggerEnter2D(Collider2D other)
    {
        InPlayLobby lobby = new InPlayLobby();
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            playerReady[number] = true;
            savePlayers[number] = other.GetComponent<Player>();
            PhotonNetwork.Destroy(other.gameObject);
            number++;
            photonView.RPC("ReadyPlayerCount",PhotonNetwork.LocalPlayer);
            if (lobby.playerMax == ReadyPlayerCount())
            {
                if (PhotonNetwork.IsMasterClient)
                    playButton.gameObject.SetActive(true);
                else
                    return;
            }
        }
    }
    [PunRPC]
    public int ReadyPlayerCount()
    {
        int p = 0;
        if (playerReady[p])
            p++;
        else
            p--;
        return p;
    }
}
