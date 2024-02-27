using System.Collections;
using System.Collections.Generic;
using Game;
using MobileFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUpgrade : GameBehaviour<ArchersGameRules>
{
    public TMP_Text _levelText,_costText;

    public int level;
    public float baseCost, multipluer;

    public Color whiteAlpha, white;
    
    public int cost;

    public bool isScaleFactor;
    void Start()
    {
        SetCost();
    }

    public void CheckMoney()
    {
        var context = GameRules.TypedGameContext;
        if (context.PlayerMoney >= cost)
        {
            GetComponent<Image>().color = white;
        }
        else
        {
            Debug.Log(GameManager.instance.playerMoney+"/"+cost);
            GetComponent<Image>().color = whiteAlpha;
        }
    }
    public void SetCost()
    {
        if (level > 0)
        {
            cost =(int)(baseCost * Mathf.Pow(multipluer, level));
        }
        else
        {
            cost = (int)baseCost;
        }

        _costText.text = cost.ToString();
        CheckMoney();
    }

    public void BuyUpgrade()
    {
        if (GameRules.TypedGameContext.PlayerMoney >= cost)
        {
            GameRules.TypedGameContext.PlayerMoney -= cost;
            level++;

            if (isScaleFactor)
            {
                GameManager.instance.scaleFactor += 0.01f;
            }
            UIController.instance.CheckMoneyUpgrade();
            UIController.instance.UpdatePlayerContext();
            GameManager.instance.SetUpgradeLevels(true);
            SetCost();
        }
    }
   
}
