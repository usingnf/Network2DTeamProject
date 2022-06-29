using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviourPun
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerControl player = collision.gameObject.GetComponent<PlayerControl>();
            if(player.key != null)
            {
                if(PhotonNetwork.IsMasterClient)
                    PhotonNetwork.Destroy(player.key.gameObject);
                player.key = null;
                if (PhotonNetwork.IsMasterClient)
                    PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
}
