/*using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.UI;

//google play games sign v2.0.0
public class GPGSManager : MonoBehaviour
{
    public static GPGSManager Instance { get; private set; }
    private bool isAuthenticated = false;
   // [SerializeField] private Text playerNameText; 
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGPGS();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeGPGS()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(OnSignInResult);
    }

    private void OnSignInResult(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            Debug.Log("Google Play Games sign-in successful.");
            isAuthenticated = true;


            string playerName = PlayGamesPlatform.Instance.localUser.userName;
            Debug.Log("Player Name: " + playerName);

            // Update UI
            //if (playerNameText != null)
            //    playerNameText.text = playerName;
        }
        else
        {
            Debug.LogError("Google Play Games sign-in failed: " + status);
            isAuthenticated = false;
        }
    }

    public bool IsAuthenticated()
    {
        return isAuthenticated;
    }
}*/
/*
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class GPGSManager : MonoBehaviour
{
    public static GPGSManager Instance { get; private set; }
    private bool isAuthenticated = false;

    [Header("UI References")]
    public RawImage playerPhotoUI;       // Assign in Inspector
    public Texture2D defaultAvatar;      // Default avatar
    public TextMeshProUGUI textMeshProUGUI, txt;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGPGS();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        txt.text = Application.identifier;
        Debug.Log("Package Name: " + Application.identifier);
    }

    private void InitializeGPGS()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(OnSignInResult);
    }

    private void OnSignInResult(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            Debug.Log("Google Play Games sign-in successful.");
            isAuthenticated = true;

            string playerName = Social.localUser.userName;
            string playerId = Social.localUser.id;

            Debug.Log("Player Name: " + playerName);
            Debug.Log("Player ID: " + playerId);

            textMeshProUGUI.text = playerName;

            // Save to PlayerPrefs
            PlayerPrefs.SetString("GPGS_PlayerName", playerName);
            //PlayerPrefs.SetInt("Distance", 0);
            PlayerPrefs.Save();
            StartCoroutine(LoadPlayerPhoto());
        }
        else
        {
            Debug.LogError("Google Play Games sign-in failed: " + status);
            isAuthenticated = false;

            // ✅ Use default fallback name
            string defaultName = "Guest";
            textMeshProUGUI.text = defaultName;

            PlayerPrefs.SetString("GPGS_PlayerName", defaultName);
            //PlayerPrefs.SetInt("Distance", 0);
            PlayerPrefs.Save();
           

            // Show default avatar
            if (playerPhotoUI != null && defaultAvatar != null)
                playerPhotoUI.texture = defaultAvatar;
        }
    }

    private IEnumerator LoadPlayerPhoto()
    {
        Texture2D avatar = null;
        float timer = 0f;
        float timeout = 5f;

        while (avatar == null && timer < timeout)
        {
            avatar = Social.localUser.image;
            if (avatar != null) break;

            timer += Time.deltaTime;
            yield return null;
        }

        if (avatar != null)
        {
            Debug.Log("Got avatar! Size: " + avatar.width + "x" + avatar.height);
            playerPhotoUI.texture = avatar;

            
            SaveAvatarToPlayerPrefs(avatar);
        }
        else
        {
            Debug.LogWarning("No avatar found, using default.");
        }
        // Proceed to next scene
       // SceneManager.LoadScene("Scene2");
    }
    private void SaveAvatarToPlayerPrefs(Texture2D avatar)
    {
        if (avatar == null) return;

        byte[] bytes = avatar.EncodeToPNG();
        string base64 = System.Convert.ToBase64String(bytes);
        PlayerPrefs.SetString("PlayerAvatar", base64);
        PlayerPrefs.Save();
        Debug.Log("Avatar saved to PlayerPrefs.");
    }

    public bool IsAuthenticated()
    {
        return isAuthenticated;
    }
}
*/