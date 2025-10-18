//using UnityEngine;

//public enum TurnDirection { Left, Right, Both }

//public class TurnPoint : MonoBehaviour
//{
//    public TurnDirection allowedTurn = TurnDirection.Both;
//}
//
/*
using UnityEngine;
using Chakwa;
using Chakwa.Player;
public enum TurnDirection { Left, Right, Both }

[RequireComponent(typeof(Collider))]
public class TurnPoint : MonoBehaviour
{
    [Header("Turn Settings")]
    public TurnDirection allowedTurn = TurnDirection.Both;

    [Tooltip("Distance from center at which player should auto-turn.")]
    public float turnRadius = 2.5f;

    [Tooltip("Optional manual center point (if collider offset).")]
    public Transform turnCenterPoint;

    private Transform player;
    private bool playerInside = false;
    private bool hasTurned = false;
    private int bufferedTurn = 0; // 1 = Left, -1 = Right
    private Vector3 center;
    private float lastDistance = Mathf.Infinity;

    void Start()
    {
        if (Path.Instance != null && Path.Instance.player != null)
            player = Path.Instance.player.transform;
        else
            Debug.LogWarning("[TurnPoint] No player found from Path.Instance!");

        center = turnCenterPoint ? turnCenterPoint.position : transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;
        hasTurned = false;
        bufferedTurn = 0;
        Debug.Log($"[TurnPoint] Player entered {name}. AllowedTurn = {allowedTurn}");
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        hasTurned = false;
        bufferedTurn = 0;
    }

    void Update()
    {
        if (!playerInside || hasTurned || player == null) return;

        // --- 1️⃣ Capture Input (A/D) ---
        if (bufferedTurn == 0)
        {
            if (Input.GetKeyDown(KeyCode.A)) bufferedTurn = 1;
            else if (Input.GetKeyDown(KeyCode.D)) bufferedTurn = -1;
        }

        if (bufferedTurn == 0) return; // no turn input yet

        // --- 2️⃣ Distance Calculation ---
        float distance = Vector3.Distance(player.position, center);

        // --- 3️⃣ Debug Player vs Radius Distance ---
        if (Mathf.Abs(distance - turnRadius) <= 0.05f)
        {
            Debug.Log($"[TurnPoint DEBUG] ✅ Player reached radius boundary! " +
                      $"PlayerDist={distance:F2}, Radius={turnRadius:F2}");
        }

        // --- 4️⃣ Turn when player reaches the radius ---
        if (distance <= turnRadius && !hasTurned)
        {
            bool wantLeft = bufferedTurn == 1;
            bool canTurn = allowedTurn == TurnDirection.Both ||
                           (allowedTurn == TurnDirection.Left && wantLeft) ||
                           (allowedTurn == TurnDirection.Right && !wantLeft);

            if (canTurn)
            {
                Path.Instance.TriggerTurn(wantLeft);
                SoundManager.Instance?.PlaySound(
                    Path.Instance.player.GetComponent<PlayerScript>().SoundDatabase.playerturnSound
                );

                hasTurned = true;
                Debug.Log($"[TurnPoint] TURN triggered {(wantLeft ? "LEFT" : "RIGHT")} at {name}");
            }
            else
            {
                Debug.Log($"[TurnPoint] Invalid turn direction at {name} (Allowed: {allowedTurn})");
            }

            bufferedTurn = 0;
        }

        lastDistance = distance;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 gizmoCenter = turnCenterPoint ? turnCenterPoint.position : transform.position;
        Gizmos.DrawWireSphere(gizmoCenter, turnRadius);
    }
#endif
}
*//*
using UnityEngine;
using Chakwa;
using Chakwa.Player;
public enum TurnDirection { Left, Right, Both }

[RequireComponent(typeof(Collider))]
public class TurnPoint : MonoBehaviour
{
    [Header("Turn Settings")]
    public TurnDirection allowedTurn = TurnDirection.Both;
    public float turnRadius = 5f;
    public Transform turnCenterPoint;

    private Transform player;
    private Collider turnCollider;
    private bool playerInside = false;
    private bool hasTurned = false;
    private int bufferedTurn = 0; 
    private Vector3 center;
    private float lastDistance = Mathf.Infinity;

    void Start()
    {
        turnCollider = GetComponent<Collider>();

        if (Path.Instance != null && Path.Instance.player != null)
            player = Path.Instance.player.transform;
        else
            Debug.LogWarning("[TurnPoint] No player found from Path.Instance!");

        center = turnCenterPoint ? turnCenterPoint.position : turnCollider.bounds.center;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;
        hasTurned = false;
        bufferedTurn = 0;
        lastDistance = Mathf.Infinity;

        Debug.Log($"[TurnPoint] Player entered {name}. AllowedTurn = {allowedTurn}, Radius = {turnRadius}");
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        hasTurned = false;
        bufferedTurn = 0;
    }

    void Update()
    {
        if (!playerInside || hasTurned || player == null) return;

       
        if (bufferedTurn == 0)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                bufferedTurn = 1;
                Debug.Log("[TurnPoint DEBUG] Buffered LEFT turn input");
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                bufferedTurn = -1;
                Debug.Log("[TurnPoint DEBUG] Buffered RIGHT turn input");
            }
        }

        if (bufferedTurn == 0) return;

      
        float distance = Vector3.Distance(player.position, center);
        if (lastDistance > turnRadius && distance <= turnRadius)
        {
            Debug.Log($"[TurnPoint DEBUG] ✅ Player reached radius! Prev={lastDistance:F2}, Now={distance:F2}, Radius={turnRadius}");
        }

        if (distance <= turnRadius && !hasTurned)
        {
            bool wantLeft = bufferedTurn == 1;

            Debug.Log($"[TurnPoint DEBUG] Checking Turn: wantLeft={wantLeft}, allowedTurn={allowedTurn}");

            bool canTurn = allowedTurn == TurnDirection.Both ||
                           (allowedTurn == TurnDirection.Left && wantLeft) ||
                           (allowedTurn == TurnDirection.Right && !wantLeft);

            Debug.Log($"[TurnPoint DEBUG] canTurn={canTurn}");

            if (canTurn)
            {
                Path.Instance.TriggerTurn(wantLeft);

                var playerScript = Path.Instance.player.GetComponent<PlayerScript>();
                if (playerScript != null && playerScript.SoundDatabase != null)
                    SoundManager.Instance?.PlaySound(playerScript.SoundDatabase.playerturnSound);

                hasTurned = true;
                Debug.Log($"[TurnPoint] ✅ TURN TRIGGERED {(wantLeft ? "LEFT" : "RIGHT")} at {name}");
            }
            else
            {
                Debug.Log($"[TurnPoint] ❌ Cannot turn here. Allowed: {allowedTurn}, Input: {(wantLeft ? "LEFT" : "RIGHT")}");
            }

            bufferedTurn = 0;
        }

        lastDistance = distance;
    }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 gizmoCenter = turnCenterPoint ? turnCenterPoint.position : transform.position;
        Gizmos.DrawWireSphere(gizmoCenter, turnRadius);
    }
    #endif
}
*/

