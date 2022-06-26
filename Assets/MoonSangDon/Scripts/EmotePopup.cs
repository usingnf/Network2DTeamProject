using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmotePopup : MonoBehaviourPun
{
    //뻘짓
    //private short Num;
    //private float Timer;
    //private GameObject Temp;
    //private Transform popupPos;
    //public GameObject[] Emotes;
    //public Transform[] spawnPos;
    //public static EmotePopup instance;

    //private void Start()
    //{
    //    for (int i = 0; i < Emotes.Length; i++)
    //    {
    //        Emotes[i].gameObject.SetActive(false);
    //    }
    //    instance = this;
    //}
    //private void Update()
    //{
    //    Timer += Time.deltaTime;
    //}
    // public void OtherClass(short Num)
    //{
    //    switch (Num)
    //    {
    //        case 0:
    //            photonView.RPC("EmotePopUp", RpcTarget.All, (short)0);
    //            break;
    //    }    
    //}

    //[PunRPC]
    //public void EmotePopUp(short Num)
    //{
    //    switch (Num)
    //    {
    //        case 0:
    //            if (Temp != null)
    //            {
    //                return;
    //            }
    //            int playerNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber();
    //            Temp = Instantiate(Emotes[0], spawnPos[playerNumber].position, spawnPos[playerNumber].rotation);
    //            Temp.SetActive(true);
    //            Destroy(Temp, 5f);
    //            Debug.Log(popupPos);
    //            break;
    //        case 1:
    //            popupPos = gameObject.transform;
    //            if (Temp != null)
    //            {
    //                return;
    //            }
    //            Temp = Instantiate(Emotes[1], popupPos);
    //            Temp.SetActive(true);
    //            Destroy(Temp, 5f);
    //            Debug.Log(popupPos);
    //            break;
    //        case 2:
    //            popupPos = gameObject.transform;
    //            if (Temp != null)
    //            {
    //                return;
    //            }
    //            Temp = Instantiate(Emotes[2], popupPos);
    //            Temp.SetActive(true);
    //            Destroy(Temp, 5f);
    //            Debug.Log(popupPos);
    //            break;
    //        case 3:
    //            popupPos = gameObject.transform;
    //            if (Temp != null)
    //            {
    //                return;
    //            }
    //            Temp = Instantiate(Emotes[3], popupPos);
    //            Temp.SetActive(true);
    //            Destroy(Temp, 5f);
    //            Debug.Log(popupPos);
    //            break;
    //        case 4:
    //            popupPos = gameObject.transform;
    //            if (Temp != null)
    //            {
    //                return;
    //            }
    //            Temp = Instantiate(Emotes[4], popupPos);
    //            Temp.SetActive(true);
    //            Destroy(Temp, 5f);
    //            Debug.Log(popupPos);
    //            break;
    //        case 5:
    //            popupPos = gameObject.transform;
    //            if (Temp != null)
    //            {
    //                return;
    //            }
    //            Temp = Instantiate(Emotes[5], popupPos);
    //            Temp.SetActive(true);
    //            Destroy(Temp, 5f);
    //            Debug.Log(popupPos);
    //            break;
    //        case 6:
    //            popupPos = gameObject.transform;
    //            if (Temp != null)
    //            {
    //                return;
    //            }
    //            Temp = Instantiate(Emotes[6], popupPos);
    //            Temp.SetActive(true);
    //            Destroy(Temp, 5f);
    //            Debug.Log(popupPos);
    //            break;

    //    }
    //}

}
