using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearPortal : MonoBehaviourPun
{
    public List<GameObject> clearList = new List<GameObject>();
    public Text text = null;

    private void Start()
    {
        text.text = $"{StageManager.Instance.clearCount}/{PhotonNetwork.PlayerList.Length}";
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {   // 캐릭터 렌더
        // w or 위쪽 방향키 눌러야 통과하게
        if (clearList.Contains(other.gameObject))
            return;


        if (other.gameObject.layer == 5)
        {   
            clearList.Add(other.gameObject);

            GameManager.Instance.PrintInfo(
                string.Format("{0}가 목적지에 도달하였습니다", other.gameObject.name)
            );
            other.transform.GetComponent<PlayerControl>().ClearStage();
            text.text = $"{StageManager.Instance.clearCount}/{PhotonNetwork.PlayerList.Length}";
        }
    }
}
