using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TrapBlockTrigger : MonoBehaviourPun
{
    public GameObject       box;
    public Collider2D        trap; 
    private SpriteRenderer      rend;
    private Color               invisibleColor;
    private Color               originColor;


    //플레이어 위로 레이캐스트 시도 했는데 여러 잡버그 발생으로 그냥 트리거를 작게 만들었습니다..........
    //
    void Start()
    { 
        rend =              GetComponentInChildren<SpriteRenderer>();
        //originColor =       rend.color;
        //invisibleColor.a =  0f;
        //rend.color =        invisibleColor;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("엔터됨");
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("엔터엔터됨");
            Debug.Log("트랩블럭콜라이더 엔터됨");
            OnActiveBlock();
            photonView.RPC("OnActiveBlock", RpcTarget.All);
        }
    }

    [PunRPC]
    void OnActiveBlock()
    {
        box.SetActive(true);
    }
}
