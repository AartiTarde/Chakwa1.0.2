/*using UnityEngine;
using System.Collections;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
public class LeaderboardScoreUploder : MonoBehaviour
{
    //public TextMeshProUGUI textMeshProUGUI, score;
    public static LeaderboardScoreUploder Instance;
    DatabaseReference dbReference;
    string userId;
    void Start()
    {
        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                dbReference = FirebaseDatabase.DefaultInstance.RootReference;

                string savedPlayerName = PlayerPrefs.GetString("GPGS_PlayerName");
                //textMeshProUGUI.text = savedPlayerName;


                int scores = PlayerPrefs.GetInt("Distance");
                //score.text = scores.ToString();
                SavePlayerName(savedPlayerName);
                uploadeScore(scores);
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });
    }

    void SavePlayerName(string playerName)
    {

        userId = SystemInfo.deviceUniqueIdentifier;

        dbReference.Child("players").Child(userId).Child("name").SetValueAsync(playerName)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Player name saved successfully!");
                }
                else
                {
                    Debug.LogError("Failed to save player name: " + task.Exception);
                }
            });



    }

    public void uploadeScore(int Distance)
    {
        dbReference.Child("players").Child(userId).Child("Distance").SetValueAsync(Distance)
           .ContinueWithOnMainThread(task =>
           {
               if (task.IsCompleted)
               {
                   Debug.Log("Player name saved successfully!");
               }
               else
               {
                   Debug.LogError("Failed to save player name: " + task.Exception);
               }
           });
    }
}
*/