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
    private Animator animator;
    Vector2 moveVec = Vector2.zero;

    public float gravity = 1f;

    public float speed = 4.0f;
    public float jumpPower = 5.0f;
    public float size = 1.0f;
    public bool isObserve;
    public int observeNumber;
    public Text text;
    public KeyScript key = null;
    private GameObject doorObj = null;             // 현재 관전중인 플레이어 번호
    public bool isShoot;                        // 발사(캐릭터가 직선으로 발사됨) // 중력X, 입력X
    public bool isClear = false;


    private void OnEnable() 
    {
        ResetClearCustomProperties();
    }

    void ResetClearCustomProperties()
    {   // CustomProperty로 하려고 했으나 Set하는 데 시간이 걸려서 플레이어 bool변수로 대체
        // Hashtable props = new Hashtable {{ GameData.PLAYER_CLEAR, false }};
        // PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        isObserve = false;
    }

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        rend = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if(GameManager.Instance != null)
        {
            if(GameManager.Instance.players.ContainsKey(photonView.OwnerActorNr) == true)
            {
                GameManager.Instance.players.Remove(photonView.OwnerActorNr);
            }
            GameManager.Instance.players.Add(photonView.OwnerActorNr, this.gameObject);
        }
            

        Debug.Log(string.Format(
            "photonView.OwnerActorNr : {0} / LocalPlayer.ActorNumber : {1}",
            photonView.OwnerActorNr,
            PhotonNetwork.LocalPlayer.ActorNumber)
        );

        photonView.RPC("SetName", RpcTarget.All, photonView.Owner.NickName);
    }

    [PunRPC]
    public void SetName(string str)
    {
        text.text = str;
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
            CheckSide();
            Move();
        }
    }

    void Move()
    {
        if(rigid.velocity.y <= 0)
        {
            animator.SetBool("isJump", false);
            animator.SetBool("isFall", true);
        }
        if (isShoot)
        {
            animator.SetFloat("speed", 0);
            if (rigid.velocity == Vector2.zero)
            {
                ShootStop();
            }
            return;
        }

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
        animator.SetFloat("speed", moveVec.magnitude * speed);
        rigid.position += moveVec * speed * Time.deltaTime;
        if(moveVec.x > 0)
        {
            photonView.RPC("Flip", RpcTarget.All, false);
        }
        else if( moveVec.x < 0)
        {
            photonView.RPC("Flip", RpcTarget.All, true);
        }
        
    }

    [PunRPC]
    private void Flip(bool isflip)
    {
        rend.flipX = isflip;
    }

    private void CheckSide()
    {
        if (animator.GetBool("isJump") == true)
            return;
        GameObject obj = null;
        RaycastHit2D[] downHit = Physics2D.BoxCastAll(transform.position, new Vector2(size, 0.05f), 0, Vector2.down * gravity, size / 2, LayerMask.GetMask("Water"));
        foreach(RaycastHit2D hit in downHit)
        {
            if(hit.collider.tag == "side")
            {
                obj = hit.collider.gameObject;
            }
        }
        if(obj != null)
        {
            Vector2 vec = this.rigid.position;
            vec.y = obj.transform.position.y + size / 2;
            rigid.position = vec;
        }
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
        RaycastHit2D[] downHit = Physics2D.BoxCastAll(transform.position, new Vector2(size, 0.05f), 0, Vector2.down * gravity, size / 2, LayerMask.GetMask("UI", "Water"));
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
        RaycastHit2D[] upHit = Physics2D.BoxCastAll(transform.position, new Vector2(size, 0.05f), 0, Vector2.up * gravity, size / 2, LayerMask.GetMask("UI"));
        count = 0;
        foreach (RaycastHit2D hit in upHit)
        {
            if (hit.collider.isTrigger == false)
            {
                count++;
            }
        }
        if (count <= 1)
        {
            animator.SetBool("isFall", false);
            animator.SetBool("isJump", true);
            rigid.AddForce(Vector2.up * gravity * power, ForceMode2D.Impulse);
        }
        
    }

    public void SuperJump(float multiplier)
    {
        photonView.RPC("HighJump", RpcTarget.All, jumpPower * multiplier);
    }

    [PunRPC]
    void HighJump(float jumpPower)
    {
        rigid.velocity = new Vector2(rigid.velocity.x, 0f);
        rigid.AddForce(Vector2.up * gravity * jumpPower, ForceMode2D.Impulse);
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)  
    {
        if (stream.IsWriting)
        {
            //stream.SendNext(doorObj);
            //stream.SendNext(rigid.position);
        }
        else
        {
            //doorObj = (GameObject)stream.ReceiveNext();
            //rigid.position = (Vector2)stream.ReceiveNext();
        }
    }

    public void ClearStage()
    {
        // Hashtable props = new Hashtable() {{GameData.PLAYER_CLEAR, true}};

        // PhotonNetwork.LocalPlayer.SetCustomProperties(props);


        isClear = true;
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
            Destroy(doorObj);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //TODO: 테그 추가해야함.
        if (collision.gameObject.tag == "Key")
        {
            if(collision.GetComponent<KeyScript>().owner == null)
            {
                //collision.GetComponent<KeyScript>().SetOwner(this);
            }
                
        }
        if (collision.gameObject.tag == "Door")
        {
            if(this.key != null)
            {
                //doorObj = collision.gameObject;
                //photonView.RPC("UseKey", RpcTarget.All);
                
            }
            
        }
    }


    public void Reset()
    {   
        // TODO 열쇠 획득 시 열쇠 해제
        transform.position = GameManager.Instance.spawnPos[0].position;
        //animator.SetTrigger("Die");
    }

    public void ShootInit(Vector2 shootVelocity, Vector2 pos)
    {
        isShoot = true;
        rigid.gravityScale = 0f;
        
        rigid.velocity = shootVelocity;
        transform.position = pos;
    }

    public void ShootStop()
    {
        isShoot = false;
        rigid.gravityScale = gravity;

        rigid.velocity = Vector2.zero;
    }


    public void ReverseGravity()
    {   Debug.Log("ReverseGravity()");
        gravity *= -1f;
        rigid.gravityScale = gravity;
        transform.Rotate(new Vector3(0f, 0f, 180f * gravity), Space.Self); 


        Camera.main.transform.Rotate(new Vector3(0f, 0f, 180f * gravity), Space.Self);   
        Camera.main.transform.localPosition = new Vector3(0f, 0f, -10f);
    }

}
