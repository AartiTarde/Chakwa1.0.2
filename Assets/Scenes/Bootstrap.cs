using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    void Start()
    {
        // Load your real starting scene
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
    }
}
