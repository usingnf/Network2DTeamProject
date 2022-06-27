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
        inPotal = true;
        playerPos = other.transform;
   }
    private void OnTriggerExit(Collider other)
    {
        inPotal = false;
        playerPos = null;
    }
    private void Update()
    {
        if(inPotal)
            StartCoroutine("OnPotalEnter");
    }
    private IEnumerator OnPotalEnter()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position,Vector2.zero);
        if (ray.transform.tag == "Player" && Input.GetKeyDown(KeyCode.E))
        {
            inPotal = false;
            playerPos.transform.position = otherPortal.transform.position;
            yield return new WaitForSeconds(0.5f);
        }
    }
    
}
