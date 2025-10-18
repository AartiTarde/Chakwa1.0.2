using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuScript : MonoBehaviour
{
    public static MainMenuScript instance;
    void Awake()
    {
        if (instance != null)
        {
            instance=this;
        }
    }
    void Start()
    {
        
    }
    public void startScene()
    {
        SceneManager.LoadScene("New Scene");
    }
}
