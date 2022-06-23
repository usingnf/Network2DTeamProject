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

        public Text infoText;
        public Transform spawnPos;
    public int readyPlayer = 0;

        private void Awake()
        {
            Instance = this;
        }

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
            SceneManager.LoadScene(0);
        }

        public override void OnLeftRoom()
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                PhotonNetwork.LeaveRoom();
                SceneManager.LoadScene(0);
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
    }

    //public static InLobby Instance { get; private set; }

    //public Text curPlayer;
    //public Text playerCount;

    //int p = 0;

    //public GameObject[] playerPrefab;

    //public Transform spawnPos;

    //int playerMax;
    //int curPlayerCount;

    //private void Awake()
    //{
    //    Instance = this;
    //}

    //private void Start()
    //{
    //    curPlayerCount = PhotonNetwork.CountOfPlayersInRooms;
    //    curPlayer.text = curPlayerCount.ToString();
    //}

    //public void EnterPlayer()
    //{
    //    GameObject entry = PhotonNetwork.Instantiate("playerPrefab 프리펩", spawnPos.position, spawnPos.rotation);
    //    p++;
    //    if (p < playerPrefab.Length)
    //        p = 0;
    //}

    //public void PlayButtonClicker()
    //{
    //    PhotonNetwork.LoadLevel("GameScene");
    //}

    //public void LeaveButtonClicker()
    //{
    //    PhotonNetwork.LeaveRoom();
    //    SceneManager.LoadScene("LobbyScene");
    //}


    //public void Start()
    //{
    //    ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable() { { GameData.PLAYER_IN, true } };
    //    PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    //}

    //#region PHOTON CALLBACK

    //public override void OnDisconnected(DisconnectCause cause)
    //{
    //    Debug.Log("Disconnected : " + cause.ToString());
    //    SceneManager.LoadScene("LobbyScene");
    //}

    //public override void OnLeftRoom()
    //{
    //    PhotonNetwork.Disconnect();
    //}

    //public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    //{
    //    if (changedProps.ContainsKey(GameData.PLAYER_LOAD))
    //    {
    //        if (CheckAllPlayerLoadLevel())
    //        {
    //            StartCoroutine(StartCountDown());
    //        }
    //        else
    //        {
    //            PrintInfo("wait players " + PlayersLoadLevel() + " / " + PhotonNetwork.PlayerList.Length);
    //        }
    //    }
    //}

    //#endregion PHOTON CALLBACK

    //private IEnumerator StartCountDown()
    //{
    //    PrintInfo("All Player Loaded, Start Count Down");
    //    yield return new WaitForSeconds(1.0f);

    //    for (int i = GameData.COUNTDOWN; i > 0; i--)
    //    {
    //        PrintInfo("Count Down " + i);
    //        yield return new WaitForSeconds(1.0f);
    //    }

    //    PrintInfo("Start Game!");

    //    int playerNumber = 1;
    //    PhotonNetwork.Instantiate("PlayerModel", spawnPos[playerNumber].position, spawnPos[playerNumber].rotation, 0);
    //}

    //private bool CheckAllPlayerLoadLevel()
    //{
    //    return PlayersLoadLevel() == PhotonNetwork.PlayerList.Length;
    //}

    //private int PlayersLoadLevel()
    //{
    //    int count = 0;
    //    foreach (Player p in PhotonNetwork.PlayerList)
    //    {
    //        object playerLoadedLevel;

    //        if (p.CustomProperties.TryGetValue(GameData.PLAYER_LOAD, out playerLoadedLevel))
    //        {
    //            if ((bool)playerLoadedLevel)
    //            {
    //                count++;
    //            }
    //        }
    //    }

    //    return count;
    //}

    //private void PrintInfo(string info)
    //{
    //    Debug.Log(info);
    //    infoText.text = info;
    //}