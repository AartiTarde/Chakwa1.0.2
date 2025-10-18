using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class FirebaseSave : MonoBehaviour
{
    DatabaseReference dbRef;

    // 👇 Paste your Firebase Realtime Database URL here
    private string databaseUrl = "https://chakwa-c2a3d-default-rtdb.firebaseio.com/";
    public static FirebaseSave instance;
    void Start()
    {
        // Initialize with explicit database URL
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("✅ Firebase ready for Database");

                dbRef = FirebaseDatabase.GetInstance(databaseUrl).RootReference;

                //SavePlayerToDatabase();



                //SavePlayerToDatabase(3000);
            }
            else
            {
                Debug.LogError("❌ Could not resolve Firebase dependencies: " + dependencyStatus);
            }
        });
    }


    public void SavePlayerToDatabase(int finalScore)
    {
        string uid = PlayerPrefs.GetString("PlayerUID", "");
        string name = PlayerPrefs.GetString("PlayerName", "");

        if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(name))
        {
            Debug.LogError("⚠️ No PlayerPrefs data found! Run FirebaseLogin first.");
            return;
        }

        Debug.Log($"🔄 Saving name {name} and score {finalScore} for UID {uid}");

        // Save both displayName and score
        dbRef.Child("users").Child(uid).Child("displayName").SetValueAsync(name);
        dbRef.Child("users").Child(uid).Child("score").SetValueAsync(finalScore)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("❌ Score save failed: " + task.Exception);
                }
                else if (task.IsCanceled)
                {
                    Debug.LogError("❌ Score save canceled");
                }
                else
                {
                    Debug.Log("✅ Name & Score saved in database");
                }
            });
    }
}
