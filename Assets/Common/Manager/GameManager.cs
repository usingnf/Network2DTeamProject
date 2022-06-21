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

    private void Awake()
    {
        Instance = this;
        object stage = 0;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(GameData.Stage, out stage) == true)
        {
            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable() { { GameData.Stage, (int)stage } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }
        
    }

    public void Start()
    {
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable() { { GameData.PLAYER_LOAD, true } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        Debug.Log("GameManager");

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("master");
            props = new ExitGames.Client.Photon.Hashtable() { { "RoomState", "Start" } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
            masterNum = PhotonNetwork.MasterClient.ActorNumber;
            StartCoroutine(StartGame());
        }
        else
        {
            if((string)PhotonNetwork.CurrentRoom.CustomProperties["RoomState"] == "Start")
            {
                StartCoroutine(Rejoin());
                Debug.Log("Rejoin");
            }
            else
            {
                Debug.Log("noMaster");
                StartCoroutine(StartGame());
            }
        }
        
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
        obj.GetComponent<PlayerControl>().text.text = PhotonNetwork.LocalPlayer.NickName;
        Camera.main.transform.parent = obj.transform;
        Camera.main.transform.position = new Vector3(0, 0, -10);
        //TODO: 황인태 1줄 아래 추가
        Camera.main.GetComponent<HCameraController>().target = obj;
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1.0f);
        PrintInfo("Start Game!");
        
        int playerNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber();
        Debug.Log(playerNumber);
        GameObject obj = PhotonNetwork.Instantiate("PlayerCharacter", spawnPos[playerNumber].position, spawnPos[playerNumber].rotation, 0);
        obj.GetComponent<PlayerControl>().text.text = PhotonNetwork.LocalPlayer.NickName;
        Camera.main.transform.parent = obj.transform;
        Camera.main.transform.position = new Vector3(0, 0, -10);
        //TODO: 황인태 2줄 아래 추가
        Camera.main.GetComponent<HCameraController>().target = obj;
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Wall", new Vector3(0.24f, -1.41f, 0), Quaternion.identity, 0);
        }
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
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Wall", new Vector3(0.24f, -1.41f, 0), Quaternion.identity, 0);
        }
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

}
