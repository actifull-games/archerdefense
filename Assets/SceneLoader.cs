using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void Start()
    {

        int level = PlayerPrefs.GetInt("Level", 1);
        if (level < 11)
        {
            SceneManager.LoadScene(level + 1);
        }
        else
        {
            int sceneCount = Random.Range(5, 12);
            SceneManager.LoadScene(sceneCount);
        }
    }
}
