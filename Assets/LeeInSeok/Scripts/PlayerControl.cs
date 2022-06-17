using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviourPun, IPunObservable
{
    private Rigidbody2D rigid;
    public float speed = 4.0f;
    public float jumpPower = 5.0f;
    public float size = 1.0f;
    Vector2 moveVec = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        GameManager.Instance.players.Add(photonView.OwnerActorNr, this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == false)
            return;

        moveVec = Vector2.zero;
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
            //Jump(jumpPower);
            photonView.RPC("Jump", RpcTarget.All, jumpPower);

        }
        moveVec = moveVec.normalized;
        rigid.position += moveVec * speed * Time.fixedDeltaTime;

    }

    [PunRPC]
    public void Jump(float power)
    {
        RaycastHit2D[] downHit = Physics2D.BoxCastAll(transform.position, new Vector2(size, 0.05f), 0, Vector2.down, size / 2, LayerMask.GetMask("UI", "Water"));
        if (downHit.Length <= 1)
        {
            return;
        }
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
            //stream.SendNext(rigid.position);
        }
        else
        {
            //rigid.position = (Vector2)stream.ReceiveNext();
        }
    }
}
