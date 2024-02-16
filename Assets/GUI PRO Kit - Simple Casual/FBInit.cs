using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
public class FBInit : MonoBehaviour
{
    public static FBInit instance;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(this.InitCallback, this.OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }
    
    private void InitCallback()
    {
         if (FB.IsInitialized)
         {
             // Signal an app activation App Event
             FB.ActivateApp();
             // Continue with Facebook SDK
             // ...
         }
         else
         {
             Debug.Log("Failed to Initialize the Facebook SDK");
         }
    }

    private void OnHideUnity(bool isGameShown)
    {

    }
}
