using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonsEdit_button : MonoBehaviourPun
{
    //private SpriteRenderer render;
    //public Sprite on;
    public GameObject pairGravity;
   static public float timer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag =="Player")
        {
                    photonView.RPC("MoonsButton", RpcTarget.All);
        }
    }
    private void Update()
    {
        //timer += Time.deltaTime;
        //if (timer < 3)
        //    pairGravity.SetActive(false);
    }

    [PunRPC]
    public void MoonsButton()
    {
            //SoundManager.Instance.PlaySound("Button", transform.position, 1.0f, 1.0f);
            //render.sprite = on;
            pairGravity.SetActive(true);
            timer = 0;
    }
}
