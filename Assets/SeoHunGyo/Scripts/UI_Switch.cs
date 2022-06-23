using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Switch : MonoBehaviour
{
    [Header("Target")]
    public GameObject   targetObject;
    public bool         isActiveTarget;

    [Header("Self")]
    public GameObject   myObject;
    public bool         isActiveSelf;



    public void OnClicked()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(isActiveTarget);
        }
        
        if (myObject != null)
        {
            myObject.SetActive(isActiveSelf);
        }
        
    }
}
