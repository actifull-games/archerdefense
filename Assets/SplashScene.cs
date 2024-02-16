using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScene : MonoBehaviour
{
    private float timer;
    private bool isTimeEnd;
    void Start()
    {
        
    }

 
    void Update()
    {
        if (!isTimeEnd)
        {
            if (timer < 2f)
            {
                timer += 1f * Time.deltaTime;
            }
            else
            {
                isTimeEnd = true;
                SceneManager.LoadScene(1);
            }
        }
    }
}
