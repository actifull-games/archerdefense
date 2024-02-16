using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    private Camera cam;
    private void Awake()
    {
        instance = this;
        cam = GetComponent<Camera>();
    }

    public void SetCameraSize()
    {
        cam.orthographicSize = TowerController.instance.distanceToEnemy + 5f;
    }

    public void ShakeCamera(float duration,float strenth)
    {
        transform.DOShakePosition(duration, strenth);
    }
}
