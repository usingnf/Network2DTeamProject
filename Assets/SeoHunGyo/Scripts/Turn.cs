using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Turn : MonoBehaviourPun
{
    [Header("Need BoxCollider2D / MoveObject")]
    public MoveObject       moveObjectScript;

    Vector2                 m_scaleVec;
    RaycastHit2D[]          m_hits;
    float                   m_length;


    private void Awake() {
        Init();
    }

    void Init()
    {
        if (moveObjectScript == null)
        {
            moveObjectScript = GetComponent<MoveObject>();
        }

        Vector2 tempVec = GetComponent<BoxCollider2D>().size;

        m_length = tempVec.x * 0.4f;
        m_scaleVec = new Vector2(tempVec.x - m_length, tempVec.y * 0.6f);
        m_length += 0.05f;
    }

    private void Update() 
    {
        if (moveObjectScript.isLeft)
        {   
            m_hits = Physics2D.BoxCastAll(transform.position, m_scaleVec, 0f, Vector2.left, m_length, LayerMask.GetMask("Obstacle"));
            
            if (m_hits.Length >= 2)
            {   
                moveObjectScript.isLeft = false;
            }
        }   
        else
        {   
            m_hits = Physics2D.BoxCastAll(transform.position, m_scaleVec, 0f, Vector2.right, m_length, LayerMask.GetMask("Obstacle"));
            
            if (m_hits.Length >= 2)
            {   
                moveObjectScript.isLeft = true;
            }
        } 

    }
}
