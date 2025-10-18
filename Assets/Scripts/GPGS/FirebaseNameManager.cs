using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;

public class FirebaseNameManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField nameInput;
    public Button saveButton;
    public TMP_Text infoText;  // optional message output (e.g., "Name saved!")

    private FirebaseAuth auth;
    private FirebaseUser user;

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                user = auth.CurrentUser;

                if (user == null)
                {
                    SignInAnonymously();
                }
                else
                {
                    LoadOrGenerateName();
                }

                if (saveButton != null)
                    saveButton.onClick.AddListener(OnSaveButtonClicked);
            }
            else
            {
                Debug.LogError("❌ Firebase dependencies not available.");
            }
        });
    }

    private void SignInAnonymously()
    {
        auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("❌ Firebase sign-in failed: " + task.Exception);
                return;
            }

            user = task.Result.User;
            LoadOrGenerateName();
        });
    }

    private void LoadOrGenerateName()
    {
        string currentName = PlayerPrefs.GetString("PlayerName", "");

        if (string.IsNullOrEmpty(currentName))
        {
            currentName = GenerateRandomName();
            PlayerPrefs.SetString("PlayerName", currentName);
            PlayerPrefs.Save();
            Debug.Log("✨ Generated new name: " + currentName);
        }
        else
        {
            Debug.Log("🎉 Loaded name from PlayerPrefs: " + currentName);
        }

        if (nameInput != null)
            nameInput.text = currentName;
    }

    public void OnSaveButtonClicked()
    {
        string newName = nameInput.text.Trim();

        if (string.IsNullOrEmpty(newName))
        {
            ShowInfo("❌ Name cannot be empty!");
            return;
        }

        PlayerPrefs.SetString("PlayerName", newName);
        PlayerPrefs.Save();

        ShowInfo("✅ Name saved: " + newName);
        Debug.Log("✅ Player name updated: " + newName);

        // Optional: Update Firebase user display name
        if (user != null)
        {
            var profile = new UserProfile { DisplayName = newName };
            user.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                    Debug.Log("🔥 Firebase display name updated!");
            });
        }
    }

    private string GenerateRandomName()
    {
        string[] adjectives = { "Swift", "Blue", "Silent", "Mighty", "Lucky", "Brave", "Crazy" };
        string[] animals = { "Fox", "Tiger", "Eagle", "Wolf", "Bear", "Shark", "Panda" };
        System.Random rand = new System.Random();
        return $"{adjectives[rand.Next(adjectives.Length)]}{animals[rand.Next(animals.Length)]}{rand.Next(100, 999)}";
    }

    private void ShowInfo(string message)
    {
        if (infoText != null)
        {
            infoText.text = message;
        }
    }
}
