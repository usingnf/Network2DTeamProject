using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTeleportal : MonoBehaviour
{
    public HTeleportal otherPortal;
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("push!!!");
        if (other.gameObject.tag == "Player")
            StartCoroutine(Teleportal(other));
    }
    public IEnumerator Teleportal(Collider2D other)
    {
        if(Input.GetKeyDown(KeyCode.E))
        {

            other.transform.position = otherPortal.transform.position;
            Camera.main.GetComponent<HCameraController>().SetTarget(other.transform);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
