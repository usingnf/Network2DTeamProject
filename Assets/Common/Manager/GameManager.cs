using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameState
{
    None,
    Play,
    Clear,
    Pause,
}

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance { get; private set; }

    public GameState state = GameState.None;
    public Text infoText;
    public Transform[] spawnPos;
    public Dictionary<int, GameObject> players = new Dictionary<int, GameObject>();
    public int masterNum = 0;
    public PlayerControl myPlayer;

    [Header("Option UI")]
    public GameObject optionUI;

    private void Awake()
    {
        Instance = this;
        object stage = 0;
        if(PhotonNetwork.IsConnected == true)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(GameData.Stage, out stage) == true)
            {
                ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable() { { GameData.Stage, (int)stage } };
                PhotonNetwork.CurrentRoom.SetCustomProperties(props);
            }
        }
            
    }

    private void Start()
    {
        if (spawnPos.Length == 0) return;

        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable() { { GameData.PLAYER_LOAD, true } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        if (PhotonNetwork.IsMasterClient)
        {
            props = new ExitGames.Client.Photon.Hashtable() { { "RoomState", "Start" } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
            masterNum = PhotonNetwork.MasterClient.ActorNumber;
            StartCoroutine(StartGame());
        }
        else
        {
            if(PhotonNetwork.IsConnected == true)
            {
                if ((string)PhotonNetwork.CurrentRoom.CustomProperties["RoomState"] == "Start")
                {
                    StartCoroutine(Rejoin());
                }
                else
                {
                    StartCoroutine(StartGame());
                }
            }
            
        }
        
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            SetOptionUI(!optionUI.activeSelf);
        }
    }

    private void SetOptionUI(bool isOn)
    {
        optionUI.SetActive(isOn);
        myPlayer.isStop = isOn;
    }

    #region PHOTON CALLBACK

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("MasterChange");
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Respawn");
        foreach(GameObject obj2 in obj)
        {
            Debug.Log("obj");
            //obj2.GetComponent<PhotonView>().OwnerActorNr = newMasterClient.ActorNumber;
            //obj2.GetComponent<PhotonView>().TransferOwnership(newMasterClient);
            if(PhotonNetwork.LocalPlayer == newMasterClient)
            {
                PhotonNetwork.Instantiate("Wall", obj2.transform.position, obj2.transform.rotation, 0);
            }
            
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected : " + cause.ToString());
        SceneManager.LoadScene("LobbyScene");
    }

    public override void OnLeftRoom()
    {   
        PhotonNetwork.Disconnect();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        /*
        if (changedProps.ContainsKey(GameData.PLAYER_LOAD))
        {
            if (CheckAllPlayerLoadLevel())
            {
                StartCoroutine(StartCountDown());
            }
            else
            {
                PrintInfo("wait players " + PlayersLoadLevel() + " / " + PhotonNetwork.PlayerList.Length);
            }
        }
        */
    }

    #endregion PHOTON CALLBACK

    private IEnumerator Rejoin()
    {
        yield return new WaitForSeconds(1.0f);
        int playerNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber();
        GameObject obj = PhotonNetwork.Instantiate("PlayerCharacter", spawnPos[playerNumber].position, spawnPos[playerNumber].rotation, 0);
        Camera.main.transform.parent = obj.transform;
        Camera.main.transform.position = new Vector3(0, 0, -10);
        Camera.main.GetComponent<HCameraController>().target = obj;
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1.0f);
        PrintInfo("Start Game!");

        Debug.Log("Create");
        int playerNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber();
        Debug.Log(playerNumber);

        GameObject obj = PhotonNetwork.Instantiate("PlayerCharacter", spawnPos[playerNumber].position, spawnPos[playerNumber].rotation, 0);
        Camera.main.transform.parent = obj.transform;
        Camera.main.transform.position = new Vector3(0, 0, -10);
        Camera.main.GetComponent<HCameraController>().target = obj;
    }

    private IEnumerator StartCountDown()
    {
        PrintInfo("All Player Loaded, Start Count Down");
        yield return new WaitForSeconds(1.0f);

        for (int i = GameData.COUNTDOWN; i > 0; i--)
        {
            PrintInfo("Count Down " + i);
            yield return new WaitForSeconds(1.0f);
        }

        PrintInfo("Start Game!");

        int playerNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber();

        GameObject obj = PhotonNetwork.Instantiate("PlayerCharacter", spawnPos[playerNumber].position, spawnPos[playerNumber].rotation, 0);
        Camera.main.transform.parent = obj.transform;
    }

    private bool CheckAllPlayerLoadLevel()
    {
        return PlayersLoadLevel() == PhotonNetwork.PlayerList.Length;
    }

    private int PlayersLoadLevel()
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
        if(info != null) { 
            Debug.Log(info);

            if (infoText == null) return;

            infoText.text = info;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }


    public Transform GetNextObserveTF(ref int obNumber)
    {
        for (int i = 1; i <= players.Count; i++)
        {
            obNumber += 1;
            //Debug.Log("obNumber : " + obNumber + " / PhotonNetwork.LocalPlayer.ActorNumber : " + PhotonNetwork.LocalPlayer.ActorNumber);

            if (obNumber == players.Count + 1)
            {   // 유효범위
                obNumber = 1;
            }

            if (obNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {   // 자기 자신인 경우
                continue;
            }

            GameObject player;
            players.TryGetValue(obNumber, out player);
            if (player.GetComponent<PlayerControl>().isObserve) 
            {   // 이미 클리어 해서 옵저버 모드인 경우
                continue;
            }

            return player.transform;
        }
        return null;
    }

    public IEnumerator MoveScene(int num)
    {
        yield return new WaitForSeconds(1.2f);
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("StageScene_" + num);
        }
    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        players.Remove(otherPlayer.ActorNumber);
    }
}
