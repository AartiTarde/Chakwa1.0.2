using System;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;

public class FirebaseLogin : MonoBehaviour
{
    FirebaseAuth auth;
    public TMP_Text nameText;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("✅ Firebase ready");
                auth = FirebaseAuth.DefaultInstance;
                SignIn();
            }
            else
            {
                Debug.LogError("❌ Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    void SignIn()
    {
        if (auth.CurrentUser != null)
        {
            Debug.Log("Already signed in as: " + auth.CurrentUser.UserId);

            // Try to load saved name
            string savedName = PlayerPrefs.GetString("PlayerName", "");

            if (string.IsNullOrEmpty(savedName))
            {
                // If no saved name, generate one now
                savedName = GenerateRandomName();
                PlayerPrefs.SetString("PlayerName", savedName);
                PlayerPrefs.Save();
                Debug.Log("✨ Generated new name for existing UID: " + savedName);
            }
            else
            {
                Debug.Log("🎉 PlayerPrefs name: " + savedName);
            }

            if (nameText != null) nameText.text = savedName;
            return;
        }

        // New anonymous sign-in
        auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("❌ SignIn failed: " + task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result.User;
            Debug.Log("✅ Signed in with UID: " + newUser.UserId);

            string randomName = GenerateRandomName();

            PlayerPrefs.SetString("PlayerName", randomName);
            PlayerPrefs.SetString("PlayerUID", newUser.UserId);
            PlayerPrefs.Save();

            Debug.Log("🎉 Random name stored in PlayerPrefs: " + randomName);

            if (nameText != null) nameText.text = randomName;
        });
    }

    private string GenerateRandomName()
    {
        string[] adjectives = { "Swift", "Blue", "Silent", "Mighty", "Lucky", "Brave", "Crazy" };
        string[] animals = { "Fox", "Tiger", "Eagle", "Wolf", "Bear", "Shark", "Panda" };

        System.Random rand = new System.Random();
        return $"{adjectives[rand.Next(adjectives.Length)]}{animals[rand.Next(animals.Length)]}{rand.Next(100, 999)}";
    }
}
