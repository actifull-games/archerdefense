using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoockAtTarget : MonoBehaviour
{
    public Transform loockGO,target;
    
    
    void Start()
    {
        target = Camera.main.transform;
        if (loockGO == null)
        {
            loockGO = transform;
        }
    }


    void LateUpdate()
    {
        if (loockGO.gameObject.activeSelf)
        {
            var lookPos = target.position - loockGO.position;
            lookPos.y = 0f;
            var rotation = Quaternion.LookRotation(lookPos);
            loockGO.rotation = rotation;
            loockGO.eulerAngles +=180f * Vector3.up;
        }
    }
}
