using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TurnObstacle : MonoBehaviourPun
{
    public LayerMask        obstacleLayer;
    public MoveObject       moveObjectScript;
    public Vector2          scaleVec;

    RaycastHit2D[]          m_hits;


    private void Awake() {
        Init();
    }

    void Init()
    {
        if (moveObjectScript == null)
        {
            moveObjectScript = GetComponent<MoveObject>();
        }
    }

    private void Update() 
    {
        if (moveObjectScript.isLeft)
        {   
            m_hits = Physics2D.BoxCastAll(transform.position, scaleVec, 0f, Vector2.left, 0.05f, obstacleLayer);
            
            if (m_hits.Length >= 1)
            {   
                moveObjectScript.isLeft = false;
            }
        }   
        else
        {   
            m_hits = Physics2D.BoxCastAll(transform.position, scaleVec, 0f, Vector2.right, 0.05f, obstacleLayer);

            if (m_hits.Length >= 1)
            {   
                moveObjectScript.isLeft = true;
            }
        } 
    }
}
