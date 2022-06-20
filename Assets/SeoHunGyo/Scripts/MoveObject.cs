using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class MoveObject : MonoBehaviourPun
{
    public float    moveSpeed;
    public bool     isLeft;
    

    Rigidbody2D     m_rigid;

    private void Awake() {
        m_rigid = GetComponent<Rigidbody2D>();
    }

    private void Update() 
    {
        Move();
    }


    void Move()
    {
        if (isLeft)
        {
            m_rigid.position -= new Vector2 (moveSpeed * Time.deltaTime , 0f) ;
        }
        else{
            m_rigid.position += new Vector2 (moveSpeed * Time.deltaTime , 0f) ;
        }
    }
}
