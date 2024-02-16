using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSlider : MonoBehaviour
{
    public int CurrentLevel;
    public bool isBoss;
    public Color curentLevelColor;
    public List<Image> _levelImages = new List<Image>();
    void Start()
    {
        if (!isBoss)
        {
            _levelImages[CurrentLevel-1].color = curentLevelColor;
            _levelImages[CurrentLevel-1].rectTransform.sizeDelta = new Vector2(50, 50);
        }
        else
        {
            _levelImages[CurrentLevel-1].rectTransform.sizeDelta = new Vector2(70, 70);
        }
    }
}
