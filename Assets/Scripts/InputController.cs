using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using MobileFramework;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController instance;
    
    public bool isCreoInputMouse;
    
    public LayerMask groundLayer;

    public Transform _BaseTransform;
    public GameObject _fancePrefab,_nearPrefab,_furtherPrefab;

    
    public Vector3 firstPoint, secondPoint;
    private Transform lastFance,lastNear,lastFurther;
    public enum SpawnState
    {
        none,
        fance,
        near,
        further
    }

    public SpawnState _spawnState;
    
    public int _needPowerFance, _needPowerNear, _needPowerFurther;

    private ArchersGameRules _gameRules;
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _gameRules = GameManagerBase.Instance.GetGameRules<ArchersGameRules>();
    }

    public void SetSpawn(int state)
    {
        if (state == 0)
        {
            _spawnState = SpawnState.fance;
        }
        if (state == 1)
        {
            _spawnState = SpawnState.near;
        }
        if (state == 2)
        {
            _spawnState = SpawnState.further;
        }
    }

    void Update()
    {
        if (GameManager.instance.isStartGame && !GameManager.instance.isGameFailed)
        {
            var context = _gameRules.PlayerController.GetContext<ArchersPlayerContext>();
            if (_spawnState != SpawnState.none)
            {
                if (isCreoInputMouse)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, 600, groundLayer))
                        {
                            if (hit.transform.name != "DontSpawn")
                            {
                                firstPoint = secondPoint = hit.point;
                            }
                        }
                    }

                    if (Input.GetMouseButton(0))
                    {
                         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            RaycastHit hit;

                            if (Physics.Raycast(ray, out hit, 600, groundLayer))
                            {
                                if (hit.transform.name != "DontSpawn")
                                {
                                    secondPoint = hit.point;
                                    if (Vector3.Distance(firstPoint, secondPoint) >= 1f)
                                    {
                                        if (_spawnState == SpawnState.fance)
                                        {
                                            if (context.PowerCount >= _needPowerFance)
                                            {
                                                GameObject go = Instantiate(_fancePrefab, firstPoint,
                                                    Quaternion.identity);
                                                lastFance = go.transform;
                                                lastFance.LookAt(secondPoint);
                                                firstPoint = secondPoint = hit.point;
                                                SetPowerMinus(_needPowerFance);
                                            }
                                        }

                                        if (_spawnState == SpawnState.near)
                                        {
                                            if (context.PowerCount >= _needPowerNear)
                                            {
                                                firstPoint = secondPoint = hit.point;
                                                GameObject go = Instantiate(_nearPrefab, firstPoint,
                                                    Quaternion.identity);
                                                lastNear = go.transform;
                                                lastNear.LookAt(2 * _BaseTransform.position - lastNear.position);
                                                SetPowerMinus(_needPowerNear);
                                            }
                                        }

                                        if (_spawnState == SpawnState.further)
                                        {
                                            if (context.PowerCount >= _needPowerFurther)
                                            {
                                                firstPoint = secondPoint = hit.point;
                                                GameObject go = Instantiate(_furtherPrefab, firstPoint,
                                                    Quaternion.identity);
                                                lastFurther = go.transform;
                                                lastFurther.LookAt(2 * _BaseTransform.position - lastFurther.position);
                                                SetPowerMinus(_needPowerFurther);
                                            }
                                        }
                                    }
                                }
                            }
                    }

                    if (Input.GetMouseButtonUp(0))
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, 600, groundLayer))
                        {
                            if (hit.transform.name != "DontSpawn")
                            {
                                firstPoint = secondPoint = hit.point;

                                if (_spawnState == SpawnState.near)
                                {
                                    if (context.PowerCount >= _needPowerNear)
                                    {
                                        firstPoint = secondPoint = hit.point;
                                        GameObject go = Instantiate(_nearPrefab, firstPoint, Quaternion.identity);
                                        lastNear = go.transform;
                                        lastNear.LookAt(2 * _BaseTransform.position - lastNear.position);
                                        SetPowerMinus(_needPowerNear);
                                    }
                                }

                                if (_spawnState == SpawnState.further)
                                {
                                    if (context.PowerCount >= _needPowerFurther)
                                    {
                                        firstPoint = secondPoint = hit.point;
                                        GameObject go = Instantiate(_furtherPrefab, firstPoint,
                                            Quaternion.identity);
                                        lastFurther = go.transform;
                                        lastFurther.LookAt(2 * _BaseTransform.position - lastFurther.position);
                                        SetPowerMinus(_needPowerFurther);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (Input.touchCount > 0)
                    {
                        Touch touch = Input.GetTouch(0);
                        if (touch.phase == TouchPhase.Began)
                        {
                            Ray ray = Camera.main.ScreenPointToRay(touch.position);
                            RaycastHit hit;

                            if (Physics.Raycast(ray, out hit, 600, groundLayer))
                            {
                                if (hit.transform.name != "DontSpawn")
                                {
                                    firstPoint = secondPoint = hit.point;
                                }
                            }
                        }

                        if (touch.phase == TouchPhase.Moved)
                        {
                            Ray ray = Camera.main.ScreenPointToRay(touch.position);
                            RaycastHit hit;

                            if (Physics.Raycast(ray, out hit, 600, groundLayer))
                            {
                                if (hit.transform.name != "DontSpawn")
                                {
                                    secondPoint = hit.point;
                                    if (Vector3.Distance(firstPoint, secondPoint) >= 1f)
                                    {
                                        if (_spawnState == SpawnState.fance)
                                        {
                                            if (context.PowerCount >= _needPowerFance)
                                            {
                                                GameObject go = Instantiate(_fancePrefab, firstPoint,
                                                    Quaternion.identity);
                                                lastFance = go.transform;
                                                lastFance.LookAt(secondPoint);
                                                firstPoint = secondPoint = hit.point;
                                                SetPowerMinus(_needPowerFance);
                                            }
                                        }

                                        if (_spawnState == SpawnState.near)
                                        {
                                            if (context.PowerCount >= _needPowerNear)
                                            {
                                                firstPoint = secondPoint = hit.point;
                                                GameObject go = Instantiate(_nearPrefab, firstPoint,
                                                    Quaternion.identity);
                                                lastNear = go.transform;
                                                lastNear.LookAt(2 * _BaseTransform.position - lastNear.position);
                                                SetPowerMinus(_needPowerNear);
                                            }
                                        }

                                        if (_spawnState == SpawnState.further)
                                        {
                                            if (context.PowerCount >= _needPowerFurther)
                                            {
                                                firstPoint = secondPoint = hit.point;
                                                GameObject go = Instantiate(_furtherPrefab, firstPoint,
                                                    Quaternion.identity);
                                                lastFurther = go.transform;
                                                lastFurther.LookAt(2 * _BaseTransform.position - lastFurther.position);
                                                SetPowerMinus(_needPowerFurther);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (touch.phase == TouchPhase.Ended)
                        {
                            Ray ray = Camera.main.ScreenPointToRay(touch.position);
                            RaycastHit hit;

                            if (Physics.Raycast(ray, out hit, 600, groundLayer))
                            {
                                if (hit.transform.name != "DontSpawn")
                                {
                                    firstPoint = secondPoint = hit.point;

                                    if (_spawnState == SpawnState.near)
                                    {
                                        if (context.PowerCount >= _needPowerNear)
                                        {
                                            firstPoint = secondPoint = hit.point;
                                            GameObject go = Instantiate(_nearPrefab, firstPoint, Quaternion.identity);
                                            lastNear = go.transform;
                                            lastNear.LookAt(2 * _BaseTransform.position - lastNear.position);
                                            SetPowerMinus(_needPowerNear);
                                        }
                                    }

                                    if (_spawnState == SpawnState.further)
                                    {
                                        if (context.PowerCount >= _needPowerFurther)
                                        {
                                            firstPoint = secondPoint = hit.point;
                                            GameObject go = Instantiate(_furtherPrefab, firstPoint,
                                                Quaternion.identity);
                                            lastFurther = go.transform;
                                            lastFurther.LookAt(2 * _BaseTransform.position - lastFurther.position);
                                            SetPowerMinus(_needPowerFurther);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void SetPowerPlus(int value = 1)
    {
        var context = _gameRules.PlayerController.GetContext<ArchersPlayerContext>();
        context.PowerCount += value;
    }

    public void SetPowerMinus(int count)
    {
        var context = _gameRules.PlayerController.GetContext<ArchersPlayerContext>();
        context.PowerCount -= count;
    }
}
