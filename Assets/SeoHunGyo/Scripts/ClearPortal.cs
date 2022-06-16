using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPortal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {   // TODO 플레이어랑만 충돌체크 하도록 하던가 예외처리로 하던가  
        // w or 위쪽 방향키 눌러야 통과하게

        if (other.gameObject.layer == 5)
        {   
            GameManager.Instance.PrintInfo(
                string.Format("{0}가 목적지에 도달하였습니다", other.gameObject.name)
            );
            
            other.transform.GetComponent<PlayerControl>().ClearStage();
        }
    }
}
