using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class MoonsEdit_Reverse : MonoBehaviourPun
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControl player = other.GetComponent<PlayerControl>();

            if (player.photonView.IsMine)
            {
                GameManager.Instance.PrintInfo("중력 반전");
                StageManager.Instance.ReverseGravity(player);

                //photonView.RPC("Toggle", RpcTarget.All);
            }

        }
    }

    //[PunRPC]
    //void Toggle()
    //{
    //    pairObject.SetActive(true);
    //    gameObject.SetActive(false);
    //}

}