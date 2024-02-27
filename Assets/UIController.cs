using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Game;
using MobileFramework;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : GameBehaviour<ArchersGameRules>
{
    public static UIController instance;
    public bool ShowCursor;
    public int currentLeveHierarhyID;
    public TMP_Text _levelText,_moneyText,_winMoneyText,_failMoneyText;
    
    public Slider powetSlider;
    public TMP_Text powerCount;

    public GameObject MenuPanel, GamePanel, WinPanel, FailPanel;

    public ButtonUpgrade damageUpgrade, rangeUpgrade, speedUpgrade;

    public TMP_Text costFance, costNear, CostFurther;
    
    public Image cursor;

    private ArchersGameRules _gameRules; 
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _gameRules = GameManagerBase.Instance.GetGameRules<ArchersGameRules>();
        _levelText.text = "Level " + _gameRules.TypedGameContext.PlayerLevel;

        if (_gameRules.PlayerController is ArchersPlayerController { Context: ArchersPlayerContext context })
        {
            damageUpgrade.level = context.DamageLevel;
            damageUpgrade.SetCost();
            rangeUpgrade.level = context.AttackRangeLevel;
            rangeUpgrade.SetCost();
            speedUpgrade.level = context.AttackSpeedLevel;
            speedUpgrade.SetCost();
        }
        
        _gameRules.TypedGameContext.PropertyChanged += GameContextPropertyChanged;

        costFance.text = GameManager.instance.fanceCost.ToString();
        costNear.text = GameManager.instance.nearCost.ToString();
        CostFurther.text = GameManager.instance.furtherCost.ToString();
    }

    private void GameContextPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if(e.PropertyName == nameof(ArchersGameContext.PlayerMoney))
            SetMoneyText();
        else if (e.PropertyName == nameof(ArchersGameContext.PlayerLevel))
            _levelText.text = "Level " + _gameRules.TypedGameContext.PlayerLevel;
    }

    void Update()
    {
        if (ShowCursor)
        {
            if (cursor != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    cursor.gameObject.SetActive(true);
                }

                if (Input.GetMouseButton(0))
                {
                    cursor.transform.position = Input.mousePosition;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    cursor.gameObject.SetActive(false);
                }
            }
        }
    }
    public void SetSpawn(int id)
    {
        InputController.instance.SetSpawn(id);
    }

    public void SetMoneyText()
    {
        _moneyText.text = GameRules.TypedGameContext.PlayerMoney.ToString();
    }
    public void StartGame()
    {
        GameManager.instance.isStartGame = true;
        MenuPanel.SetActive(false);
        GamePanel.SetActive(true);
    }

    public void CheckMoneyUpgrade()
    {
        damageUpgrade.CheckMoney();
        rangeUpgrade.CheckMoney();
        speedUpgrade.CheckMoney();
    }

    public void SetTextPower(int currentPower, int maxPower)
    {
        powerCount.text = currentPower + "/" + maxPower;
        powetSlider.value = currentPower;
    }

    public void OpenFail()
    {
        if (FailPanel.activeSelf == false)
        {
            int money = GameManager.instance.EnemyDestroyCount;
            _failMoneyText.text = "+" + money.ToString();
            GameRules.TypedGameContext.PlayerMoney += money;
            GamePanel.SetActive(false);
            FailPanel.SetActive(true);
            PlayerPrefs.SetInt("LastPlayerLevel",currentLeveHierarhyID);
        }
    }
    
    public void OpenWin()
    {
        if (WinPanel.activeSelf == false)
        {
            GameManager.instance.isStartGame = true;
            int money = GameManager.instance.EnemyDestroyCount * 2;
            _winMoneyText.text = "+" + money.ToString();
            GameRules.TypedGameContext.PlayerMoney += money;
            GamePanel.SetActive(false);
            WinPanel.SetActive(true);
            GameRules.TypedGameContext.PlayerLevel++;
            PlayerPrefs.SetInt("LastPlayerLevel",currentLeveHierarhyID);
        }
    }

    public void LoadNextLevel()
    {
        
        SceneManager.LoadScene(1);
    }

    public void ReloadLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void UpdatePlayerContext()
    {
        if (GameRules.PlayerController is ArchersPlayerController { Context: ArchersPlayerContext context })
        {
            context.DamageLevel = damageUpgrade.level;
            context.AttackSpeedLevel = speedUpgrade.level;
            context.AttackRangeLevel = rangeUpgrade.level;
            context.ApplyChanges();
        }
    }

}
