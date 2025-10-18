/*using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;

    private bool isInternetReachable = false;
    public float checkInterval = 5f; 

   
   
    void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            StartCoroutine(CheckInternetConnectivity());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator CheckInternetConnectivity()
    {
        while (true)
        {
            // Check the current internet reachability
            NetworkReachability reachability = Application.internetReachability;

            // Determine if the internet is reachable
            if (reachability == NetworkReachability.NotReachable)
            {
                if (isInternetReachable)
                {
                    // Internet just disconnected
                    isInternetReachable = false;
                    OnInternetDisconnected();
                }
            }
            else
            {
                if (!isInternetReachable)
                {
                    // Internet just connected
                    isInternetReachable = true;
                    OnInternetConnected();
                }
            }

           
            yield return new WaitForSeconds(checkInterval);
        }
    }

   
    void OnInternetConnected()
    {
        Debug.Log("Internet is reachable.");
        
    }

    
    void OnInternetDisconnected()
    {
        Debug.LogError("No internet connection.");
    }

 
    public bool IsInternetReachable()
    {
        return isInternetReachable;
    }
}
*/
using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    // Static instance of the NetworkManager
    public static NetworkManager Instance;

    // Boolean to track internet reachability
    private bool isInternetReachable = false;

    // Interval in seconds to check internet connectivity
    public float checkInterval = 5f;

    void Awake()
    {
        // If no instance exists, set this object as the instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scene loads
        }
        else
        {
            // If an instance already exists, destroy this duplicate object
            Destroy(gameObject);
        }

        // Start the coroutine to check for internet connectivity
        StartCoroutine(CheckInternetConnectivity());
    }

    // Coroutine to check internet connectivity
    IEnumerator CheckInternetConnectivity()
    {
        while (true)
        {
            // Check the current internet reachability
            NetworkReachability reachability = Application.internetReachability;

            // Handle internet connection status
            if (reachability == NetworkReachability.NotReachable)
            {
                if (isInternetReachable)
                {
                    // Internet just disconnected
                    isInternetReachable = false;
                    OnInternetDisconnected();
                }
            }
            else
            {
                if (!isInternetReachable)
                {
                    // Internet just connected
                    isInternetReachable = true;
                    OnInternetConnected();
                }
            }

            // Wait for the specified check interval before checking again
            yield return new WaitForSeconds(checkInterval);
        }
    }

    // Called when the internet becomes reachable
    void OnInternetConnected()
    {
        Debug.Log("Internet is reachable.");
    }

    // Called when the internet becomes unreachable
    void OnInternetDisconnected()
    {
        Debug.LogError("No internet connection.");
    }

    // Public method to check if the internet is reachable
    public bool IsInternetReachable()
    {
        return isInternetReachable;
    }
}
