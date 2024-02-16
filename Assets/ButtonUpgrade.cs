using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUpgrade : MonoBehaviour
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
        if (GameManager.instance.playerMoney >= cost)
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
        if (GameManager.instance.playerMoney >= cost)
        {
            GameManager.instance.MinusMoney(cost);
            level++;

            if (isScaleFactor)
            {
                GameManager.instance.scaleFactor += 0.01f;
            }
            UIController.instance.CheckMoneyUpgrade();
            GameManager.instance.SetUpgradeLevels(true);
            SetCost();
        }
    }
   
}
