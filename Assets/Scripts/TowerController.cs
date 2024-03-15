using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using DG.Tweening;
using Game;
using MobileFramework;
using UnityEngine;
using UnityEngine.UI;

public class TowerController : GameBehaviour<ArchersGameRules>
{
    public static TowerController instance;

    public enum AnimationStateE
    {
        idle,
        attak,
        dead,
        win
    }
    
    public AnimationStateE _stateAnim;
    
    public int Health;
    public int Damage;
    public float distanceToEnemy;
    public float attackTimeEvent,attakSpeed;
    
    public AnimancerComponent _TowerPlayerAnimancer;
    public AnimationClip idle, attak, dead, win;
    public Transform _Enemy,towerVisual;
    
    public Shoot _shooter;

    private bool isDead;
    private int currentID=-1;


    private bool isSetUpgrades;

    public Transform distanceArcherVisual;
    private List<Transform> _enemys = new List<Transform>();

    private float startDistance;
    public ParticleSystem UpgradeParticele,WinParticle,FailParticle;

    public GameObject healthBarGO;
    private float timeBarActive;
    private bool isActiveBar;
    
    private float characterScale=1.5f;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _TowerPlayerAnimancer.Play(idle, 0.25f);
        startDistance = distanceToEnemy;
        //Debug.Log(Vector3.one*(characterScale + GameManager.instance.scaleFactor));
    }

    public void AddEnemy(Transform enemy)
    {
        _enemys.Add(enemy);
    }

    public void MinusHealth(int damage)
    {
        if (!GameManager.instance.isGameFailed)
        {
            Health -= damage;
            
            timeBarActive = 0f;
            isActiveBar = true;
            healthBarGO.SetActive(true);
            
            if (Health <= 0)
            {
                Health = 0;
                isDead = true;
                //_health.value = Health;
                PlayFail();
                GameManager.instance.isGameFailed = true;
            }
            else
            {
                //_health.value = Health;
            }
        }
    }

    public void SetUpgrade(bool isButtonDown = false)
    {
        var controller = (ArchersPlayerController)GameRules.PlayerController;
        float scale = Mathf.Clamp((characterScale + GameManager.instance.scaleFactor), 1.5f, 2.3f);
        _TowerPlayerAnimancer.transform.localScale = Vector3.one * scale;
        controller.Tower.ApplyUpgrades();

        distanceToEnemy = controller.Tower.Stats.AttackRange.CurrentValue;
        Damage = (int)controller.Tower.Stats.Damage.CurrentValue;
        attakSpeed = controller.Tower.Stats.AttackSpeed.CurrentValue;
        distanceArcherVisual.localScale =
            new Vector3(distanceToEnemy / 10f, distanceToEnemy / 10f, distanceToEnemy / 10f);
        if (_shooter != null)
        {
            _shooter.Damage = Damage;
        }
        CameraController.instance.SetCameraSize();
        if (isButtonDown)
        {
            UpgradeParticele.Play(true);
        }

        isSetUpgrades = true;
    }

    private void Update()
    {
        if (GameManager.instance.isStartGame && !isDead)
        {
            if (isActiveBar)
            {
                if (timeBarActive < 3f)
                {
                    timeBarActive += 1f * Time.deltaTime;
                }
                else
                {
                    isActiveBar = false;
                    timeBarActive = 0f;
                    healthBarGO.SetActive(false);
                }
            }
            
            if (!isSetUpgrades)
            {       
                SetUpgrade(false);
            }
        }

        if (_Enemy == null)
        {
            float distance = 100f;
            for (int i = 0; i < _enemys.Count; i++)
            {
                if (_enemys[i] != null)
                {
                    float secondDistance =
                        Vector3.Distance(transform.position, _enemys[i].transform.position);
                    if (distance > secondDistance)
                    {
                        distance = secondDistance;
                        currentID = i;
                    }
                }
            }
            if (_enemys.Count - 1 >= currentID && currentID != -1)
            {
                _Enemy = _enemys[currentID].transform;
            }
        }
        else
        {
            if (_Enemy.GetComponent<Enemy>().Health > 0)
            {
                var moveDirection = _Enemy.position - _TowerPlayerAnimancer.transform.position;
                if (Vector3.Distance(transform.position, _Enemy.position) > distanceToEnemy)
                {

                    var lookPos = moveDirection;
                    lookPos.y = 0;
                    var rotation = Quaternion.LookRotation(lookPos);
                    _TowerPlayerAnimancer.transform.rotation =
                        Quaternion.Slerp(_TowerPlayerAnimancer.transform.rotation, rotation, Time.deltaTime * 10);

                }
                else
                {
                    if (Health > 0)
                    {
                        var lookPos = moveDirection;
                        lookPos.y = 0;
                        var rotation = Quaternion.LookRotation(lookPos);
                        _TowerPlayerAnimancer.transform.rotation =
                            Quaternion.Slerp(_TowerPlayerAnimancer.transform.rotation, rotation, Time.deltaTime * 10);
                        PlayAnimation(AnimationStateE.attak);
                    }
                }

                _shooter.currentEnemy = _Enemy;
                _shooter.StartShoot(true);
            }
            else
            {
                _shooter.StartShoot(false);
                PlayAnimation(AnimationStateE.idle);
                _enemys.Remove(_Enemy);
                _Enemy = null;
            }
        }
    }

    public void StartWinParticle()
    {
        WinParticle.Play(true);
    }

    public void PlayFail()
    {
        FailParticle.Play();
        CameraController.instance.ShakeCamera(4f,0.5f);
        towerVisual.DOLocalMoveY(-3.5f, 3f).OnComplete(CompleteFail);
    }

    void CompleteFail()
    {
        FailParticle.Stop();
        UIController.instance.OpenFail();
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
                    _TowerPlayerAnimancer.Play(idle, 0.25f);
                }
                else
                {
                    if(_TowerPlayerAnimancer!=null) _TowerPlayerAnimancer.Stop();
                }
            }
            if (stateE == AnimationStateE.attak)
            {
                if (attak != null)
                {
                    var eventState = _TowerPlayerAnimancer.Play(attak, 0.25f);
                    eventState.Speed = attakSpeed;
                    eventState.Events.Add(attackTimeEvent, StartShoot);
                }
                else
                {
                    if(_TowerPlayerAnimancer!=null) _TowerPlayerAnimancer.Stop();
                }
            }
            if (stateE == AnimationStateE.dead)
            {
                _TowerPlayerAnimancer.enabled = false;
                if (dead != null)
                {
                    _TowerPlayerAnimancer.Play(dead, 0.25f);
                }
                else
                {
                    if(_TowerPlayerAnimancer!=null) _TowerPlayerAnimancer.Stop();
                }
            }
            
            if (stateE == AnimationStateE.win)
            {
                if (win != null)
                {
                    var eventState = _TowerPlayerAnimancer.Play(win, 0.25f);
                    eventState.Events.OnEnd = PlayIdle;
                }else
                {
                    if(_TowerPlayerAnimancer!=null) _TowerPlayerAnimancer.Stop();
                }
            }

            _stateAnim = stateE;
        }
    }
}
