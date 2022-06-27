using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class HInLobby : MonoBehaviourPunCallbacks
{
    public static HInLobby Instance { get; private set; }

    public Transform potalPos;
    public Text infoText;
    public Transform spawnPos;
    public int readyPlayer = 0;

    private void Awake()
    {
        Instance = this;
    }


    #region PHOTON CALLBACK
       
    public void Start()
    {
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable() { { GameData.PLAYER_IN, true } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        PhotonNetwork.Instantiate("PlayerCharacter", spawnPos.position, spawnPos.rotation, 0);
    }

    #region PHOTON CALLBACK

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected : " + cause.ToString());
        SceneManager.LoadScene("LobbyScene");
    }

    public override void OnLeftRoom()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene("LobbyScene");
            return;
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        PrintInfo(readyPlayer/2 + " / " + PhotonNetwork.CurrentRoom.MaxPlayers);
        if (changedProps.ContainsKey(GameData.PLAYER_LOAD))
        {
        }
    }

    #endregion PHOTON CALLBACK

     

    private bool CheckAllPlayerLoadLevel()
    {
        return PlayersLoadLevel() == PhotonNetwork.PlayerList.Length;
    }

    public int PlayersLoadLevel()
    {
        int count = 0;
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object playerLoadedLevel;

            if (p.CustomProperties.TryGetValue(GameData.PLAYER_LOAD, out playerLoadedLevel))
            {
                if ((bool)playerLoadedLevel)
                {
                    count++;
                }
            }
        }

        return count;
    }

    public void PrintInfo(string info)
    {
        Debug.Log(info);
        infoText.text = info;
    }
 

    #endregion PHOTON CALLBACK

}