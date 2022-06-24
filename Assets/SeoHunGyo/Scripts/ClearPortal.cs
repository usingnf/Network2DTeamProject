using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPortal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {   
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (other.transform.GetComponent<PlayerControl>().isClear == true)
                return;
            GameManager.Instance.PrintInfo(
                string.Format("{0}가 목적지에 도달하였습니다", other.gameObject.name)
            );
            
            other.transform.GetComponent<PlayerControl>().ClearStage();
        }
    }
}
