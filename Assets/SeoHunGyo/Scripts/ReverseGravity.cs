using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class ReverseGravity : MonoBehaviourPun
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
<<<<<<< HEAD

        if (other.CompareTag("Player"))
        {
            PlayerControl player = other.GetComponent<PlayerControl>();

            if(player.photonView.IsMine)
            {
                GameManager.Instance.PrintInfo( "중력 반전" );
                StageManager.Instance.ReverseGravity(player);
                
                photonView.RPC("Toggle", RpcTarget.All);
            }
            
=======
        if (other.gameObject.layer == 5)
        {   
            GameManager.Instance.PrintInfo( "중력 반전" );
            StageManager.Instance.ReverseGravity();
            
            gameObject.SetActive(false);
>>>>>>> parent of 2e08183 (Merge branch 'main' into Moons)
        }
    }
    
}
