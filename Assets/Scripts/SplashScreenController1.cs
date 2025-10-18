using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreenController1 : MonoBehaviour
{
    [Tooltip("Name of the next scene to load after splash")]
    public string sceneToLoad = "MainScene";

    [Tooltip("How long (in seconds) the splash screen stays visible")]
    public float displayTime = 3f;

    [Tooltip("Optional: fade effect")]
    public CanvasGroup fadeGroup;

    public static SplashScreenController1 instance;

    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        StartCoroutine(SplashRoutine());
    }

    private IEnumerator SplashRoutine()
    {
        
        if (fadeGroup)
        {
            fadeGroup.alpha = 0;
            float t = 0;
            while (t < 1f)
            {
                t += Time.deltaTime;
                fadeGroup.alpha = t; 
                yield return null;
            }
        }

       
        yield return new WaitForSeconds(displayTime);

        
        if (fadeGroup)
        {
            float t = 1;
            while (t > 0f)
            {
                t -= Time.deltaTime;
                fadeGroup.alpha = t; 
                yield return null;
            }
        }

        
        SceneManager.LoadScene(sceneToLoad);
    }
}
