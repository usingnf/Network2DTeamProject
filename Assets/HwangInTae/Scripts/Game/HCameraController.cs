using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HCameraController : MonoBehaviourPun
{
    public GameObject target;
    public float speed = 10.0f;
    bool shake = false;
    int shakeCount = 0;
    public float shakePower = 0.3f; // 흔들림 파워
    private void Start()
    {

    }
    private void LateUpdate()
    {
        MoveCamera();
        if(shake)
            CameraShake();
    }
    public void MoveCamera()
    {
        if (null == target)
            return;
        if (shake)
            return;
        Vector3 vec = this.transform.position;
        
        vec.z = 0f;
        //vec2.z = -10.0f;
        if(Vector3.Distance(vec, target.transform.position) < 0.1f)
        {
            Vector3 vec2 = target.transform.position;
            vec2.z = -10.0f;
            this.transform.position = vec2;
        }
        else
        {
            Vector3 vec2 = Vector3.Lerp(vec, target.transform.position, Time.deltaTime);
            vec2.z = -10.0f;
            this.transform.position = vec2;
        }

        //Vector3 dir = transform.position - target.transform.position;
        //Vector3  moveVector = new Vector3(dir.x * Time.deltaTime * speed, dir.y * Time.deltaTime * speed, posZ);
        //transform.Translate(moveVector);
    }
    public void FadeIn()
    {
       
    }
    public void FadeOut()
    {

    }
    public void CameraShake()
    {
        shake = true;
        StartCoroutine(Shake());
           
        shakeCount = 0;
    }
    IEnumerator Shake()
    {
        float posX = Random.RandomRange(-shakePower, shakePower);
        float posY = Random.RandomRange(-shakePower, shakePower);
        Vector2 randomDir = new Vector2(posX, posY);
        Vector3 playerPos = transform.position;
        for (shakeCount = 0; shakeCount < 10; shakeCount++)
        {
            Vector3 vec = Vector3.Lerp(playerPos, randomDir, 0.5f);
            vec.z = -10.0f;
            Debug.Log(shakeCount);
            transform.position = vec;
            yield return new WaitForSeconds(0.6f); //한번 주기에 흔들리는 시간
        }

        shake = false;
    }
}
