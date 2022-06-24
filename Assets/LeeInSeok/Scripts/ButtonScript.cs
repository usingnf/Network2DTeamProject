using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviourPun
{
    public GameObject wall;
    public int count = 0;
    public bool defaultState = true;
    public bool permament = false;
    private bool isUsed = false;

    private SpriteRenderer render;
    public Sprite on;
    public Sprite off;

    void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("collide");
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //Debug.Log("ui");
            count++;
            if(count > 0)
            {
                photonView.RPC("Button", RpcTarget.All, defaultState);
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            count--;
            if(count <= 0)
            {
                photonView.RPC("Button", RpcTarget.All, !defaultState);
            }
        }
    }

    [PunRPC]
    public void Button(bool swt)
    {
        if(isUsed == true)
        {
            return;
        }
        if(permament == true)
        {
            isUsed = true;
        }
        //Debug.Log("button");
        if (swt == true)
        {
            render.sprite = off;
            wall.SetActive(false);
        }
        else
        {
            render.sprite = on;
            wall.SetActive(true);
        }
    }
}
