using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Fance : MonoBehaviour
{
    public int Health;

    private bool isScale;
    public void SetDamage(int damage)
    {
        if (!isScale)
        {
            isScale = true;
            Health -= damage;
            SetScaleOn();
        }
    }

    void SetScaleOn()
    {
        transform.GetChild(0).DOScale(1.2f, 0.1f).OnComplete(SetScaleOff);
    }
    void SetScaleOff()
    {
        transform.GetChild(0).DOScale(1f, 0.1f).OnComplete(SetOff);
    }

    void SetOff()
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
        isScale = false;
    }
    
}
