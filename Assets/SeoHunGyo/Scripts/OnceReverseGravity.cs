using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class OnceReverseGravity : MonoBehaviourPun
{
    public GameObject pairObject;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {   
            GameManager.Instance.PrintInfo( "중력 반전" );
            StageManager.Instance.ReverseGravity();
        }
    }

    [PunRPC]
    void Toggle()
    {
    }
    
}
