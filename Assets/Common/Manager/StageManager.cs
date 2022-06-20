using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageManager : MonoBehaviourPunCallbacks
{
    public static StageManager Instance { get; private set; }

    public int curStage = 1;
    public int clearCount = 0;      // 4명 다 들어오면 다음 스테이지

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
        PhotonNetwork.LoadLevel("LoadingScene");
        //PhotonNetwork.LoadLevel("StageScene_1");
    }

    public bool CheckClear()
    {
        foreach(Player player in PhotonNetwork.PlayerList)
        {   // 플레이어 돌면서 Clear 확인
            object isClear;
            if (player.CustomProperties.TryGetValue(GameData.PLAYER_CLEAR, out isClear))
            {
                if (!(bool)isClear) 
                {
                    return false;
                }
            }
        }

        StartCoroutine(StageClear());
        return true;
    }


    IEnumerator StageClear()
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.PrintInfo("Stage Clear");


        yield return new WaitForSeconds(1f);
        // TODO 스테이지 전환
        // PhotonNetwork.LoadLevel(
        //     string.Format("StageScene_{0}", ++curStage)
        // );
        Debug.Log("clear");
        PhotonNetwork.LoadLevel("GameScene");
    }
}
