using System.Collections;
using System.Collections.Generic;
using Animancer;
using Attributes;
using DG.Tweening;
using Game;
using MobileFramework;
using MobileFramework.Abilities;
using MobileFramework.Game;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : GameBehaviour<ArchersGameRules>
{
    public bool isBoss;
    public bool ignoreSpeedAnim;
    public int Health;
    public int Damage;
    public float attackTimeEvent,attakSpeed;
    
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
    public AnimationClip idle, walk, attak,win;
    public List<AnimationClip> dead = new List<AnimationClip>();
    private CharacterController _characterController;
    public float walkSpeed;
    
    public Transform _Character,_Fance, _Base;
    public float distanceToCharacter = 1f;
    
    [HideInInspector] public bool isDead;
    
    public List<HighLight> _HighLights = new List<HighLight>();
    private bool isChancaMaterials;
    private int currentID=-1;
    private bool IsFanceDetected;
    
    public LayerMask fanceLayerMask;
    public AIPath _AIPath;
    

    public Transform _canvasSpawner;
    public GameObject _canvasPlus;

    public Camera m_camera;
    private Vector2 screenPos;
    private bool onScreen;

    private Vector3 BasePosition;
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _AnimancerComponent = GetComponent<AnimancerComponent>();
        _AIPath.speed = walkSpeed;
        m_camera = Camera.main;
        BasePosition = new Vector3(Random.Range(_Base.position.x - 2f, _Base.position.x + 2f),
            _Base.position.y,
            Random.Range(_Base.position.z - 2f, _Base.position.z + 2f));
        InitAbilities();
        GameRules.AddEnemy(gameObject);
    }

    private GameplayAbilitySystem _abilitySystem;
    private EnemyStatsAttributes _stats;

    private VitalityAttributes _vitality;

    private void InitAbilities()
    {
        _abilitySystem = GetComponent<GameplayAbilitySystem>();
        if (_abilitySystem == null)
        {
            _abilitySystem = gameObject.AddComponent<GameplayAbilitySystem>();
        }
        _stats = _abilitySystem.AddAttributeSet<EnemyStatsAttributes>((ctx, s) =>
        {
            s.MoveSpeed.BaseValue = walkSpeed;
            s.Damage.BaseValue = Damage;
            s.AttackSpeed.BaseValue = attakSpeed;
            s.Reset();
        });
        _vitality = _abilitySystem.AddAttributeSet<VitalityAttributes>((ctx, s) =>
        {
            s.MaxHealth.BaseValue = Health;
            s.Health.BaseValue = Health;
            s.Reset();
        });
        _vitality.Death.AddListener(OnDeath);
        _vitality.Health.Changed.AddListener(OnDamage);
    }

    private void OnDamage()
    {
        if (!isChancaMaterials)
        {
            isChancaMaterials = true;
            StartCoroutine(LightRenderer());
        }
        
        if (_Character != null)
        {
            if (Vector3.Distance(transform.position, _Character.position) > distanceToCharacter + 1f)
            {
                _Character = null;
            }
        }

        GameObject plusMoneyCanvas = Instantiate(_canvasPlus, _canvasSpawner.position, Quaternion.identity);
        plusMoneyCanvas.transform.DOMoveY(4f, 1.1f).OnComplete(() => { Destroy(plusMoneyCanvas); });
        InputController.instance.SetPowerPlus();
    }

    private void OnDeath()
    {
        isDead = true;
        _AIPath.canMove = false;
        GameManager.instance.RemoveEnemy();
        GameRules.RemoveEnemy(gameObject);
        if (isBoss)
        {
            BossSpawner.instance.isBossDesd = true;
        }

        PlayAnimation(AnimationStateE.dead);
        Destroy(gameObject, 5f);
    }

    private AnimancerState stateWalk;

    void Update()
    {
        if (!GameManager.instance.isGameFailed)
        {
            if (!isDead)
            {
                if (_Fance == null)
                {
                    RaycastHit hit;

                    if (Physics.Linecast(new Vector3(transform.position.x, transform.position.y+0.5f, transform.position.z),transform.position+transform.forward*1f, out hit,  fanceLayerMask))
                    {
                        _Fance = hit.transform;
                    }

                    if (Physics.Linecast(new Vector3(transform.position.x-0.25f, transform.position.y+0.5f, transform.position.z),transform.position+transform.forward*1f, out hit,  fanceLayerMask))
                    {
                        _Fance = hit.transform;
                    }

                    if (Physics.Linecast(new Vector3(transform.position.x-0.25f, transform.position.y+0.5f, transform.position.z),transform.position+transform.forward*1f, out hit,  fanceLayerMask))
                    {
                        _Fance = hit.transform;
                    }
                    
                    if (_Fance == null)
                    {
                        Character[] listEnemys = FindObjectsOfType<Character>();

                        float distance = 5f;

                        for (int i = 0; i < listEnemys.Length; i++)
                        {

                            if (listEnemys[i].Health > 0)
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

                        if (distance <= 2f)
                        {
                            if (isBoss)
                            {
                                RaycastHit raycastHit;
                                if (Physics.Linecast(new Vector3(transform.position.x, 0.5f, transform.position.z),
                                    transform.position+transform.forward * 20f, out raycastHit))
                                {
                                    if (hit.transform != null)
                                    {
                                        if (hit.transform == listEnemys[currentID].transform)
                                        {
                                            _Character = listEnemys[currentID].transform;
                                        }
                                    }
                                }
                            }
                            else if (listEnemys.Length - 1 >= currentID && currentID != -1)
                            {
                                _Character = listEnemys[currentID].transform;
                            }
                        }
                    }

                    var moveDirection = BasePosition - transform.position;
                    if (moveDirection.magnitude > distanceToCharacter + 1)
                    {
                        _AIPath.canMove = true;
                        _AIPath.destination = BasePosition;
                        transform.LookAt(BasePosition);
                        PlayAnimation(AnimationStateE.walk);
                    }
                    else
                    {
                        if (Health > 0)
                        {
                            _AIPath.canMove = false;
                            transform.LookAt(BasePosition);
                            PlayAnimation(AnimationStateE.attak);
                        }
                    }
                }

                if (_Fance != null)
                {
                    if (Health > 0)
                    {
                        _AIPath.canMove = false;
                        PlayAnimation(AnimationStateE.attak);
                    }
                }
                else if (_Character != null)
                {
                    if (_Character.GetComponent<Character>().Health > 0)
                    {
                        var moveDirection = _Character.position - transform.position;
                        if (moveDirection.magnitude > distanceToCharacter)
                        {
                            transform.LookAt(_Character);
                            _AIPath.canMove = true;
                            _AIPath.destination = _Character.position;
                            PlayAnimation(AnimationStateE.walk);
                        }
                        else
                        {
                            if (Health > 0)
                            {
                                _AIPath.canMove = false;
                                transform.LookAt(_Character);
                                PlayAnimation(AnimationStateE.attak);
                            }
                        }
                    }
                    else
                    {
                        _Character = null;
                    }
                }

                if (!ignoreSpeedAnim)
                {
                    if (_stateAnim == AnimationStateE.walk)
                    {
                        stateWalk = _AnimancerComponent.Play(walk, 0.25f);
                        stateWalk.Speed = _characterController.velocity.magnitude / 3f;

                    }
                }
            }
        }
        else
        {
            _AIPath.canMove = false;
            PlayAnimation(AnimationStateE.win);
        }
    }

    public void SetMyDamage(int myDamage)
    {
        
    }

    void AddMoneyFanceAttack()
    {
        GameObject plusMoneyCanvas = Instantiate(_canvasPlus, _canvasSpawner.position,
            Quaternion.identity);
        plusMoneyCanvas.transform.DOMoveY(4f, 1.1f).OnComplete(() =>
        {
            Destroy(plusMoneyCanvas);
        });
        InputController.instance.SetPowerPlus();
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
    
    void SetDamage()
    {
        if (_Fance != null)
        {
            _Fance.GetComponent<Fance>().SetDamage(Damage);
            AddMoneyFanceAttack();
        }else
        if (_Character != null)
        {
            if (_Character.GetComponent<Character>().Health > 0)
            {
                _Character.GetComponent<Character>().SetMyDamage(Damage,transform);
            }
        }else
        {
            TowerController.instance.MinusHealth(Damage);
        }
    }
    public bool CheckVisibility()
    {
        //Check Visibility
 
        screenPos = m_camera.WorldToScreenPoint(transform.position);
        onScreen = screenPos.x > 0f && screenPos.x < Screen.width && screenPos.y > 0f && screenPos.y < Screen.height;
 
        if (onScreen )//&& m_renderer.isVisible)
        {
            return true;
        }
        else
        {
            return false;
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
                    var eventState = _AnimancerComponent.Play(attak, 0.25f);
                    eventState.Speed = attakSpeed;
                    eventState.Events.Add(attackTimeEvent, SetDamage);
                }
                else
                {
                    if(_AnimancerComponent!=null) _AnimancerComponent.Stop();
                }
            }
            if (stateE == AnimationStateE.dead)
            {
                _characterController.enabled = false;
                if (dead.Count>0)
                {
                    _AnimancerComponent.Play(dead[Random.Range(0,dead.Count)], 0.25f);
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
