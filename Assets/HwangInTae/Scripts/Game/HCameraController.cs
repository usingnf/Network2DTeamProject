using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HCameraController : MonoBehaviourPun
{
    public Transform target;
    public float speed = 10.0f;
    float posZ = -10f;

    private void Start()
    {

    }

    private void Update()
    {
    }
  

    public void SetTarget(Transform _target)
    {
        StartCoroutine(MovePos(_target));
    }
    IEnumerator MovePos(Transform _target)
    {
        yield return new WaitForFixedUpdate();
        while(Vector3.Distance(target.position, _target.position) > 1f)
        {
            Debug.Log("작동");
            target.Translate(new Vector3(_target.position.x * Time.deltaTime * speed, _target.position.y * Time.deltaTime * speed, posZ));
        }
    }
}
