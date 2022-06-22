using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JumpBlock : MonoBehaviour
{
    [Range(0.1f, 4f)] public float jumpPowerMultiplier;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.layer == 5)
        {   
            other.transform.GetComponent<PlayerControl>().SuperJump(jumpPowerMultiplier);
        }
    }
}
