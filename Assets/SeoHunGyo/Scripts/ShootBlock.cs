using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShootBlock : MonoBehaviourPun
{
    public Vector2  shootVelocity;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {   
            other.transform.GetComponent<PlayerControl>().ShootInit(shootVelocity, transform.position);
        }
    }
}