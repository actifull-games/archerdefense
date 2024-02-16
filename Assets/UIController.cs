using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
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
    private void Awake()
    {
        instance = this;

        damageUpgrade.level = PlayerPrefs.GetInt("DamageLevel",0);
        rangeUpgrade.level = PlayerPrefs.GetInt("RangeLevel",0);
        speedUpgrade.level = PlayerPrefs.GetInt("SpeedLevel",0);
    }

    private void Start()
    {
        _levelText.text = "Level " + PlayerPrefs.GetInt("Level", 1);
        costFance.text = GameManager.instance.fanceCost.ToString();
        costNear.text = GameManager.instance.nearCost.ToString();
        CostFurther.text = GameManager.instance.furtherCost.ToString();
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
        _moneyText.text = GameManager.instance.playerMoney.ToString();
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
            GameManager.instance.PlusPlayerMoney(money);
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
            GameManager.instance.PlusPlayerMoney(money);
            GamePanel.SetActive(false);
            WinPanel.SetActive(true);
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 1) + 1);
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

}
