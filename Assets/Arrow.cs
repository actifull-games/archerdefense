using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage;

 

    public GameObject ParticleDamage;
   

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Enemy")
        {
            other.transform.GetComponent<Enemy>().SetMyDamage(damage);
            Instantiate(ParticleDamage, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