using UnityEngine;
using Chakwa;
using Chakwa.Player;
public enum TurnDirection { Left, Right, Both }

[RequireComponent(typeof(Collider))]
public class TurnPoint : MonoBehaviour
{
    [Header("Turn Settings")]
    public TurnDirection allowedTurn = TurnDirection.Both;
    [Tooltip("Distance from center where player should auto-turn")]
    public float turnRadius = 5f;

    [Tooltip("Optional override for center position")]
    public Transform turnCenterPoint;

    [Header("Debug")]
    public bool showDebugLogs = true;
    public bool showRadiusGizmo = true;

    private Transform player;
    private Vector3 center;
    private bool playerInside;
    private bool hasTurned;
    private int bufferedTurn = 0; // 1=left, -1=right
    private float lastDistance = Mathf.Infinity;

    void Start()
    {
        if (Path.Instance != null && Path.Instance.player != null)
            player = Path.Instance.player.transform;
        else
            Debug.LogError("[TurnPoint] ❌ Path.Instance or player not found!");

        // get accurate center
        var col = GetComponent<Collider>();
        center = turnCenterPoint ? turnCenterPoint.position : col.bounds.center;

        if (showDebugLogs)
            Debug.Log($"[TurnPoint] {name} Center={center}  Radius={turnRadius}");
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;
        hasTurned = false;
        bufferedTurn = 0;
        lastDistance = Mathf.Infinity;

        if (showDebugLogs)
            Debug.Log($"[TurnPoint] Player entered {name}. AllowedTurn={allowedTurn}");
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        hasTurned = false;
        bufferedTurn = 0;
    }

    void Update()
    {
        if (!playerInside || hasTurned || player == null) return;

        // get input once
        if (bufferedTurn == 0)
        {
            if (Input.GetKeyDown(KeyCode.A)) bufferedTurn = 1;
            else if (Input.GetKeyDown(KeyCode.D)) bufferedTurn = -1;
        }

        if (bufferedTurn == 0) return;

        float distance = Vector3.Distance(player.position, center);

        if (showDebugLogs)
            Debug.Log($"[TurnPoint DEBUG] {name} → Distance={distance:F2}, Radius={turnRadius}");

        // when crossing radius
        if (lastDistance > turnRadius && distance <= turnRadius)
        {
            if (showDebugLogs)
                Debug.Log($"[TurnPoint DEBUG] ✅ Player reached radius boundary for {name}");
        }

        // perform turn
        if (distance <= turnRadius && !hasTurned)
        {
            bool wantLeft = bufferedTurn == 1;
            bool canTurn = allowedTurn == TurnDirection.Both ||
                           (allowedTurn == TurnDirection.Left && wantLeft) ||
                           (allowedTurn == TurnDirection.Right && !wantLeft);

            if (canTurn)
            {
                if (showDebugLogs)
                    Debug.Log($"[TurnPoint] ✅ TURNING {(wantLeft ? "LEFT" : "RIGHT")} at {name}");

                Path.Instance.TriggerTurn(wantLeft);

                // play sound safely
                var ps = Path.Instance.player.GetComponent<PlayerScript>();
                if (ps?.SoundDatabase != null)
                    SoundManager.Instance?.PlaySound(ps.SoundDatabase.playerturnSound);

                hasTurned = true;
            }
            else
            {
                if (showDebugLogs)
                    Debug.Log($"[TurnPoint] ❌ Invalid direction. Allowed={allowedTurn}");
            }

            bufferedTurn = 0;
        }

        lastDistance = distance;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!showRadiusGizmo) return;

        Gizmos.color = Color.yellow;
        Vector3 gizmoCenter = turnCenterPoint ? turnCenterPoint.position : GetComponent<Collider>().bounds.center;
        Gizmos.DrawWireSphere(gizmoCenter, turnRadius);

        UnityEditor.Handles.Label(gizmoCenter + Vector3.up * 1.2f, $"{name}\nRadius {turnRadius:F1}");
    }
#endif
}
