/*using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using System.Linq;

public class LeaderboardManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI[] playerNameTexts; // 5 UI text elements for names
    public TextMeshProUGUI[] playerScoreTexts; // 5 UI text elements for scores

    DatabaseReference dbReference;
    public static LeaderboardManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }

    }
    void Start()
    {
        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                dbReference = FirebaseDatabase.DefaultInstance.RootReference;
                FetchLeaderboard();
            }
            else
            {
                Debug.LogError("Firebase dependencies not available: " + task.Result);
            }
        });
    }

    void FetchLeaderboard()
    {
        dbReference.Child("players").GetValueAsync().ContinueWithOnMainThread((System.Threading.Tasks.Task<DataSnapshot> snapshotTask) =>
        {
            if (snapshotTask.IsCompleted)
            {
                DataSnapshot snapshot = snapshotTask.Result;
                List<PlayerData> players = new List<PlayerData>();

                foreach (var child in snapshot.Children)
                {
                    string name = child.Child("name").Value != null ? child.Child("name").Value.ToString() : "Unknown";
                    int score = child.Child("Distance").Value != null ? int.Parse(child.Child("Distance").Value.ToString()) : 0;
                    players.Add(new PlayerData(name, score));
                }

                players = players.OrderByDescending(p => p.score).ToList();
                for (int i = 0; i < 5; i++)
                {
                    if (i < players.Count)
                    {
                        playerNameTexts[i].text = players[i].name;
                        playerScoreTexts[i].text = players[i].score.ToString();
                    }
                    else
                    {
                        playerNameTexts[i].text = "-";
                        playerScoreTexts[i].text = "0";
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to fetch leaderboard: " + snapshotTask.Exception);
            }
        });
    }

    // Simple class to store player info
    private class PlayerData
    {
        public string name;
        public int score;
        public PlayerData(string n, int s)
        {
            name = n;
            score = s;
        }
    }
}

*/