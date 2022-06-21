using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerControl : MonoBehaviourPun, IPunObservable
{
    private Rigidbody2D rigid;
    private BoxCollider2D coll;
    private SpriteRenderer rend;
    public float speed = 4.0f;
    public float jumpPower = 5.0f;
    public float size = 1.0f;
    Vector2 moveVec = Vector2.zero;
    public bool isObserve;
    public int observeNumber;
    public Text text;
    public KeyScript key = null;
    private GameObject doorObj = null;


    private void OnEnable() 
    {
        ResetClearCustomProperties();
    }

    void ResetClearCustomProperties()
    {
        // Hashtable props = new Hashtable {{ GameData.PLAYER_CLEAR, false }};
        // PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        isObserve = false;
    }

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        rend = GetComponent<SpriteRenderer>();

        if(GameManager.Instance != null)
            GameManager.Instance.players.Add(photonView.OwnerActorNr, this.gameObject);

        Debug.Log(string.Format(
            "photonView.OwnerActorNr : {0} / LocalPlayer.ActorNumber : {1}",
            photonView.OwnerActorNr,
            PhotonNetwork.LocalPlayer.ActorNumber)
        );
    }


    void Update()
     {
        if (photonView.IsMine == false)
            return;

        
        if (isObserve)
        {
            Observe();
        }
        else
        {
            Move();
        }
    }

    void Move()
    {
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

    void Observe()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ObserveNext(ref observeNumber);
        }
    }

    [PunRPC]
    public void Jump(float power)
    {
        int count = 0;
        RaycastHit2D[] downHit = Physics2D.BoxCastAll(transform.position, new Vector2(size, 0.05f), 0, Vector2.down, size / 2, LayerMask.GetMask("UI", "Water"));
        foreach(RaycastHit2D hit in downHit)
        {
            if(hit.collider.isTrigger == false)
            {
                count++;
            }
        }
        if (count <= 1)
        {
            return;
        }
        count = 0;
        RaycastHit2D[] upHit = Physics2D.BoxCastAll(transform.position, new Vector2(size, 0.05f), 0, Vector2.up, size / 2, LayerMask.GetMask("UI"));
        foreach (RaycastHit2D hit in upHit)
        {
            if (hit.collider.isTrigger == false)
            {
                count++;
            }
        }
        if (count <= 1)
        {
            rigid.AddForce(Vector2.up * power, ForceMode2D.Impulse);
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

    public void ClearStage()
    {   
        // Hashtable props = new Hashtable() {{GameData.PLAYER_CLEAR, true}};

        // PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        

        
        // 아직 스테이지 완전 클리어 아니면 옵저버 모드로 전환
        StageManager.Instance.GoalIn(this);

        // if (!StageManager.Instance.CheckClear())
        // {   
        //     isObserve = true;
        //     rend.enabled = false;
        //     coll.enabled = false;
        //     ObserveNext(photonView.OwnerActorNr);
        // }

    }

    public void SetObserveMode()
    {
        isObserve = true;
        rend.enabled = false;
        coll.enabled = false;
        rigid.gravityScale = 0f;

        observeNumber = photonView.OwnerActorNr;

        ObserveNext(ref observeNumber);
    }

    void ObserveNext(ref int obNumber)
    {   
        Camera.main.transform.SetParent(GameManager.Instance.GetNextObserveTF(ref obNumber));
        Camera.main.transform.localPosition = new Vector3(0f, 0f, -10f);
    }

    [PunRPC]
    void UseKey()
    {
        if (this.key != null)
        {
            Destroy(key.gameObject);
            this.key = null;
            Destroy(doorObj.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //TODO: 테그 추가해야함.
        if (collision.gameObject.tag == "Key")
        {
            if(collision.GetComponent<KeyScript>().owner == null)
                collision.GetComponent<KeyScript>().SetOwner(this);
        }
        if (collision.gameObject.tag == "Door")
        {
            if(this.key != null)
            {
                doorObj = collision.gameObject;
                photonView.RPC("UseKey", RpcTarget.All);
                
            }
            
        }
    }

}
