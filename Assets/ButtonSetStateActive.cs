using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSetStateActive : MonoBehaviour
{
    public List<Image> buttonImage = new List<Image>();

    public Sprite activeSprite, deactiveSprite;
    
    public void SetActive(int id)
    {
        foreach (var image in buttonImage)
        {
            image.sprite = deactiveSprite;
        }

        buttonImage[id].sprite = activeSprite;
    }
}
