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
    private float size = 0.8f;
    public bool isObserve;
    public int observeNumber;                   // 현재 관전중인 플레이어 번호
    public Text text;
    public KeyScript key = null;       
    public bool isClear = false;
    public bool isShoot;
    public bool trgJump = false;
    public bool isReady = false;    //황인태 추가
    public bool isStop;                         // 입력 안 받는 상태
    public bool isDead = false;

    public GameObject EmotePos;
    public GameObject[] EmotesObj;
    private GameObject Temp;

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
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        rend = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        if(GameManager.Instance != null)
        {
            if(GameManager.Instance.players.ContainsKey(photonView.OwnerActorNr) == true)
            {
                GameManager.Instance.players.Remove(photonView.OwnerActorNr);
            }
            GameManager.Instance.players.Add(photonView.OwnerActorNr, this.gameObject);

            if (photonView.IsMine)
                GameManager.Instance.myPlayer = this;
        }
        
        if(StageManager.Instance != null)
        {
            StageManager.Instance.onReverseGravity += ReverseGravity;
        }
        
        photonView.RPC("SetName", RpcTarget.All, photonView.Owner.NickName);
        EmoteInit();
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

    private void Move()
    {
        EmoteControl();
        int count = 0;
        RaycastHit2D[] downHit = Physics2D.BoxCastAll(transform.position, new Vector2(size - 0.05f, 0.05f), 0, Vector2.down * gravity, size / 2, LayerMask.GetMask("Player", "Obstacle"));
        foreach (RaycastHit2D hit in downHit)
        {
            if (hit.collider.isTrigger == false)
            {
                count++;
            }
        }
        if (count > 1)
        {
            if(animator.GetBool("isJump") == false)
            {
                if (trgJump == true)
                {
                    trgJump = false;
                    SoundManager.Instance.PlaySound("Crash", transform.position, 1.0f, 1.0f);
                }
            }
        }
        else
        {
            trgJump = true;
        }
        
        if (rigid.velocity.y <= 0)
        {
            animator.SetBool("isJump", false);
            if(rigid.velocity.y <= -0.2f)
            {
                if (animator.GetBool("isFall") == false)
                {
                    trgJump = true;
                }
                animator.SetBool("isFall", true);
            }
        }

        if (isShoot)
        {
            animator.SetFloat("speed", 0);
            if (Mathf.Abs(rigid.velocity.x) < 0.2f)
            {
                ShootStop();
            }
            return;
        }

        moveVec = Vector2.zero;

        if (!isStop)
        {
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
        }

        moveVec = moveVec.normalized;
        animator.SetFloat("speed", moveVec.magnitude * speed);
        rigid.position += moveVec * speed * Time.deltaTime;

        if(moveVec.x * gravity > 0)
        {
            photonView.RPC("Flip", RpcTarget.All, false);
        }
        else if( moveVec.x * gravity < 0)
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
        RaycastHit2D[] downHit = Physics2D.BoxCastAll(transform.position, new Vector2(size, 0.05f), 0, Vector2.down * gravity, size / 2, LayerMask.GetMask("Obstacle"));
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

    private void Observe()
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
        RaycastHit2D[] downHit = Physics2D.BoxCastAll(transform.position, new Vector2(size-0.15f, 0.05f), 0, Vector2.down * gravity, size / 2, LayerMask.GetMask("Player", "Obstacle"));
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
        
        RaycastHit2D[] upHit = Physics2D.BoxCastAll(transform.position, new Vector2(size, 0.05f), 0, Vector2.up * gravity, size / 2, LayerMask.GetMask("Player"));
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
            SoundManager.Instance.PlaySound("Jump0", this.transform.position, 1, 1);
            animator.SetBool("isJump", true);
            trgJump = true;
            animator.SetBool("isFall", false);
            
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

    public void GoalIn()
    {
        if (isClear)
            return;

        isClear = true;

        SoundManager.Instance.PlaySound("Clear", transform.position, 1.0f, 1.0f);
        SetObserveMode();
    }

    public void SetObserveMode()
    {
        isObserve = true;
        rend.enabled = false;
        coll.enabled = false;
        text.enabled = false;
        rigid.velocity = Vector2.zero;
        rigid.gravityScale = 0f;

        if (photonView.IsMine)
        {
            SetObserveCam();
        }
    }

    public void SetObserveCam()
    {   
        observeNumber = photonView.OwnerActorNr;
        ObserveNext(ref observeNumber);
    }

    private void ObserveNext(ref int obNumber)
    {   
        Transform observeTF = GameManager.Instance.GetNextObserveTF(ref obNumber);
        if (observeTF != null)
        {
            Camera.main.GetComponent<HCameraController>().target = observeTF.gameObject;
            Camera.main.transform.SetParent(observeTF);
            //Camera.main.transform.localPosition = new Vector3(0f, 0f, -10f);
        }

    }

    public void TeamGoalIn(int actorNumber)
    {   
        if (!isObserve) return;

        if (observeNumber == actorNumber)
        {
            ObserveNext(ref observeNumber);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
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
            if(isDead == false)
                photonView.RPC("OnReadyCancle", RpcTarget.All);
        }
    }

    [PunRPC]
    public void Return()
    {   
        transform.position = GameManager.Instance.spawnPos[0].position;
        rigid.velocity = Vector2.zero;
        if (key != null)
        {
            key.GetComponent<PhotonView>().RPC("Return", RpcTarget.All);
        }
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

    [PunRPC]
    public void OnGameStart()
    {
        Debug.Log("gs");
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
        this.isDead = true;
        //if (photonView.IsMine == true)
        //PhotonNetwork.Destroy(this.gameObject);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.DestroyAll();
            GameManager.Instance.StartCoroutine(GameManager.Instance.MoveScene(1));
        }
        
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
        Debug.Log("ReverseGravity()");
        gravity *= -1f;
        rigid.gravityScale = gravity;
        transform.Rotate(new Vector3(0f, 0f, 180f * gravity), Space.Self);


        if (!photonView.IsMine) return;

        Camera.main.transform.Rotate(new Vector3(0f, 0f, 180f * gravity), Space.Self);
        Camera.main.transform.localPosition = new Vector3(0f, 0f, -10f);
    }

    public void EmoteInit()
    {
        EmotePos = transform.Find("EmotePos").gameObject;
        for (int i = 0; i < EmotesObj.Length; i++)
        {
            EmotesObj[i].SetActive(false);
        }
    }

    [PunRPC]
    public void EmotePopUp(int Num)
    {
        if (Temp != null)
        {
            return;
        }
        Temp = Instantiate(EmotesObj[Num].gameObject, EmotePos.transform);
        Temp.SetActive(true);
        Destroy(Temp, 3f);
    }
    public void EmoteControl(int num = -1)
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)||num==0)
        {
            photonView.RPC("EmotePopUp", RpcTarget.All, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || num == 1)
        {
            photonView.RPC("EmotePopUp", RpcTarget.All, 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || num == 2)
        {
            photonView.RPC("EmotePopUp", RpcTarget.All, 2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) || num == 3)
        {
            photonView.RPC("EmotePopUp", RpcTarget.All, 3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) || num == 4)
        {
            photonView.RPC("EmotePopUp", RpcTarget.All, 4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6) || num == 5)
        {
            photonView.RPC("EmotePopUp", RpcTarget.All, 5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7) || num == 6)
        {
            photonView.RPC("EmotePopUp", RpcTarget.All, 6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8) || num == 7)
        {
            photonView.RPC("EmotePopUp", RpcTarget.All , 7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9) || num == 8)
        {
            photonView.RPC("EmotePopUp", RpcTarget.All, 8);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0) || num == 9)
        {
            photonView.RPC("EmotePopUp", RpcTarget.All, 9);
        }
    }

}
