using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreenController : MonoBehaviour
{
    public float displayTime = 3f; // How long the splash shows (in seconds)
    public string nextSceneName = "MainMenu";

    private bool isLoading = false;

    void Start()
    {
        StartCoroutine(WaitAndLoad());
    }

    IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(displayTime);

        // Prevent loading twice (just in case)
        if (!isLoading)
        {
            isLoading = true;
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
