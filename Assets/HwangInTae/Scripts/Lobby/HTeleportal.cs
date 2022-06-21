using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTeleportal : MonoBehaviour
{
    public HTeleportal otherPortal;

   private void OnTriggerStay2D(Collider2D other)
   {
       Debug.Log("push!!!");
           StartCoroutine(Teleportal(other));
   }
    //private void Update()
    //{
    //    OnPotalEnter();
    //}
    //private void OnPotalEnter()
    //{
    //    if (Physics2D.BoxCast(transform.position, new Vector2(1, 1), 0,Vector2.zero,0.02f,LayerMask.GetMask("Player")) && Input.GetKeyDown(KeyCode.E))
    //    {
    //        other.transform.position = otherPortal.transform.position;
    //    }
    //}
    public IEnumerator Teleportal(Collider2D other)
    {
        if(Input.GetKeyDown(KeyCode.E) && other.gameObject.tag == "Player")
        {
            other.transform.position = otherPortal.transform.position;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
