using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviourPun
{
    
    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
            StartCoroutine(MoveScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator MoveScene()
    {
        yield return new WaitForSeconds(1.0f);
        PhotonNetwork.LoadLevel("GameScene");
    }
}
