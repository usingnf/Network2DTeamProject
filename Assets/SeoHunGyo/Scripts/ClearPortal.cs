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

            }

            
        }
    }
}
