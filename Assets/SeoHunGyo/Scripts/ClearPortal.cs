using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ClearPortal : MonoBehaviourPun
{
    private void OnTriggerEnter2D(Collider2D other) 
    {   
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (other.GetComponent<PhotonView>().IsMine)
            {
                PlayerControl player = other.transform.GetComponent<PlayerControl>();

                if (player == null) return;

                if (player.isClear) return;

                StageManager.Instance.photonView.RPC("GoalIn", RpcTarget.All , player.photonView.OwnerActorNr);

                // if (other.transform.GetComponent<PlayerControl>().isClear == true)
                //     return;
                // GameManager.Instance.PrintInfo(
                //     string.Format("{0}가 목적지에 도달하였습니다", other.gameObject.name)
                // );
                
                // other.transform.GetComponent<PlayerControl>().ClearStage();
            }

            
        }
    }
}
