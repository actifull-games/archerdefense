using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoFade : MonoBehaviour
{

    public bool isText;
    public Color colorFade;
    void Start()
    {
        if (isText)
        {
            TMP_Text text = GetComponent<TMP_Text>();
            
            text.DOColor(colorFade, 0.8f);
        }
        else
        {
            Image image = GetComponent<Image>();
            image.DOColor(colorFade, 0.8f);
        }
    }

    
}
