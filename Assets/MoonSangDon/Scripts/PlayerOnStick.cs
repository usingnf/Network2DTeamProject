using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnStick: MonoBehaviour
{

    Rigidbody2D rigid;
    Animator animator;


    public bool leftMove = false;
    public bool rightMove = false;

    float moveSpeed = 70;
    float jumpPower = 15f;
    bool jumping = false;
    bool jumpState = false;

    bool playerState = false;
    bool sideState = false;
    int sideFlag = 0;
    float sidePower = 0.12f;
    float sideSpeed = 0;


    // Use this for initialization
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && !jumpState)
        {
            jumping = true;
            jumpState = true;
        }
        Move();
        Jump();
        if (sideState && playerState)
            SideMove();
    }
    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            animator.SetBool("Direction", false);
            moveVelocity = new Vector3(-0.10f, 0, 0);
            playerState = false;


        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            animator.SetBool("Direction", true);
            moveVelocity = new Vector3(+0.10f, 0, 0);
            playerState = false;
        }
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            moveVelocity = new Vector3(0, 0, 0);
            playerState = true;
        }
        transform.position += moveVelocity * moveSpeed * Time.deltaTime;
    }
    void Jump()
    {

        if (!jumping)
            return;
        rigid.velocity = Vector2.zero;
        Vector2 jumpVelocity = new Vector2(0, jumpPower);
        rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);
        jumping = false;
        playerState = false;
    }
    void SideMove()
    {
        Vector3 moveVelocity = Vector3.zero;

        if (this.sideFlag == 1)
        {
            moveVelocity = new Vector3(sidePower, 0, 0);
        }
        else
        {
            moveVelocity = new Vector3(-sidePower, 0, 0);
        }
        transform.position += moveVelocity * sideSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "goldCoin")
        {
            collision.gameObject.SetActive(false);
            //Destroy(collision.gameObject);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 && (transform.position.y - collision.transform.position.y) > 0.5)
        {
            jumpState = false;
        }
        if (collision.gameObject.tag == "side" && (transform.position.y - collision.transform.position.y) > 0.5)
        {
            sideState = true;
            sideFlag = collision.gameObject.GetComponent<BlockMover>().moveFlag;
            sideSpeed = collision.gameObject.GetComponent<BlockMover>().moveSpeed;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "side" && (transform.position.y - collision.transform.position.y) > 0.5)
        {
            sideFlag = collision.gameObject.GetComponent<BlockMover>().moveFlag;
            sideSpeed = collision.gameObject.GetComponent<BlockMover>().moveSpeed;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "side")
        {
            sideState = false; ;
        }
    }
}

