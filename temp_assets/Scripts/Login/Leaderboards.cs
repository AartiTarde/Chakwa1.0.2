/*using System;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Leaderboards;
using Unity.Services.Authentication;
using TMPro;
using Unity.Services.Core;

public class Leaderboards : MonoBehaviour
{
    public static Leaderboards Instance { get; private set; }
    public string leaderboardId = "ChakwaTest";
    public TMP_Text[] names;
    public TMP_Text[] scores;
    public TMP_Text highestScoreText;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    async void Start()
    {
        await EnsureUGSInitializedAsync();
        await ShowTopScoresAsync();
    }
    static async Task EnsureUGSInitializedAsync()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
            await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    public async Task SubmitScoreAsync(long scoreValue)
    {
        await EnsureUGSInitializedAsync();
        try
        {
            var entry = await LeaderboardsService.Instance
                .AddPlayerScoreAsync(leaderboardId, scoreValue);
            Debug.Log($"Score submitted: {entry.Score} by {AuthenticationService.Instance.PlayerName}");
            await ShowTopScoresAsync();
        }
        catch (Exception e)
        {
            Debug.LogError("Submit score failed: " + e.Message);
        }
    }
    async Task ShowTopScoresAsync()
    {
        try
        {
            var resp = await LeaderboardsService.Instance.GetScoresAsync(
                leaderboardId,
                new GetScoresOptions { Limit = names.Length }
            );
            double highest = double.MinValue;
            string highDisplay = "No scores yet";

            for (int i = 0; i < names.Length; i++)
            {
                if (i < resp.Results.Count)
                {
                    var entry = resp.Results[i];
                    names[i].text = entry.PlayerName;
                    scores[i].text = entry.Score.ToString();

                    if (entry.Score > highest)
                    {
                        highest = entry.Score;
                        highDisplay = entry.Score.ToString();
                    }
                }
                else
                {
                    names[i].text = $"{i + 1}. –";
                    scores[i].text = "-";
                }
            }
            highestScoreText.text = highDisplay;
        }
        catch (Exception ex)
        {
            Debug.LogError("Load leaderboard failed: " + ex.Message);
        }
    }
}
*/
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Leaderboards;
using Unity.Services.Authentication;
using TMPro;
using Unity.Services.Core;

public class Leaderboards : MonoBehaviour
{
    public static Leaderboards Instance { get; private set; }

    [Header("Leaderboard Settings")]
    public string leaderboardId = "ChakwaTest";
    public float refreshInterval = 5f; // Time in seconds between leaderboard refreshes

    [Header("UI References")]
    public TMP_Text[] names;
    public TMP_Text[] scores;
    public TMP_Text highestScoreText;

    private Coroutine updateCoroutine;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    async void Start()
    {
        await EnsureUGSInitializedAsync();
        await ShowTopScoresAsync();
        updateCoroutine = StartCoroutine(UpdateLeaderboardPeriodically());
    }

    static async Task EnsureUGSInitializedAsync()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            await UnityServices.InitializeAsync();
        }

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    public async Task SubmitScoreAsync(long scoreValue)
    {
        await EnsureUGSInitializedAsync();
        try
        {
            var entry = await LeaderboardsService.Instance
                .AddPlayerScoreAsync(leaderboardId, scoreValue);

            Debug.Log($"Score submitted: {entry.Score} by {AuthenticationService.Instance.PlayerName}");
            await ShowTopScoresAsync();
        }
        catch (Exception e)
        {
            Debug.LogError("Submit score failed: " + e.Message);
        }
    }

    async Task ShowTopScoresAsync()
    {
        try
        {
            var resp = await LeaderboardsService.Instance.GetScoresAsync(
                leaderboardId,
                new GetScoresOptions { Limit = names.Length }
            );

            double highest = double.MinValue;
            string highDisplay = "No scores yet";

            for (int i = 0; i < names.Length; i++)
            {
                if (i < resp.Results.Count)
                {
                    var entry = resp.Results[i];
                    names[i].text = entry.PlayerName ?? $"Player {i + 1}";
                    scores[i].text = entry.Score.ToString();

                    if (entry.Score > highest)
                    {
                        highest = entry.Score;
                        highDisplay = entry.Score.ToString();
                    }
                }
                else
                {
                    names[i].text = $"{i + 1}. –";
                    scores[i].text = "-";
                }
            }

            highestScoreText.text = highDisplay;
        }
        catch (Exception ex)
        {
            Debug.LogError("Load leaderboard failed: " + ex.Message);
        }
    }

    private IEnumerator UpdateLeaderboardPeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(refreshInterval);
            if (gameObject.activeInHierarchy)
            {
                _ = ShowTopScoresAsync(); // Fire and forget
            }
        }
    }

    public void StopLeaderboardUpdates()
    {
        if (updateCoroutine != null)
        {
            StopCoroutine(updateCoroutine);
            updateCoroutine = null;
        }
    }
}
