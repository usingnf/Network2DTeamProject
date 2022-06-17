using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviourPun
{
    public GameObject wall;
    public int count = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collide");
        if(collision.gameObject.layer == LayerMask.NameToLayer("UI"))
        {
            Debug.Log("ui");
            count++;
            if(count > 0)
            {
                photonView.RPC("Button", RpcTarget.All, true);
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("UI"))
        {
            count--;
            if(count <= 0)
            {
                photonView.RPC("Button", RpcTarget.All, false);
            }
        }
    }

    [PunRPC]
    public void Button(bool swt)
    {
        Debug.Log("button");
        if (swt == true)
        {
            wall.SetActive(false);
        }
        else
        {
            wall.SetActive(true);
        }
    }
}
