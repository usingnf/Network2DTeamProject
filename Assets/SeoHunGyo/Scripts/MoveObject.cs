using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class MoveObject : MonoBehaviourPun
{
    public float    moveSpeed;
    public bool     isLeft;
    
    
    Rigidbody2D     m_rigid;
    SpriteRenderer rend;


    private void Awake()
    { 
        m_rigid = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
    }
    private void Update()   { Move(); }


    void Move()
    {
        if (isLeft)
        {
            rend.flipX = true;
            m_rigid.position -= new Vector2 (moveSpeed * Time.deltaTime , 0f) ;
        }
        else
        {
            rend.flipX = false;
            m_rigid.position += new Vector2 (moveSpeed * Time.deltaTime , 0f) ;
        }
    }
}
