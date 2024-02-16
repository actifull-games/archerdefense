using System.Collections;
using System.Collections.Generic;
using Animancer;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    public int Health;
    public int Damage;
    public float attackTimeEvent,attakSpeed;
    
    public enum CharacterState
    {
        near,
        further
    }

    public CharacterState _state;
    public enum AnimationStateE
    {
        idle,
        walk,
        attak,
        dead,
        win
    }
    
    public AnimationStateE _stateAnim;
    
    public AnimancerComponent _AnimancerComponent;
    public AnimationClip idle, walk, attak, dead,win;
    
    private CharacterController _characterController;
    public float walkSpeed;
    
    
    public Transform _Enemy, _Base;
    public float distanceToEnemy = 1f;
    
    [HideInInspector] public bool isDead;
    
    public List<HighLight> _HighLights = new List<HighLight>();
    private bool isChancaMaterials;
    private int currentID=-1;
    public bool IsFanceDetected;

    public LayerMask fanceLayerMask;

    public AIPath _AIPath;

    public Shoot _shooter;

    private bool isSetUpgrades;
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _AnimancerComponent = GetComponent<AnimancerComponent>();
        _AIPath.speed = walkSpeed;
        _AnimancerComponent.Play(idle, 0.25f);
        if (_shooter != null)
        {
            _shooter.Damage = Damage;
        }
    }


    void Update()
    {
        if (GameManager.instance.isStartGame)
        {
            if (!isSetUpgrades)
            {
                distanceToEnemy += GameManager.instance.rangeLevel;
                Damage =Damage+(int)Mathf.Pow(1.2f, GameManager.instance.damageLevel);
                if (GameManager.instance.speedLevel > 0)
                {
                    attakSpeed = attakSpeed + (Mathf.Pow(1.1f, GameManager.instance.speedLevel) - 1);
                }

                if (_shooter != null)
                {
                    _shooter.Damage = Damage;
                }
                isSetUpgrades = true;
            }
        }
        if (!isDead)
        {
            if (_Enemy == null)
            {
                Enemy[] listEnemys = FindObjectsOfType<Enemy>();

                float distance = 100f;

                for (int i = 0; i < listEnemys.Length; i++)
                {

                    if (listEnemys[i].isDead ==false)
                    {
                        float secondDistance =
                            Vector3.Distance(transform.position, listEnemys[i].transform.position);
                        if (distance > secondDistance)
                        {
                            distance = secondDistance;
                            currentID = i;
                        }
                    }

                }

                if (listEnemys.Length - 1 >= currentID && currentID != -1)
                {
                    _Enemy = listEnemys[currentID].transform;
                }
                else
                {
                    _AIPath.canMove = false;
                }
            }
            else
            {
                if (_state == CharacterState.near)
                {
                    if (_Enemy.GetComponent<Enemy>().Health > 0)
                    {
                        if (!IsFanceDetected)
                        {
                            var moveDirection = _Enemy.position - transform.position;
                            if (moveDirection.magnitude > distanceToEnemy)
                            {
                               
                                var lookPos = moveDirection;
                                lookPos.y = 0;
                                var rotation = Quaternion.LookRotation(lookPos);
                                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);
                                //_characterController.Move(moveDirection.normalized * walkSpeed * Time.deltaTime);
                                if (_Enemy.GetComponent<Enemy>().CheckVisibility())
                                {
                                    _AIPath.canMove = true;
                                    _AIPath.destination=_Enemy.position;
                                    if (_AIPath.velocity.magnitude <= 0f)
                                    {
                                        PlayAnimation(AnimationStateE.idle);
                                    }
                                    else
                                    {
                                        PlayAnimation(AnimationStateE.walk);
                                    }
                                }
                                RaycastHit hit;

                                if (Physics.Linecast(transform.position + new Vector3(0f, 0.5f, 0f),
                                    //_Enemy.position + new Vector3(0f, 0.5f, 0f), out hit,
                                    transform.position + transform.forward*1f, out hit,
                                    fanceLayerMask))
                                {
                                    IsFanceDetected = true;
                                    _AIPath.canMove = false;
                                    //_Enemy = null;
                                    return;
                                }
                            }
                            else
                            {
                                if (Health > 0)
                                {
                                    var lookPos = moveDirection;
                                    lookPos.y = 0;
                                    var rotation = Quaternion.LookRotation(lookPos);
                                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);
                                    PlayAnimation(AnimationStateE.attak);
                                }
                            }
                        }
                        else
                        {
                            PlayAnimation(AnimationStateE.idle);
                            _AIPath.canMove = false;
                            //_AIPath.destination=_Enemy.position;
                            Enemy[] listEnemys = FindObjectsOfType<Enemy>();

                            float distance = 100f;

                            for (int i = 0; i < listEnemys.Length; i++)
                            {
                                if (listEnemys[i].Health > 0)
                                {
                                    RaycastHit hit;
                                    if (Physics.Linecast(transform.position + new Vector3(0f, 0.5f, 0f),
                                        listEnemys[i].transform.position + new Vector3(0f, 0.5f, 0f), out hit,
                                        fanceLayerMask))
                                    {
                                        IsFanceDetected = true;
                                    }
                                    else
                                    {
                                        _Enemy = listEnemys[i].transform;
                                        IsFanceDetected = false;
                                        _AIPath.canMove = true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        PlayAnimation(AnimationStateE.idle);
                        _Enemy = null;
                    }
                }
                else
                {
                    if (_Enemy.GetComponent<Enemy>().Health > 0)
                    {
                       
                            var moveDirection = _Enemy.position - transform.position;
                            if (moveDirection.magnitude > distanceToEnemy)
                            {
                                var lookPos = moveDirection;
                                lookPos.y = 0;
                                var rotation = Quaternion.LookRotation(lookPos);
                                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);
                            }
                            else
                            {
                                if (_Enemy.GetComponent<Enemy>().CheckVisibility())
                                {
                                    if (Health > 0)
                                    {
                                        var lookPos = moveDirection;
                                        lookPos.y = 0;
                                        var rotation = Quaternion.LookRotation(lookPos);
                                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation,
                                            Time.deltaTime * 10);
                                        _shooter.currentEnemy = _Enemy;
                                        _shooter.StartShoot(true);
                                        PlayAnimation(AnimationStateE.attak);
                                    }
                                }
                                else
                                {
                                    Enemy[] listEnemys = FindObjectsOfType<Enemy>();

                                    float distance = 100f;

                                    for (int i = 0; i < listEnemys.Length; i++)
                                    {

                                        if (listEnemys[i].Health > 0)
                                        {
                                            if (listEnemys[i].CheckVisibility())
                                            {
                                                _Enemy = listEnemys[i].transform;
                                            }
                                        }
                                    }
                                }
                            }

                        
                    }
                    else
                    {
                        _shooter.StartShoot(false);
                        PlayAnimation(AnimationStateE.idle);
                        _Enemy = null;
                    }
                }
            }
        }

        if (!isDead)
        {
            if (Health <= 0)
            {
                isDead = true;
                _AIPath.canMove = false;
                PlayAnimation(AnimationStateE.dead);
            }
        }
    }

    public void SetMyDamage(int myDamage,Transform enemy)
    {
        if (!isChancaMaterials)
        {
            isChancaMaterials = true;
            StartCoroutine(LightRenderer());
        }

        if (IsFanceDetected)
        {
            IsFanceDetected = false;
        }
        if (_Enemy != null)
        {
            if (Vector3.Distance(transform.position, _Enemy.position) > distanceToEnemy)
            {
                _Enemy = null;
            }
        }
        
        Health -= myDamage;
        if (Health <= 0)
        {
            Health = 0;
            Destroy(gameObject,5f);
        }
    }
    
    IEnumerator LightRenderer()
    {
        foreach (var highLight in _HighLights)
        {
            highLight.SetColorOn();
        }

        yield return new WaitForSeconds(0.1f);
        foreach (var highLight in _HighLights)
        {
            highLight.SetColorOff();
        }

        isChancaMaterials = false;
    }
    
    void DamageEnemy()
    {
        if (_Enemy.GetComponent<Enemy>().Health > 0)
        {
            _Enemy.GetComponent<Enemy>().SetMyDamage(Damage);
        }
        else
        {
            _AIPath.canMove = false;
        }
    }

    void StartShoot()
    {
        PlayAnimation(AnimationStateE.idle);
        if (_Enemy != null)
        {
            if (_Enemy.GetComponent<Enemy>().Health > 0)
            {
                _shooter.ShootProjectile();
            }
        }
    }
    void PlayIdle()
    {
        PlayAnimation(AnimationStateE.idle);
    }
    
    public void PlayAnimation(AnimationStateE stateE)
    {
        if (_stateAnim != stateE)
        {
            if (stateE == AnimationStateE.idle)
            {
                if (idle != null)
                {
                    _AnimancerComponent.Play(idle, 0.25f);
                }
                else
                {
                    if(_AnimancerComponent!=null) _AnimancerComponent.Stop();
                }
            }
            if (stateE == AnimationStateE.walk)
            {
                if (walk != null)
                {
                    _AnimancerComponent.Play(walk, 0.25f);
                }else
                {
                    if(_AnimancerComponent!=null) _AnimancerComponent.Stop();
                }
            }
            if (stateE == AnimationStateE.attak)
            {
                if (attak != null)
                {
                    if (_state == CharacterState.near)
                    {
                        var eventState = _AnimancerComponent.Play(attak, 0.25f);
                        eventState.Speed = attakSpeed;
                        eventState.Events.Add(attackTimeEvent, DamageEnemy);
                    }
                    else
                    {
                        var eventState = _AnimancerComponent.Play(attak, 0.25f);
                        eventState.Speed = attakSpeed;
                        eventState.Events.Add(attackTimeEvent, StartShoot);
                    }
                }
                else
                {
                    if(_AnimancerComponent!=null) _AnimancerComponent.Stop();
                }
            }
            if (stateE == AnimationStateE.dead)
            {
                _characterController.enabled = false;
                if (dead != null)
                {
                    _AnimancerComponent.Play(dead, 0.25f);
                }
                else
                {
                    if(_AnimancerComponent!=null) _AnimancerComponent.Stop();
                }
            }
            
            if (stateE == AnimationStateE.win)
            {
                if (win != null)
                {
                    var eventState = _AnimancerComponent.Play(win, 0.25f);
                    eventState.Events.OnEnd = PlayIdle;
                }else
                {
                    if(_AnimancerComponent!=null) _AnimancerComponent.Stop();
                }
            }

            _stateAnim = stateE;
        }
    }
}
