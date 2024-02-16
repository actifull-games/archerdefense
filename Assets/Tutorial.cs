using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static Tutorial instance;

    public bool isTutorialActive;

    public GameObject ArrowScreenIndicator,MainUIMenu, MainUIGame;
    public GameObject btnNear, btnFurther;
    public GameObject fanceCanvas, nearCanvas, furtherCanvas;
    public GameObject handMainUI1, handMainUI2;

    public int stateTutorial;

    public List<EnemySpawner> _EnemySpawners = new List<EnemySpawner>();
    private void Awake()
    {
        instance = this;
        if (PlayerPrefs.GetInt("Tutorial", 0) == 0)
        {
            isTutorialActive = true;
        }
        else
        {
            isTutorialActive = false;
        }
    }

    void Start()
    {
        if (isTutorialActive)
        {
            ArrowScreenIndicator.SetActive(false);
            MainUIMenu.SetActive(false);
            MainUIGame.SetActive(true);
            btnNear.SetActive(false);
            btnFurther.SetActive(false);
            SetState();
        }
    }

    public void AddState(int id)
    {
        if (isTutorialActive)
        {
            if (stateTutorial < id)
            {
                stateTutorial = id;
                SetState();
            }
        }
    }

    void SetState()
    {
        if (stateTutorial == 0)
        {
            fanceCanvas.SetActive(true);
            GameManager.instance.isStartGame = true;
        }
        if (stateTutorial == 1)
        {
            fanceCanvas.SetActive(false);
            handMainUI1.SetActive(true);
            btnNear.SetActive(true);
        }
        if (stateTutorial == 2)
        {
            nearCanvas.SetActive(true);
            handMainUI1.SetActive(false);
        }
        if (stateTutorial == 3)
        {
            nearCanvas.SetActive(false);
            handMainUI2.SetActive(true);
            btnFurther.SetActive(true);
        }
        if (stateTutorial == 4)
        {
            furtherCanvas.SetActive(true);
            handMainUI2.SetActive(false);
        }
        if (stateTutorial == 5)
        {
            furtherCanvas.SetActive(false);
            ArrowScreenIndicator.SetActive(true);
            PlayerPrefs.SetInt("Tutorial", 1);
            foreach (var spawner in _EnemySpawners)
            {
                spawner.isStartTime = true;
            }
            isTutorialActive = false;
        }
    }
    
}
