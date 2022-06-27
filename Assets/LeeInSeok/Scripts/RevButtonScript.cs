using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevButtonScript : MonoBehaviourPun
{
    public GameObject wall;
    public int count = 0;
    public bool defaultState = true;
    public bool permament = false;
    private bool isUsed = false;
    public GameObject reverse;

    private SpriteRenderer render;
    public Sprite on;
    public Sprite off;
    public float timer;

    void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer <=3)
        {
            reverse.SetActive(false);
        }
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
<<<<<<< HEAD:Assets/LeeInSeok/Scripts/RevButtonScript.cs
        reverse.SetActive(true);
        timer = 0f;
=======
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
>>>>>>> parent of 2e08183 (Merge branch 'main' into Moons):Assets/LeeInSeok/Scripts/ButtonScript.cs
    }
}
