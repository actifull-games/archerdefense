using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTrigger : MonoBehaviour
{
    public TowerController _tower;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            _tower.AddEnemy(other.transform); 
        }
    }
}
