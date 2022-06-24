using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class StageManager : MonoBehaviourPunCallbacks
{
    public UnityAction onReverseGravity;
    public static StageManager Instance { get; private set; }

    public int curStage = 1;        // 매 스테이지 StageMgr 둘거면 string nextSceneName 두고 그걸로 불러와도 될듯
    public int clearCount = 0;      // 클리어한 플레이어 수 체크



    private void Awake() {
        Instance = this;
    }


    //
    private void Update() 
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }


    void Restart()
    {   // TODO 씬 이름 "Stage " + curStage로

        Hashtable props = new Hashtable() { { "RoomState", "ReStart" } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);

        //PhotonNetwork.LoadLevel("GameScene");
        PhotonNetwork.LoadLevel("StageScene_1");
    }

    public bool CheckClear()
    {   
        foreach(Player player in PhotonNetwork.PlayerList)
        {   // 플레이어 돌면서 Clear 확인
            object isClear;
            if (player.CustomProperties.TryGetValue(GameData.PLAYER_CLEAR, out isClear))
            {
                if (!(bool)isClear) 
                {   //Debug.Log("UnClear");
                    return false;
                }
            }
        }

        StartCoroutine(StageClear());
        return true;
    }

    public void GoalIn(PlayerControl player)
    {
        clearCount++;

        if (clearCount == PhotonNetwork.PlayerList.Length)
        {
            StartCoroutine(StageClear());
        }
        else
        {
            player.SetObserveMode();
        }
    }


    IEnumerator StageClear()
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.PrintInfo("Stage Clear");


        yield return new WaitForSeconds(0.5f);
        
        Hashtable props = new Hashtable() { { "RoomState", "Clear" } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);

        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel("StageScene_1");
        //PhotonNetwork.LoadLevel(string.Format( "StageScene_{0}", ++curStage ));

    }

    public void ReverseGravity()
    {   
        photonView.RPC("Event_ReverseGravity", RpcTarget.All);
    }

    [PunRPC]
    public void Event_ReverseGravity()
    {   
        onReverseGravity?.Invoke();
    }
}
