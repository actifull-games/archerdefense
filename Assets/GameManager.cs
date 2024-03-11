using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int playerMoney;
    
    public int maxPowerCount;

    public int fanceCost, nearCost, furtherCost;
    
    public float scaleFactor;
    private InputController _inputController;

    public bool isStartGame,isGameFailed;

    
    public int EnemyCounts,EnemyDestroyCount;

    
    private void Awake()
    {
        instance = this;
        playerMoney = PlayerPrefs.GetInt("Money", 0);
    }

    void Start()
    {
        _inputController = InputController.instance;
        //_inputController._power = maxPowerCount;
        _inputController._needPowerFance = fanceCost;
        _inputController._needPowerNear = nearCost;
        _inputController._needPowerFurther = furtherCost;

        //UIController.instance.powetSlider.maxValue = maxPowerCount;
        //UIController.instance.powetSlider.value = maxPowerCount;
        //UIController.instance.SetTextPower(maxPowerCount,maxPowerCount);
        
        
        UIController.instance.SetMoneyText();

        SetUpgradeLevels();
    }

    public void SetUpgradeLevels(bool isButtonDown=false)
    {
        scaleFactor = PlayerPrefs.GetFloat("ScaleFactor",0);
        
        TowerController.instance.SetUpgrade(isButtonDown);
    }
    
    public void AddEnemyCount(int count)
    {
        EnemyCounts += count;
    }

    public void RemoveEnemy()
    {
        EnemyCounts -= 1;
        EnemyDestroyCount += 1;
        if (EnemyCounts <= 0)
        {
            //StartCoroutine(WaitForWin());
        }
    }

    IEnumerator WaitForWin()
    {
        TowerController.instance.StartWinParticle();
        yield return new WaitForSeconds(2f);
        UIController.instance.OpenWin();
    }
}
