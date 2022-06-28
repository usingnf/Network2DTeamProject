using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTeleportal : MonoBehaviour
{
    public HTeleportal otherPortal;
    Transform playerPos;

    bool inPotal = false;
   private void OnTriggerEnter2D(Collider2D other)
   {
        PhotonView photonView = other.GetComponent<PhotonView>();
        if(photonView != null)
        {
            if(photonView.IsMine == true)
            {
                inPotal = true;
                playerPos = other.transform;
            }
        }
        
   }
    private void OnTriggerExit(Collider other)
    {
        PhotonView photonView = other.GetComponent<PhotonView>();
        if (photonView != null)
        {
            if (photonView.IsMine == true)
            {
                inPotal = false;
                playerPos = null;
            }
        }
            
    }
    private void Update()
    {
        if(inPotal)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                inPotal = false;
                playerPos = null;
                playerPos.transform.position = otherPortal.transform.position;
            }
        }
    }
}
