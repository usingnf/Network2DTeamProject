using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviourPun
{
    public GameObject owner = null;
    public Vector3 startVec = Vector3.zero;
    void Start()
    {
        startVec = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(owner != null)
            this.transform.position = Vector3.Lerp(this.transform.position, owner.transform.position, Time.deltaTime);
    }

    public void SetOwner(PlayerControl player)
    {
        if(owner != null)
            owner.GetComponent<PlayerControl>().key = null;
        owner = player.gameObject;
        player.key = this;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(owner == null)
                SetOwner(collision.gameObject.GetComponent<PlayerControl>());
        }
        
    }

    [PunRPC]
    public void Return()
    {
        if(owner != null)
            owner.GetComponent<PlayerControl>().key = null;
        this.owner = null;
        this.transform.position = startVec;
    }
}
