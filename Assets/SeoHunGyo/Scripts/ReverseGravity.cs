﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class ReverseGravity : MonoBehaviourPun
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.layer == 5)
        {   
            GameManager.Instance.PrintInfo( "중력 반전" );
            StageManager.Instance.ReverseGravity();
            
            gameObject.SetActive(false);
        }
    }
    
}