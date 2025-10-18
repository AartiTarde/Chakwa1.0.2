using System;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using TMPro;

public class SignIn : MonoBehaviour
{
    public static SignIn Instance { get; private set; }
    public string GuestName { get; private set; }
    public TMP_Text signText;
    async void Awake()
    {
        if (Instance == null) Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadOrGenerateGuestName();
        await InitializeAndSignInAsync();
        await SetPlayerNameAsync();
    }
    void LoadOrGenerateGuestName()
    {
        if (PlayerPrefs.HasKey("GuestName"))
        {
            GuestName = PlayerPrefs.GetString("GuestName");
            signText.text = GuestName.ToString();
        }
        else
        {
            GuestName = "Guest" + UnityEngine.Random.Range(1000, 9999);
            PlayerPrefs.SetString("GuestName", GuestName);
            PlayerPrefs.Save();
            signText.text = GuestName.ToString();
        }
        Debug.Log("Guest name: " + GuestName);
    }
    static async Task InitializeAndSignInAsync()
    {
        if (UnityServices.State == ServicesInitializationState.Uninitialized)
            await UnityServices.InitializeAsync();

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        Debug.Log("Signed in anonymously, PlayerID: " + AuthenticationService.Instance.PlayerId);
    }

    async Task SetPlayerNameAsync()
    {
        try
        {
            await AuthenticationService.Instance.UpdatePlayerNameAsync(GuestName);
            Debug.Log("PlayerName set: " + AuthenticationService.Instance.PlayerName);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to set PlayerName: " + e.Message);
        }
    }
}
