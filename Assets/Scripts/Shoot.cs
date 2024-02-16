using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform spawnPoint,spawnPoint2;

    public Transform projectile;

    public Transform currentEnemy;

    public ParticleSystem _Particle;
    
    public float timer, reloadTime;
    private bool isStartShoot;

    public bool isShootAtAnimation;
    public int Damage;

    private bool isSpawnOne;
    public bool isPhysics;

    [HideInInspector] public Character myCharacter;
    public void StartShoot(bool isShoot)
    {
        isStartShoot = isShoot;
    }
    void Update()
    {
        if (isStartShoot)
        {
            if (currentEnemy != null)
            {
                if (!isShootAtAnimation)
                {
                    if (timer < reloadTime)
                    {
                        timer += 1f * Time.deltaTime;
                    }
                    else
                    {
                        timer = 0;
                        ShootProjectile();
                    }
                }
            }
        }
    }

    public void ShootProjectile()
    {
        if (spawnPoint2 != null)
        {
            if (isSpawnOne)
            {
                isSpawnOne = false;
                Transform proj = Instantiate(projectile, spawnPoint2.position, spawnPoint2.rotation);
                Vector3 posToMoveMidl = currentEnemy.position + new Vector3(0, 1, 0);

                if (_Particle != null)
                {
                    _Particle.gameObject.SetActive(false);
                    _Particle.gameObject.SetActive(true);
                }

                if (!isPhysics)
                {
                    proj.DOMove(posToMoveMidl, 0.5f).OnComplete(() =>
                    {
                        SetDamageEnemy();
                        Destroy(proj.gameObject);
                    });
                }
            }
            else
            {
                isSpawnOne = true;
                Transform proj = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
                Vector3 posToMoveMidl = currentEnemy.position + new Vector3(0, 1, 0);

                if (_Particle != null)
                {
                    _Particle.gameObject.SetActive(false);
                    _Particle.gameObject.SetActive(true);
                }

                if (!isPhysics)
                {
                    proj.DOMove(posToMoveMidl, 0.5f).OnComplete(() =>
                    {
                        SetDamageEnemy();
                        Destroy(proj.gameObject);
                    });
                }
            }
        }
        else
        {
            Transform proj = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
            Vector3 posToMoveMidl = currentEnemy.position + new Vector3(0, 1, 0);

           
            
            if (_Particle != null)
            {
                _Particle.gameObject.SetActive(false);
                _Particle.gameObject.SetActive(true);
            }
            proj.LookAt(posToMoveMidl);
            if (!isPhysics)
            {
                proj.DOMove(posToMoveMidl, 0.5f).OnComplete(() =>
                {
                    SetDamageEnemy();
                    Destroy(proj.gameObject);
                });
            }
            else
            {
                proj.GetComponent<Arrow>().damage = Damage;
                proj.LookAt(currentEnemy.position);
                Vector3 targetDir = new Vector3(currentEnemy.position.x,currentEnemy.position.y+1f,currentEnemy.position.z) - transform.position;
                proj.GetComponent<Rigidbody>().AddForce(targetDir*300f);
            }
        }
    }

    void SetDamageEnemy()
    {
        if (currentEnemy != null)
        {
            currentEnemy.GetComponent<Enemy>().SetMyDamage(Damage);
        }
    }
}
