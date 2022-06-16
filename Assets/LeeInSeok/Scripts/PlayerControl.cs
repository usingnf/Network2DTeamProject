using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerControl : MonoBehaviourPun, IPunObservable
{
    private Rigidbody2D rigid;
    public float speed = 4.0f;
    public float jumpPower = 5.0f;
    public float size = 1.0f;



    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (photonView.IsMine == false)
            return;

        Vector2 moveVec = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            moveVec += new Vector2(-1, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveVec += new Vector2(1, 0);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump(jumpPower);
            //photonView.RPC("Jump", RpcTarget.All, photonView.ViewID, jumpPower);

        }
        moveVec = moveVec.normalized;
        rigid.position += moveVec * speed * Time.deltaTime;
    }

    public void Jump(float power)
    {
        RaycastHit2D[] upHit = Physics2D.BoxCastAll(transform.position, new Vector2(size, 0.05f), 0, Vector2.up, size / 2, LayerMask.GetMask("UI"));
        if(upHit.Length <= 1)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

        }
        else
        {

        }
    }

    public void ClearStage()
    {   // TODO 시점
        Hashtable props = new Hashtable() {{GameData.PLAYER_CLEAR, true}};

        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        
        StageManager.Instance.CheckClear();
    }
}
