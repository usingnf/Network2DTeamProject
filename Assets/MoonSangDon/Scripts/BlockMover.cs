using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMover : MonoBehaviourPun
{

    public int moveFlag = 1;
    public float moveSpeed = 20;
    public bool upPos;
    public bool fowardPos;
    public bool crossPos;
    float movePower = 0.12f;

    // Use this for initialization

    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
            StartCoroutine("BlockMove");
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
            Move();
    }
    void Move()
    {
        if (upPos)//위아래이동
        {
            Vector3 moveVelocity = Vector3.zero;

            if (this.moveFlag == 1)
            {
                moveVelocity = new Vector3(0, movePower, 0);
            }
            else
            {
                moveVelocity = new Vector3(0, -movePower, 0);
            }
            transform.position += moveVelocity * moveSpeed * Time.deltaTime;
        }
        else if (fowardPos)//앞뒤이동
        {
            Vector3 moveVelocity = Vector3.zero;

            if (this.moveFlag == 1)
            {
                moveVelocity = new Vector3(movePower, 0, 0);
            }
            else
            {
                moveVelocity = new Vector3(-movePower, 0, 0);
            }
            transform.position += moveVelocity * moveSpeed * Time.deltaTime;
        }
        else if(crossPos)
        {
            Vector3 moveVelocity = Vector3.zero;

            if (this.moveFlag == 1)
            {
                moveVelocity = new Vector3(movePower, movePower, 0);
            }
            else
            {
                moveVelocity = new Vector3(-movePower, -movePower, 0);
            }
            transform.position += moveVelocity * moveSpeed * Time.deltaTime;
        }
    }
    IEnumerator BlockMove()
    {
        if (moveFlag == 1)
        {
            moveFlag = 2;
        }
        else
        {
            moveFlag = 1;
        }
        yield return new WaitForSeconds(2f);
        StartCoroutine("BlockMove");
    }
}