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
            if(photonView.IsMine)
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
            if (photonView.IsMine)
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
                if (null == otherPortal)
                    return;
                inPotal = false;
                playerPos.transform.position = otherPortal.transform.position;
                playerPos = null;
            }
        }
    }
}
