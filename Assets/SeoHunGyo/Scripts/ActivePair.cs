using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ActivePair : MonoBehaviourPun
{
    public GameObject pairObject;

    private void OnDisable() 
    {
        if (PhotonNetwork.IsMasterClient)
            photonView.RPC("Active", RpcTarget.All);
    }

    [PunRPC]
    void Active()
    {
        pairObject.SetActive(true);
    }

}
