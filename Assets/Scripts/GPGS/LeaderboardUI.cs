using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class LeaderboardUI : MonoBehaviour
{
    DatabaseReference dbRef;

    [Header("UI Elements")]
    public TextMeshProUGUI[] nameTexts = new TextMeshProUGUI[5];
    public TextMeshProUGUI[] scoreTexts = new TextMeshProUGUI[5];

    private string databaseUrl = "https://chakwa-c2a3d-default-rtdb.firebaseio.com/";

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("Firebase ready for Leaderboard UI");
                dbRef = FirebaseDatabase.GetInstance(databaseUrl).RootReference;
                LoadLeaderboard();
            }
            else
            {
                Debug.LogError("Could not resolve Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    public void LoadLeaderboard()
    {
        dbRef.Child("users").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to load leaderboard: " + task.Exception);
                return;
            }

            if (task.Result == null || !task.Result.HasChildren)
            {
                Debug.LogWarning("No users found in database");
                return;
            }

            // Collect players
            List<PlayerEntry> players = new List<PlayerEntry>();

            foreach (var child in task.Result.Children)
            {
                string name = child.Child("displayName").Value?.ToString() ?? "Unknown";
                int score = 0;
                int.TryParse(child.Child("score").Value?.ToString(), out score);

                players.Add(new PlayerEntry(name, score));
            }

            players = players.OrderByDescending(p => p.score).ToList();


            for (int i = 0; i < 5; i++)
            {
                if (i < players.Count)
                {
                    nameTexts[i].text = players[i].name;
                    scoreTexts[i].text = players[i].score.ToString();
                }
                else
                {
                    nameTexts[i].text = "-";
                    scoreTexts[i].text = "0";
                }
            }
        });
    }
}

public class PlayerEntry
{
    public string name;
    public int score;

    public PlayerEntry(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}
