using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum EDir { 
    None, Left, Right, Up, Down, 
}

public class OneSideBlock : MonoBehaviourPun
{
    public EDir     eWalkableSide;
    
    Vector2         m_castVec;


    private void Awake() {
        Init();
    }

    void Init()
    {
        switch (eWalkableSide)
        {
            case EDir.Left     : m_castVec = Vector2.left;     break;
            case EDir.Right    : m_castVec = Vector2.right;    break;
            case EDir.Up       : m_castVec = Vector2.up;       break;
            case EDir.Down     : m_castVec = Vector2.down;     break;
        }
    }


}
