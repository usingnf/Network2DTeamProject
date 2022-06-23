using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JumpBlock : MonoBehaviour
{
    Animator m_anim;
    [Range(0.1f, 4f)] public float jumpPowerMultiplier;

    private void Awake() {
        m_anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.layer == 5)
        {   
            other.transform.GetComponent<PlayerControl>().SuperJump(jumpPowerMultiplier);
            m_anim.SetTrigger("Works");
        }
    }
}
