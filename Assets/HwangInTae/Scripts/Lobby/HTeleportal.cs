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
        PhotonView photonview = other.GetComponent<PhotonView>();
        if (photonview != null)
        {
            if(photonview.IsMine == true)
            {
                inPotal = true;
                playerPos = other.transform;
            }
        }
        
   }
    private void OnTriggerExit(Collider other)
    {
        PhotonView photonview = other.GetComponent<PhotonView>();
        if (photonview != null)
        {
            if (photonview.IsMine == true)
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
                if(playerPos != null)
                {
                    inPotal = false;
                    playerPos.position = otherPortal.transform.position;
                }
            }
        }
            
    }    
}
