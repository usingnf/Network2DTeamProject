using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class ResetObstacle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(other.GetComponent<PhotonView>().IsMine == true)
            {
                GameManager.Instance.PrintInfo(
                string.Format("{0}가 원위치 되었습니다", other.gameObject.name));
            
                //SoundManager.Instance.PlaySound("Die", this.transform.position, 1, 1);
                other.GetComponent<PhotonView>().RPC("Return", RpcTarget.All);
                //other.transform.GetComponent<PlayerControl>().Return();
            }

        }
    }
}
