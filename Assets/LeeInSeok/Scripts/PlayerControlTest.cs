using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlTest : MonoBehaviourPun, IPunObservable
{
    private Rigidbody2D rigid;
    public float speed = 4.0f;
    public float jumpPower = 5.0f;
    public float size = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == false)
            return;

        Vector2 moveVec = Vector2.zero;
        if(Input.GetKey(KeyCode.A))
        {
            moveVec += new Vector2(-1, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveVec += new Vector2(1, 0);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            
            photonView.RPC("Jump", RpcTarget.All, photonView.ViewID, jumpPower);
            //Jump(jumpPower);
            
        }
        moveVec = moveVec.normalized;
        rigid.position += moveVec * speed * Time.deltaTime;
    }
    /*
    [PunRPC]
    public void Jump(int playerNum, float power)
    {
        if (playerNum != photonView.ViewID)
            return;
        //int num = PhotonNetwork.LocalPlayer.GetPlayerNumber();
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        RaycastHit2D[] upHit = Physics2D.BoxCastAll(transform.position, new Vector2(size, 0.05f), 0, Vector2.up, size / 2, LayerMask.GetMask("UI"));

        foreach (RaycastHit2D hit in upHit)
        {
            if (hit.collider.gameObject == gameObject)
                continue;
            IPutonable putOn = null;
            if (hit.collider != null)
            {
                putOn = hit.collider.GetComponent<IPutonable>();
                if (putOn != null)
                {
                    photonView.RPC("Puton", RpcTarget.All, power);
                    //putOn.Puton(power);
                }
            }
        }
    }
    
    public void Puton(float power)
    {
        Debug.Log(this.gameObject.name);
        rigid.AddForce(Vector2.up * power, ForceMode2D.Impulse);
        RaycastHit2D[] upHit = Physics2D.BoxCastAll(transform.position, new Vector2(size, 0.05f), 0, Vector2.up, size / 2, LayerMask.GetMask("UI"));

        foreach(RaycastHit2D hit in upHit)
        {
            if (hit.collider.gameObject == gameObject)
                continue;
            IPutonable putOn = null;
            if(hit.collider != null)
            {
                putOn = hit.collider.GetComponent<IPutonable>();
                if(putOn != null)
                {
                    //photonView.RPC("Puton", RpcTarget.All, power);
                    putOn.Puton(power);
                }
            }
        }
    }
    */
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {

        }
        else
        {

        }
    }
}
