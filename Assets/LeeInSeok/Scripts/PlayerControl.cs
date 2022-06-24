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
    Vector2 moveVec = Vector2.zero;

    public float gravity = 1f;

    public float speed = 4.0f;
    public float jumpPower = 5.0f;
    public float size = 1.0f;
    public bool isObserve;
    public int observeNumber;                   // 현재 관전중인 플레이어 번호
    public Text text;
    public KeyScript key = null;
    private GameObject doorObj = null;             
    public bool isShoot;                        
    public bool isReady = false;    //황인태 추가

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

        if(GameManager.Instance != null)
            GameManager.Instance.players.Add(photonView.OwnerActorNr, this.gameObject);

        Debug.Log(string.Format(
            "photonView.OwnerActorNr : {0} / LocalPlayer.ActorNumber : {1}",
            photonView.OwnerActorNr,
            PhotonNetwork.LocalPlayer.ActorNumber)
        );

        StageManager.Instance.onReverseGravity += ReverseGravity;

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
            Move();
        }
    }

    void Move()
    {
        if (isShoot)
        {
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
        rigid.position += moveVec * speed * Time.deltaTime;
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
        Debug.Log("Jump");
        int count = 0;
        RaycastHit2D[] downHit = Physics2D.BoxCastAll(transform.position, new Vector2(size - 0.05f, 0.05f), 0, Vector2.down * gravity, size / 2, LayerMask.GetMask("UI", "Water"));
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
        if(collision.gameObject.tag == "ReadyPotal")
        {
            if(HInLobby.Instance != null)
                HInLobby.Instance.readyPlayer++;
            HInLobby.Instance.PrintInfo(HInLobby.Instance.readyPlayer / 2 + " / " + PhotonNetwork.CurrentRoom.MaxPlayers);
            if (PhotonNetwork.IsMasterClient)
            {
                if (HInLobby.Instance.readyPlayer/2 >= PhotonNetwork.PlayerList.Length)
                {
                    //start
                    photonView.RPC("OnGameStart", RpcTarget.All);
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ReadyPotal")
        {
            if (HInLobby.Instance != null)
                HInLobby.Instance.readyPlayer--;
            photonView.RPC("OnReadyCancle", RpcTarget.All);
        }
    }

    public void Reset()
    {   
        // TODO 열쇠 획득 시 열쇠 해제
        transform.position = GameManager.Instance.spawnPos[0].position;
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

    [PunRPC]
    public void OnGameStart()
    {
        StartCoroutine("GameStart");
    }
    [PunRPC]
    public void OnReadyCancle()
    {
        StartCoroutine("ReadyCancle");
    }
    private IEnumerator GameStart()
    {
        
        HInLobby.Instance.PrintInfo("전원 준비 완료");
        yield return new WaitForSeconds(0.7f);
        for(int i = GameData.COUNTDOWN; i > 0; i--) {
            HInLobby.Instance.PrintInfo(i.ToString());
            yield return new WaitForSeconds(1.0f);
        }
        if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel(string.Format("StageScene_1"));
    }
    private IEnumerator ReadyCancle()
    {
        StopCoroutine("GameStart");
        HInLobby.Instance.PrintInfo(text.text + " 준비 취소");
        yield return new WaitForSeconds(1.0f);
        HInLobby.Instance.PrintInfo(HInLobby.Instance.readyPlayer / 2 + " / " + PhotonNetwork.CurrentRoom.MaxPlayers);
    }


    public void ReverseGravity()
    {   
        //Debug.Log("ReverseGravity()");
        gravity *= -1f;
        rigid.gravityScale = gravity;
        transform.Rotate(new Vector3(0f, 0f, 180f * gravity), Space.Self); 

        Camera.main.transform.Rotate(new Vector3(0f, 0f, 180f * gravity), Space.Self);   
        Camera.main.transform.localPosition = new Vector3(0f, 0f, -10f);
    }
}
