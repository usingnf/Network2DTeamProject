using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace SHG
{
    public class PlayerController : MonoBehaviourPun
    {
        public float moveSpeed = 10f;





        private void Update() 
        {
            if (!photonView.IsMine)
            {
                return;
            }


            Move();
        }


        public void Move()
        {
            
        }
    }
}

