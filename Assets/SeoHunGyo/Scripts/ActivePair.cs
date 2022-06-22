using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ActivePair : MonoBehaviourPun
{
    public GameObject pairObject;

    private void OnDisable() 
    {
        pairObject.SetActive(true);
    }
}
