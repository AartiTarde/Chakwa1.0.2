using UnityEngine;
using System.Runtime.InteropServices;

public static class HapticManager
{

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void _TriggerHaptic(int type);

    [DllImport("__Internal")]
    private static extern void _TriggerVibration();
#endif

   
    public enum HapticType
    {
        Selection = 0,  // Light tap
        Success = 1,    // Success notification
        Warning = 2,    // Warning notification
        Failure = 3,    // Error notification
        Light = 4,      // Light impact
        Medium = 5,     // Medium impact
        Heavy = 6       // Heavy impact
    }

    /// <summary>
    /// Trigger a haptic feedback
    /// </summary>
    public static void PlayHaptic(HapticType type)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
                PlayAndroidHaptic(type);
        #elif UNITY_IOS && !UNITY_EDITOR
                _TriggerHaptic((int)type);
        #else
                Handheld.Vibrate(); // fallback for Editor/Other
        #endif
    }

    /// <summary>
    /// Trigger simple vibration
    /// </summary>
    public static void Vibrate(long milliseconds = 250)
    {
       
        #if UNITY_ANDROID && !UNITY_EDITOR
                try
                {
                    using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    {
                        AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                        AndroidJavaObject vibrator = activity.Call<AndroidJavaObject>("getSystemService", "vibrator");
                        if (vibrator != null)
                        {
                            vibrator.Call("vibrate", milliseconds);
                        }
                    }
                }
                catch { Handheld.Vibrate(); }
            #elif UNITY_IOS && !UNITY_EDITOR
                    _TriggerVibration();
            #else
                    Handheld.Vibrate(); // Editor fallback
            #endif
    }

    #if UNITY_ANDROID && !UNITY_EDITOR
    private static void PlayAndroidHaptic(HapticType type)
    {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject view = activity.Call<AndroidJavaObject>("getWindow").Call<AndroidJavaObject>("getDecorView");

            int feedback = 0; // default LONG_PRESS

            switch (type)
            {
                case HapticType.Light: feedback = 1; break;    // VIRTUAL_KEY
                case HapticType.Medium: feedback = 3; break;   // KEYBOARD_TAP
                case HapticType.Heavy: feedback = 0; break;    // LONG_PRESS
                default: feedback = 0; break;
            }

            view.Call("performHapticFeedback", feedback);
        }
    }
    #endif
}
