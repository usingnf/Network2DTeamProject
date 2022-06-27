using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class OnceReverseGravity : MonoBehaviourPun
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.layer == 5)
        {   
            GameManager.Instance.PrintInfo( "중력 반전" );
            StageManager.Instance.ReverseGravity();
<<<<<<< HEAD:Assets/SeoHunGyo/Scripts/OnceReverseGravity.cs
        }
    }

    [PunRPC]
    void Toggle()
    {
    }
=======
            
            gameObject.SetActive(false);
        }
    }
>>>>>>> parent of 2e08183 (Merge branch 'main' into Moons):Assets/SeoHunGyo/Scripts/ReverseGravity.cs
    
}
