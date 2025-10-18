//using System.Collections.Generic;
//using UnityEngine;
//using Chakwa;



//public enum TileType
//{
//    Straight,
//    LeftTurn,
//    RightTurn,
//    DoubleTurn,
//    TIntersection
//}
//public enum Lane
//{
//    Left = -1,
//    Center = 0,
//    Right = 1
//}


//public class Tile : MonoBehaviour
//{
//    [Header("Tile Points")]
//    public Transform entryPoint;
//    public Transform exitPoint;

//    [Header("TIntersection Properties")]
//    public List<Transform> tIntersectionExits = new List<Transform>(2);

//    [Header("Tile Properties")]
//    public TileType tileType;

//    [Header("Optional Path Segments (for DoubleTurn Visuals)")]
//    public List<Transform> pathSegments = new List<Transform>();

//    [Header("Obstacles")]
//    public GameObject hurdel;

//    private bool hasActivatedObstacle = false;


//    [Header("Respawn Points")]
//    public Transform safeBeforeTurn;
//    public Transform safeAfterTurn;


//    public GameObject[] collectSpot;

//    [Tooltip("Transform that represents where the player should respawn on this tile. If empty, uses this transform.")]
//    public Transform checkpointTransform;


//    [Header("Lane Entry Points")]
//    public Transform laneLeftEntry;
//    public Transform laneCenterEntry;
//    public Transform laneRightEntry;

//    [Header("Lane Exit Points")]
//    public Transform laneLeftExit;
//    public Transform laneCenterExit;
//    public Transform laneRightExit;




//    [Header("Turn Lane Midpoints")]
//    public Transform laneLeftMid;
//    public Transform laneCenterMid;
//    public Transform laneRightMid;

//    void Start()
//    {
//        if (hurdel != null)
//            hurdel.SetActive(false);

//        // Delay activation to next frame so tile is fully instantiated
//        Invoke(nameof(ShowHurdelIfNeeded), 0.1f);
//    }
//    private void Awake()
//    {
//        if (checkpointTransform == null) checkpointTransform = transform;
//    }
//    private void OnTriggerEnter(Collider other)
//    {
//        var pm = other.GetComponent<Chakwa.Player.PlayerMovement>();
//        if (pm != null && pm.IsDead())
//        {
//            Debug.Log("[Tile] Player is dead — not updating checkpoint for tile " + gameObject.name);
//            return;
//        }

//        // Prefer entryPoint as the safe respawn pose; fallback to checkpointTransform or tile transform
//        Transform cp = entryPoint != null ? entryPoint : (checkpointTransform != null ? checkpointTransform : transform);

//        if (RespawnManager.Instance != null)
//        {
//            RespawnManager.Instance.SetRespawnPoint(cp.position, cp.rotation);
//            Debug.Log($"[Tile] Checkpoint set -> {cp.position} (tile: {gameObject.name})");
//        }
//        else
//        {
//            Debug.LogWarning("[Tile] RespawnManager.Instance is null — can't set checkpoint.");
//        }
//    }
//    void ShowHurdelIfNeeded()
//    {
//        if (!hasActivatedObstacle && hurdel != null && gameObject.activeInHierarchy)
//        {
//            hurdel.SetActive(true);
//            hasActivatedObstacle = true;
//            Debug.Log("Hurdel activated on tile: " + gameObject.name);
//        }
//    }

//    public Vector3 GetOffset()
//    {
//        return (tileType == TileType.TIntersection && tIntersectionExits.Count > 0)
//            ? tIntersectionExits[0].position - entryPoint.position
//            : exitPoint.position - entryPoint.position;
//    }

//    public bool AllowsTurn()
//    {
//        return tileType == TileType.LeftTurn ||
//               tileType == TileType.RightTurn ||
//               tileType == TileType.TIntersection;
//    }

//    private void OnDrawGizmos()
//    {
//        if (entryPoint != null)
//        {
//            Gizmos.color = Color.green;
//            Gizmos.DrawSphere(entryPoint.position, 0.2f);
//            Gizmos.DrawLine(entryPoint.position, entryPoint.position + entryPoint.forward * 2f);
//#if UNITY_EDITOR
//            UnityEditor.Handles.Label(entryPoint.position + Vector3.up * 0.3f, "Entry");
//#endif
//        }

//        if (tileType == TileType.TIntersection && tIntersectionExits != null)
//        {
//            Gizmos.color = Color.red;
//            for (int i = 0; i < Mathf.Min(tIntersectionExits.Count, 2); i++)
//            {
//                if (tIntersectionExits[i] != null)
//                {
//                    Gizmos.DrawSphere(tIntersectionExits[i].position, 0.2f);
//                    Gizmos.DrawLine(tIntersectionExits[i].position, tIntersectionExits[i].position + tIntersectionExits[i].forward * 2f);
//                    #if UNITY_EDITOR
//                                        UnityEditor.Handles.Label(tIntersectionExits[i].position + Vector3.up * 0.3f, $"Exit {i + 1}");
//                    #endif
//                }
//            }
//        }
//        else if (exitPoint != null)
//        {
//            Gizmos.color = Color.red;
//            Gizmos.DrawSphere(exitPoint.position, 0.2f);
//            Gizmos.DrawLine(exitPoint.position, exitPoint.position + exitPoint.forward * 2f);
//            #if UNITY_EDITOR
//                        UnityEditor.Handles.Label(exitPoint.position + Vector3.up * 0.3f, "Exit");
//            #endif
//        }

//        if (tileType == TileType.DoubleTurn && pathSegments != null && pathSegments.Count > 1)
//        {
//            Gizmos.color = Color.cyan;
//            for (int i = 0; i < pathSegments.Count - 1; i++)
//            {
//                if (pathSegments[i] != null && pathSegments[i + 1] != null)
//                {
//                    Gizmos.DrawLine(pathSegments[i].position, pathSegments[i + 1].position);
//                }
//            }

//            #if UNITY_EDITOR
//                                for (int i = 0; i < pathSegments.Count; i++)
//                                {
//                                    if (pathSegments[i] != null)
//                                        UnityEditor.Handles.Label(pathSegments[i].position + Vector3.up * 0.2f, $"Path {i + 1}");
//                                }
//            #endif
//        }
//    }
//    public Transform GetLaneEntry(Lane lane)
//    {
//        switch (lane)
//        {
//            case Lane.Left: return laneLeftEntry != null ? laneLeftEntry : transform;
//            case Lane.Center: return laneCenterEntry != null ? laneCenterEntry : transform;
//            case Lane.Right: return laneRightEntry != null ? laneRightEntry : transform;
//            default: return transform;
//        }
//    }

//    public Transform GetLaneExit(Lane lane)
//    {
//        switch (lane)
//        {
//            case Lane.Left: return laneLeftExit != null ? laneLeftExit : transform;
//            case Lane.Center: return laneCenterExit != null ? laneCenterExit : transform;
//            case Lane.Right: return laneRightExit != null ? laneRightExit : transform;
//            default: return transform;
//        }
//    }

//}
//using System.Collections.Generic;
//using UnityEngine;
//using Chakwa;

//public enum TileType
//{
//    Straight,
//    LeftTurn,
//    RightTurn,
//    DoubleTurn,
//    TIntersection
//}

//public enum Lane
//{
//    Left = -1,
//    Center = 0,
//    Right = 1
//}

//public class Tile : MonoBehaviour
//{
//    [Header("Tile Points")]
//    public Transform entryPoint;
//    public Transform exitPoint;

//    [Header("TIntersection Properties")]
//    public List<Transform> tIntersectionExits = new List<Transform>(2);

//    [Header("Tile Properties")]
//    public TileType tileType;

//    [Header("Optional Path Segments (for DoubleTurn Visuals)")]
//    public List<Transform> pathSegments = new List<Transform>();

//    [Header("Obstacles")]
//    public GameObject hurdel;
//    private bool hasActivatedObstacle = false;

//    [Header("Respawn Points")]
//    public Transform safeBeforeTurn;
//    public Transform safeAfterTurn;

//    public GameObject[] collectSpot;

//    [Tooltip("Transform that represents where the player should respawn on this tile. If empty, uses this transform.")]
//    public Transform checkpointTransform;

//    // -------------------------------------
//    // Lane Path Points
//    // -------------------------------------
//    [Header("Lane Entry Points")]
//    public Transform laneLeftEntry;
//    public Transform laneCenterEntry;
//    public Transform laneRightEntry;

//    [Header("Lane Mid Points (for turns)")]
//    public Transform laneLeftMid;
//    public Transform laneCenterMid;
//    public Transform laneRightMid;

//    [Header("Lane Exit Points")]
//    public Transform laneLeftExit;
//    public Transform laneCenterExit;
//    public Transform laneRightExit;

//    // -------------------------------------
//    // Initialization
//    // -------------------------------------
//    private void Awake()
//    {
//        if (checkpointTransform == null)
//            checkpointTransform = transform;
//    }

//    void Start()
//    {
//        if (hurdel != null)
//            hurdel.SetActive(false);

//        // Delay activation so tile is fully spawned
//        Invoke(nameof(ShowHurdelIfNeeded), 0.1f);
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        var pm = other.GetComponent<Chakwa.Player.PlayerMovement>();
//        if (pm != null && pm.IsDead())
//        {
//            Debug.Log("[Tile] Player is dead — not updating checkpoint for tile " + gameObject.name);
//            return;
//        }

//        Transform cp = entryPoint != null ? entryPoint :
//                       (checkpointTransform != null ? checkpointTransform : transform);

//        if (RespawnManager.Instance != null)
//        {
//            RespawnManager.Instance.SetRespawnPoint(cp.position, cp.rotation);
//            Debug.Log($"[Tile] Checkpoint set -> {cp.position} (tile: {gameObject.name})");
//        }
//        else
//        {
//            Debug.LogWarning("[Tile] RespawnManager.Instance is null — can't set checkpoint.");
//        }
//    }

//    void ShowHurdelIfNeeded()
//    {
//        if (!hasActivatedObstacle && hurdel != null && gameObject.activeInHierarchy)
//        {
//            hurdel.SetActive(true);
//            hasActivatedObstacle = true;
//            Debug.Log("Hurdel activated on tile: " + gameObject.name);
//        }
//    }

//    // -------------------------------------
//    // Lane Path Helpers
//    // -------------------------------------

//    public Transform GetLaneEntry(Lane lane)
//    {
//        return lane switch
//        {
//            Lane.Left => laneLeftEntry != null ? laneLeftEntry : transform,
//            Lane.Center => laneCenterEntry != null ? laneCenterEntry : transform,
//            Lane.Right => laneRightEntry != null ? laneRightEntry : transform,
//            _ => transform
//        };
//    }

//    public Transform GetLaneMid(Lane lane)
//    {
//        return lane switch
//        {
//            Lane.Left => laneLeftMid,
//            Lane.Center => laneCenterMid,
//            Lane.Right => laneRightMid,
//            _ => null
//        };
//    }

//    public Transform GetLaneExit(Lane lane)
//    {
//        return lane switch
//        {
//            Lane.Left => laneLeftExit != null ? laneLeftExit : transform,
//            Lane.Center => laneCenterExit != null ? laneCenterExit : transform,
//            Lane.Right => laneRightExit != null ? laneRightExit : transform,
//            _ => transform
//        };
//    }

//    /// <summary>
//    /// Get the world-space position along a lane path.
//    /// t = 0 (entry), t = 0.5 (mid), t = 1 (exit)
//    /// </summary>
//    public Vector3 GetLanePosition(Lane lane, float progress)
//    {
//        Transform start = null, mid = null, end = null;

//        switch (lane)
//        {
//            case Lane.Left:
//                start = laneLeftEntry;
//                mid = laneLeftMid != null ? laneLeftMid : laneCenterMid;
//                end = laneLeftExit;
//                break;
//            case Lane.Center:
//                start = laneCenterEntry;
//                mid = laneCenterMid;
//                end = laneCenterExit;
//                break;
//            case Lane.Right:
//                start = laneRightEntry;
//                mid = laneRightMid != null ? laneRightMid : laneCenterMid;
//                end = laneRightExit;
//                break;
//        }

//        if (start == null || end == null) return transform.position;

//        // Quadratic Bezier for smooth curve
//        if (mid != null)
//            return Mathf.Pow(1 - progress, 2) * start.position +
//                   2 * (1 - progress) * progress * mid.position +
//                   Mathf.Pow(progress, 2) * end.position;
//        else
//            return Vector3.Lerp(start.position, end.position, progress);
//    }

//    // -------------------------------------
//    // Tile Info
//    // -------------------------------------
//    public Vector3 GetOffset()
//    {
//        return (tileType == TileType.TIntersection && tIntersectionExits.Count > 0)
//            ? tIntersectionExits[0].position - entryPoint.position
//            : exitPoint.position - entryPoint.position;
//    }

//    public bool AllowsTurn()
//    {
//        return tileType == TileType.LeftTurn ||
//               tileType == TileType.RightTurn ||
//               tileType == TileType.TIntersection;
//    }

//    // -------------------------------------
//    // Debug Gizmos
//    // -------------------------------------
//    private void OnDrawGizmos()
//    {
//        if (entryPoint != null)
//        {
//            Gizmos.color = Color.green;
//            Gizmos.DrawSphere(entryPoint.position, 0.2f);
//            Gizmos.DrawLine(entryPoint.position, entryPoint.position + entryPoint.forward * 2f);
//            #if UNITY_EDITOR
//                UnityEditor.Handles.Label(entryPoint.position + Vector3.up * 0.3f, "Entry");
//            #endif
//        }

//        if (tileType == TileType.TIntersection && tIntersectionExits != null)
//        {
//            Gizmos.color = Color.red;
//            for (int i = 0; i < Mathf.Min(tIntersectionExits.Count, 2); i++)
//            {
//                if (tIntersectionExits[i] != null)
//                {
//                    Gizmos.DrawSphere(tIntersectionExits[i].position, 0.2f);
//                    Gizmos.DrawLine(tIntersectionExits[i].position, tIntersectionExits[i].position + tIntersectionExits[i].forward * 2f);

//                    #if UNITY_EDITOR
//                        UnityEditor.Handles.Label(tIntersectionExits[i].position + Vector3.up * 0.3f, $"Exit {i + 1}");
//                    #endif
//                }
//            }
//        }
//        else if (exitPoint != null)
//        {
//            Gizmos.color = Color.red;
//            Gizmos.DrawSphere(exitPoint.position, 0.2f);
//            Gizmos.DrawLine(exitPoint.position, exitPoint.position + exitPoint.forward * 2f);

//            #if UNITY_EDITOR
//                UnityEditor.Handles.Label(exitPoint.position + Vector3.up * 0.3f, "Exit");
//            #endif
//        }

//        // Draw lane paths
//        Gizmos.color = Color.cyan;
//        foreach (Lane lane in System.Enum.GetValues(typeof(Lane)))
//        {
//            Transform entry = GetLaneEntry(lane);
//            Transform mid = GetLaneMid(lane);
//            Transform exit = GetLaneExit(lane);

//            if (entry != null && exit != null)
//            {
//                if (mid != null)
//                {
//                    Vector3 prev = entry.position;
//                    for (int i = 1; i <= 20; i++)
//                    {
//                        float t = i / 20f;
//                        Vector3 pos = GetLanePosition(lane, t);
//                        Gizmos.DrawLine(prev, pos);
//                        prev = pos;
//                    } 
//                }
//                else
//                {
//                    Gizmos.DrawLine(entry.position, exit.position);
//                }
//            }
//        }
//    }
//}
/*
using System.Collections.Generic;
using UnityEngine;
using Chakwa;

public enum TileType
{
    Straight,
    LeftTurn,
    RightTurn,
    DoubleTurn,
    TIntersection
}

public enum Lane
{
    Left = -1,
    Center = 0,
    Right = 1
}

public class Tile : MonoBehaviour
{
    [Header("Tile Points")]
    public Transform entryPoint;
    public Transform exitPoint;

    [Header("TIntersection Properties")]
    public List<Transform> tIntersectionExits = new List<Transform>(2);

    [Header("Tile Properties")]
    public TileType tileType;

    [Header("Optional Path Segments (for DoubleTurn Visuals)")]
    public List<Transform> pathSegments = new List<Transform>();

    [Header("Obstacles")]
    public GameObject hurdel;
    private bool hasActivatedObstacle = false;

    [Header("Respawn Points")]
    public Transform safeBeforeTurn;
    public Transform safeAfterTurn;

    public GameObject[] collectSpot;

    [Tooltip("Transform that represents where the player should respawn on this tile. If empty, uses this transform.")]
    public Transform checkpointTransform;

    // -------------------------------------
    // Lane Path Points
    // -------------------------------------
    [Header("Lane Entry Points")]
    public Transform laneLeftEntry;
    public Transform laneCenterEntry;
    public Transform laneRightEntry;

    [Header("Lane Mid Points (for turns)")]
    public Transform laneLeftMid;
    public Transform laneCenterMid;
    public Transform laneRightMid;

    [Header("Lane Exit Points")]
    public Transform laneLeftExit;
    public Transform laneCenterExit;
    public Transform laneRightExit;

    // -------------------------------------
    // Lane Width (NEW)
    // -------------------------------------
    [Header("Lane Width Settings")]
    [Tooltip("Fallback lane width if mesh bounds not found.")]
    public float fallbackLaneWidth = 3.5f;
    [Tooltip("Number of lanes (default 3)")]
    public int laneCount = 3;

    // -------------------------------------
    // Initialization
    // -------------------------------------
    private void Awake()
    {
        if (checkpointTransform == null)
            checkpointTransform = transform;
    }

    void Start()
    {
        if (hurdel != null)
            hurdel.SetActive(false);

        // Delay activation so tile is fully spawned
        Invoke(nameof(ShowHurdelIfNeeded), 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        var pm = other.GetComponent<Chakwa.Player.PlayerMovement>();
        if (pm != null && pm.IsDead())
        {
            Debug.Log("[Tile] Player is dead — not updating checkpoint for tile " + gameObject.name);
            return;
        }

        Transform cp = entryPoint != null ? entryPoint :
                       (checkpointTransform != null ? checkpointTransform : transform);

        if (RespawnManager.Instance != null)
        {
            RespawnManager.Instance.SetRespawnPoint(cp.position, cp.rotation);
            Debug.Log($"[Tile] Checkpoint set -> {cp.position} (tile: {gameObject.name})");
        }
        else
        {
            Debug.LogWarning("[Tile] RespawnManager.Instance is null — can't set checkpoint.");
        }
    }

    void ShowHurdelIfNeeded()
    {
        if (!hasActivatedObstacle && hurdel != null && gameObject.activeInHierarchy)
        {
            hurdel.SetActive(true);
            hasActivatedObstacle = true;
            Debug.Log("Hurdel activated on tile: " + gameObject.name);
        }
    }

    // -------------------------------------
    // Lane Width Calculation (NEW)
    // -------------------------------------

    /// <summary>
    /// Dynamically calculates lane width based on the tile's mesh bounds and scale.
    /// </summary>
    public float GetDynamicLaneWidth()
    {
        Renderer rend = GetComponentInChildren<Renderer>();
        if (rend == null)
        {
            Debug.LogWarning($"[Tile] {name}: No Renderer found. Using fallback width.");
            return fallbackLaneWidth;
        }

        // Get the tile’s total visible width in world space
        float totalWidth = rend.bounds.size.x;

        // Divide by lane count
        float laneWidth = totalWidth / Mathf.Max(1, laneCount);

        return laneWidth;
    }

    // -------------------------------------
    // Lane Path Helpers
    // -------------------------------------

    public Transform GetLaneEntry(Lane lane)
    {
        return lane switch
        {
            Lane.Left => laneLeftEntry != null ? laneLeftEntry : transform,
            Lane.Center => laneCenterEntry != null ? laneCenterEntry : transform,
            Lane.Right => laneRightEntry != null ? laneRightEntry : transform,
            _ => transform
        };
    }

    public Transform GetLaneMid(Lane lane)
    {
        return lane switch
        {
            Lane.Left => laneLeftMid,
            Lane.Center => laneCenterMid,
            Lane.Right => laneRightMid,
            _ => null
        };
    }

    public Transform GetLaneExit(Lane lane)
    {
        return lane switch
        {
            Lane.Left => laneLeftExit != null ? laneLeftExit : transform,
            Lane.Center => laneCenterExit != null ? laneCenterExit : transform,
            Lane.Right => laneRightExit != null ? laneRightExit : transform,
            _ => transform
        };
    }

    /// <summary>
    /// Get the world-space position along a lane path (Bezier interpolation)
    /// </summary>
    public Vector3 GetLanePosition(Lane lane, float progress)
    {
        Transform start = null, mid = null, end = null;

        switch (lane)
        {
            case Lane.Left:
                start = laneLeftEntry;
                mid = laneLeftMid != null ? laneLeftMid : laneCenterMid;
                end = laneLeftExit;
                break;
            case Lane.Center:
                start = laneCenterEntry;
                mid = laneCenterMid;
                end = laneCenterExit;
                break;
            case Lane.Right:
                start = laneRightEntry;
                mid = laneRightMid != null ? laneRightMid : laneCenterMid;
                end = laneRightExit;
                break;
        }

        if (start == null || end == null) return transform.position;

        // Quadratic Bezier interpolation
        if (mid != null)
            return Mathf.Pow(1 - progress, 2) * start.position +
                   2 * (1 - progress) * progress * mid.position +
                   Mathf.Pow(progress, 2) * end.position;
        else
            return Vector3.Lerp(start.position, end.position, progress);
    }

    // -------------------------------------
    // Tile Info
    // -------------------------------------
    public Vector3 GetOffset()
    {
        return (tileType == TileType.TIntersection && tIntersectionExits.Count > 0)
            ? tIntersectionExits[0].position - entryPoint.position
            : exitPoint.position - entryPoint.position;
    }

    public bool AllowsTurn()
    {
        return tileType == TileType.LeftTurn ||
               tileType == TileType.RightTurn ||
               tileType == TileType.TIntersection;
    }

    // -------------------------------------
    // Debug Gizmos
    // -------------------------------------
    private void OnDrawGizmos()
    {
        if (entryPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(entryPoint.position, 0.2f);
            Gizmos.DrawLine(entryPoint.position, entryPoint.position + entryPoint.forward * 2f);
#if UNITY_EDITOR
            UnityEditor.Handles.Label(entryPoint.position + Vector3.up * 0.3f, "Entry");
#endif
        }

        if (tileType == TileType.TIntersection && tIntersectionExits != null)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < Mathf.Min(tIntersectionExits.Count, 2); i++)
            {
                if (tIntersectionExits[i] != null)
                {
                    Gizmos.DrawSphere(tIntersectionExits[i].position, 0.2f);
                    Gizmos.DrawLine(tIntersectionExits[i].position, tIntersectionExits[i].position + tIntersectionExits[i].forward * 2f);

#if UNITY_EDITOR
                    UnityEditor.Handles.Label(tIntersectionExits[i].position + Vector3.up * 0.3f, $"Exit {i + 1}");
#endif
                }
            }
        }
        else if (exitPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(exitPoint.position, 0.2f);
            Gizmos.DrawLine(exitPoint.position, exitPoint.position + exitPoint.forward * 2f);
#if UNITY_EDITOR
            UnityEditor.Handles.Label(exitPoint.position + Vector3.up * 0.3f, "Exit");
#endif
        }

        // Draw lane paths
        Gizmos.color = Color.cyan;
        foreach (Lane lane in System.Enum.GetValues(typeof(Lane)))
        {
            Transform entry = GetLaneEntry(lane);
            Transform mid = GetLaneMid(lane);
            Transform exit = GetLaneExit(lane);

            if (entry != null && exit != null)
            {
                if (mid != null)
                {
                    Vector3 prev = entry.position;
                    for (int i = 1; i <= 20; i++)
                    {
                        float t = i / 20f;
                        Vector3 pos = GetLanePosition(lane, t);
                        Gizmos.DrawLine(prev, pos);
                        prev = pos;
                    }
                }
                else
                {
                    Gizmos.DrawLine(entry.position, exit.position);
                }
            }
        }

        // Optional: visualize lane width
        float laneW = GetDynamicLaneWidth();
        Vector3 right = transform.right;
        Vector3 midPos = entryPoint != null ? entryPoint.position : transform.position;
        Gizmos.color = Color.yellow;
        for (int i = -1; i <= 1; i++)
        {
            Vector3 pos = midPos + right * (i * laneW);
            Gizmos.DrawSphere(pos, 0.15f);
        }
    }
}
*/


//using System.Collections.Generic;
//using UnityEngine;
//using Chakwa;

//public enum TileType
//{
//    Straight,
//    LeftTurn,
//    RightTurn,
//    DoubleTurn,
//    TIntersection
//}

//public enum Lane
//{
//    Left = -1,
//    Center = 0,
//    Right = 1
//}

//public class Tile : MonoBehaviour
//{
//    [Header("Tile Points")]
//    public Transform entryPoint;
//    public Transform exitPoint;

//    [Header("TIntersection Properties")]
//    public List<Transform> tIntersectionExits = new List<Transform>(2);

//    [Header("Tile Properties")]
//    public TileType tileType;

//    [Header("Optional Path Segments (for DoubleTurn Visuals)")]
//    public List<Transform> pathSegments = new List<Transform>();

//    [Header("Obstacles")]
//    public GameObject hurdel;
//    private bool hasActivatedObstacle = false;

//    [Header("Respawn Points")]
//    public Transform safeBeforeTurn;
//    public Transform safeAfterTurn;

//    public GameObject[] collectSpot;
//    public Transform checkpointTransform;

//    // -------------------------------------
//    // Lane Path Points
//    // -------------------------------------
//    [Header("Lane Entry Points")]
//    public Transform laneLeftEntry;
//    public Transform laneCenterEntry;
//    public Transform laneRightEntry;

//    [Header("Lane Mid Points (for turns)")]
//    public Transform laneLeftMid;
//    public Transform laneCenterMid;
//    public Transform laneRightMid;

//    // 🔹 NEW: Second middle control points (for S-turns, double curves)
//    [Header("Lane Second Mid Points (for double turns)")]
//    public Transform laneLeftMid2;
//    public Transform laneCenterMid2;
//    public Transform laneRightMid2;

//    [Header("Lane Exit Points")]
//    public Transform laneLeftExit;
//    public Transform laneCenterExit;
//    public Transform laneRightExit;

//    // -------------------------------------
//    // Lane Width
//    // -------------------------------------
//    [Header("Lane Width Settings")]
//    public float fallbackLaneWidth = 3.5f;
//    public int laneCount = 3;

//    // -------------------------------------
//    // Initialization
//    // -------------------------------------
//    private void Awake()
//    {
//        if (checkpointTransform == null)
//            checkpointTransform = transform;
//    }

//    private void Start()
//    {
//        if (hurdel != null)
//            hurdel.SetActive(false);
//        Invoke(nameof(ShowHurdelIfNeeded), 0.1f);
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        var pm = other.GetComponent<Chakwa.Player.PlayerMovement>();
//        if (pm != null && pm.IsDead())
//            return;

//        Transform cp = entryPoint != null ? entryPoint :
//                       (checkpointTransform != null ? checkpointTransform : transform);

//        if (RespawnManager.Instance != null)
//        {
//            RespawnManager.Instance.SetRespawnPoint(cp.position, cp.rotation);
//        }
//    }

//    private void ShowHurdelIfNeeded()
//    {
//        if (!hasActivatedObstacle && hurdel != null && gameObject.activeInHierarchy)
//        {
//            hurdel.SetActive(true);
//            hasActivatedObstacle = true;
//        }
//    }

//    // -------------------------------------
//    // Lane Width Calculation
//    // -------------------------------------
//    public float GetDynamicLaneWidth()
//    {
//        Renderer rend = GetComponentInChildren<Renderer>();
//        if (rend == null) return fallbackLaneWidth;

//        float totalWidth = rend.bounds.size.x;
//        return totalWidth / Mathf.Max(1, laneCount);
//    }

//    // -------------------------------------
//    // Lane Path Helpers
//    // -------------------------------------
//    public Transform GetLaneEntry(Lane lane)
//    {
//        return lane switch
//        {
//            Lane.Left => laneLeftEntry ?? transform,
//            Lane.Center => laneCenterEntry ?? transform,
//            Lane.Right => laneRightEntry ?? transform,
//            _ => transform
//        };
//    }

//    public Transform GetLaneMid(Lane lane)
//    {
//        return lane switch
//        {
//            Lane.Left => laneLeftMid,
//            Lane.Center => laneCenterMid,
//            Lane.Right => laneRightMid,
//            _ => null
//        };
//    }

//    // 🔹 NEW
//    public Transform GetLaneMid2(Lane lane)
//    {
//        return lane switch
//        {
//            Lane.Left => laneLeftMid2,
//            Lane.Center => laneCenterMid2,
//            Lane.Right => laneRightMid2,
//            _ => null
//        };
//    }

//    public Transform GetLaneExit(Lane lane)
//    {
//        return lane switch
//        {
//            Lane.Left => laneLeftExit ?? transform,
//            Lane.Center => laneCenterExit ?? transform,
//            Lane.Right => laneRightExit ?? transform,
//            _ => transform
//        };
//    }

//    /// <summary>
//    /// Get world-space position along a lane path (supports up to 2 midpoints)
//    /// </summary>
//    public Vector3 GetLanePosition(Lane lane, float progress)
//    {
//        Transform start = GetLaneEntry(lane);
//        Transform mid1 = GetLaneMid(lane);
//        Transform mid2 = GetLaneMid2(lane);
//        Transform end = GetLaneExit(lane);

//        if (start == null || end == null) return transform.position;

//        // 🔹 3 control points (quadratic Bezier) → Entry, Mid1, Exit
//        if (mid1 != null && mid2 == null)
//        {
//            return Mathf.Pow(1 - progress, 2) * start.position +
//                   2 * (1 - progress) * progress * mid1.position +
//                   Mathf.Pow(progress, 2) * end.position;
//        }

//        // 🔹 4 control points (cubic Bezier) → Entry, Mid1, Mid2, Exit
//        else if (mid1 != null && mid2 != null)
//        {
//            return Mathf.Pow(1 - progress, 3) * start.position +
//                   3 * Mathf.Pow(1 - progress, 2) * progress * mid1.position +
//                   3 * (1 - progress) * Mathf.Pow(progress, 2) * mid2.position +
//                   Mathf.Pow(progress, 3) * end.position;
//        }

//        // 🔹 No mids → straight line
//        else
//        {
//            return Vector3.Lerp(start.position, end.position, progress);
//        }
//    }

//    // -------------------------------------
//    // Tile Info
//    // -------------------------------------
//    public Vector3 GetOffset()
//    {
//        return (tileType == TileType.TIntersection && tIntersectionExits.Count > 0)
//            ? tIntersectionExits[0].position - entryPoint.position
//            : exitPoint.position - entryPoint.position;
//    }

//    public bool AllowsTurn()
//    {
//        return tileType == TileType.LeftTurn ||
//               tileType == TileType.RightTurn ||
//               tileType == TileType.TIntersection ||
//               tileType == TileType.DoubleTurn;
//    }

//    // -------------------------------------
//    // Gizmos
//    // -------------------------------------
//    private void OnDrawGizmos()
//    {
//        if (entryPoint != null)
//        {
//            Gizmos.color = Color.green;
//            Gizmos.DrawSphere(entryPoint.position, 0.2f);
//#if UNITY_EDITOR
//            UnityEditor.Handles.Label(entryPoint.position + Vector3.up * 0.3f, "Entry");
//#endif
//        }

//        if (exitPoint != null)
//        {
//            Gizmos.color = Color.red;
//            Gizmos.DrawSphere(exitPoint.position, 0.2f);
//#if UNITY_EDITOR
//            UnityEditor.Handles.Label(exitPoint.position + Vector3.up * 0.3f, "Exit");
//#endif
//        }

//        // 🔹 Draw lane paths with mid and mid2 points
//        Gizmos.color = Color.cyan;
//        foreach (Lane lane in System.Enum.GetValues(typeof(Lane)))
//        {
//            Transform entry = GetLaneEntry(lane);
//            Transform mid1 = GetLaneMid(lane);
//            Transform mid2 = GetLaneMid2(lane);
//            Transform exit = GetLaneExit(lane);

//            if (entry != null && exit != null)
//            {
//                Vector3 prev = entry.position;
//                for (int i = 1; i <= 20; i++)
//                {
//                    float t = i / 20f;
//                    Vector3 pos = GetLanePosition(lane, t);
//                    Gizmos.DrawLine(prev, pos);
//                    prev = pos;
//                }
//            }
//        }

//        // Lane width indicators
//        float laneW = GetDynamicLaneWidth();
//        Vector3 midPos = entryPoint ? entryPoint.position : transform.position;
//        Vector3 right = transform.right;
//        Gizmos.color = Color.yellow;
//        for (int i = -1; i <= 1; i++)
//        {
//            Vector3 pos = midPos + right * (i * laneW);
//            Gizmos.DrawSphere(pos, 0.15f);
//        }
//    }
//}



//using System.Collections.Generic;
//using UnityEngine;
//using Chakwa;

//public enum TileType
//{
//    Straight,
//    LeftTurn,
//    RightTurn,
//    DoubleTurn,
//    TIntersection
//}

//public enum Lane
//{
//    Left = -1,
//    Center = 0,
//    Right = 1
//}

//[System.Serializable]
//public class LaneSegment
//{
//    public Transform start;
//    public Transform end;
//}

//public class Tile : MonoBehaviour
//{
//    [Header("Tile Properties")]
//    public TileType tileType;

//    [Header("Entry/Exit Points (for tile chaining)")]
//    public Transform entryPoint;
//    public Transform exitPoint;

//    [Header("T-Intersection Exits (for branch points)")]
//    public List<Transform> tIntersectionExits = new List<Transform>(2);

//    [Header("Obstacle Settings")]
//    public GameObject hurdel;
//    private bool hasActivatedObstacle = false;

//    [Header("Respawn / Safety Points")]
//    public Transform safeBeforeTurn;
//    public Transform safeAfterTurn;
//    public Transform checkpointTransform;

//    [Header("Collectible Spots")]
//    public GameObject[] collectSpot;

//    [Header("Lane Width Settings")]
//    [Tooltip("Fallback if mesh bounds missing")]
//    public float fallbackLaneWidth = 3.5f;
//    public int laneCount = 3;

//    [Header("Connected Tiles (for segment switching)")]
//    public Tile nextForwardTile;
//    public Tile nextLeftTile;
//    public Tile nextRightTile;


//    private void Awake()
//    {
//        if (checkpointTransform == null)
//            checkpointTransform = transform;
//    }

//    private void Start()
//    {
//        if (hurdel != null)
//            hurdel.SetActive(false);
//        Invoke(nameof(ShowHurdelIfNeeded), 0.1f);
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        var pm = other.GetComponent<Chakwa.Player.PlayerMovement>();
//        if (pm != null && pm.IsDead()) return;

//        Transform cp = entryPoint != null ? entryPoint :
//                       (checkpointTransform != null ? checkpointTransform : transform);

//        if (RespawnManager.Instance != null)
//        {
//            RespawnManager.Instance.SetRespawnPoint(cp.position, cp.rotation);
//        }
//    }

//    private void ShowHurdelIfNeeded()
//    {
//        if (!hasActivatedObstacle && hurdel != null && gameObject.activeInHierarchy)
//        {
//            hurdel.SetActive(true);
//            hasActivatedObstacle = true;
//        }
//    }


//    public float GetDynamicLaneWidth()
//    {
//        Renderer rend = GetComponentInChildren<Renderer>();
//        if (rend == null)
//            return fallbackLaneWidth;

//        float totalWidth = rend.bounds.size.x;
//        return totalWidth / Mathf.Max(1, laneCount);
//    }

//    public bool AllowsTurn()
//    {
//        return tileType == TileType.LeftTurn ||
//               tileType == TileType.RightTurn ||
//               tileType == TileType.TIntersection ||
//               tileType == TileType.DoubleTurn;
//    }
//    public Tile GetConnectedTile(TurnDirection dir)
//    {
//        switch (dir)
//        {
//            case TurnDirection.Left:
//                return nextLeftTile;
//            case TurnDirection.Right:
//                return nextRightTile;
//            default:
//                return nextForwardTile;
//        }
//    }
//}
/*
using System.Collections.Generic;
using UnityEngine;
using Chakwa;

public enum TileType
{
    Straight,
    LeftTurn,
    RightTurn,
    DoubleTurn,
    TIntersection
}

public enum Lane
{
    Left = -1,
    Center = 0,
    Right = 1
}

[System.Serializable]
public class LaneSegment
{
    public Transform start;
    public Transform end;
}

public class Tile : MonoBehaviour
{
    [Header("Tile Properties")]
    public TileType tileType;

    [Header("Entry/Exit Points (for tile chaining)")]
    public Transform entryPoint;
    public Transform exitPoint;

    [Header("T-Intersection Exits (for branch points)")]
    public List<Transform> tIntersectionExits = new List<Transform>(2);

    [Header("Obstacle Settings")]
    public GameObject hurdel;
    private bool hasActivatedObstacle = false;

    [Header("Respawn / Safety Points")]
    public Transform safeBeforeTurn;
    public Transform safeAfterTurn;
    public Transform checkpointTransform;

    [Header("Collectible Spots")]
    public GameObject[] collectSpot;

    [Header("Lane Width Settings")]
    [Tooltip("Fallback if mesh bounds missing")]
    public float fallbackLaneWidth = 3.5f;
    public int laneCount = 3;

    [Header("Connected Tiles (for segment switching)")]
    public Tile nextForwardTile;
    public Tile nextLeftTile;
    public Tile nextRightTile;

    [Header("Per-Tile Lane World Positions")]
    [Tooltip("World-space lane center positions (set manually or auto-generated)")]
    public Vector3 leftLanePos;
    public Vector3 centerLanePos;
    public Vector3 rightLanePos;

    [Header("Lane Settings")]
    public float laneOffset = 3.0f; // distance between lanes
    private Dictionary<Lane, Vector3> lanePositions = new Dictionary<Lane, Vector3>();

    [Tooltip("If true, automatically generate lane positions. If false, use manually set world-space positions.")]
    public bool autoGenerateLanes = false;

    private void Awake()
    {
        if (checkpointTransform == null)
            checkpointTransform = transform;
    }

    private void Start()
    {
        if (hurdel != null)
            hurdel.SetActive(false);

        Invoke(nameof(ShowHurdelIfNeeded), 0.1f);
        InitializeLanePositions();

    }
    
    private void OnTriggerEnter(Collider other)
    {
        var pm = other.GetComponent<Chakwa.Player.PlayerMovement>();
        if (pm != null && pm.IsDead()) return;

        Transform cp = entryPoint != null ? entryPoint :
                       (checkpointTransform != null ? checkpointTransform : transform);

        if (RespawnManager.Instance != null)
        {
            RespawnManager.Instance.SetRespawnPoint(cp.position, cp.rotation);
        }
    }

    private void ShowHurdelIfNeeded()
    {
        if (!hasActivatedObstacle && hurdel != null && gameObject.activeInHierarchy)
        {
            hurdel.SetActive(true);
            hasActivatedObstacle = true;
        }
    }

    // ------------------ LANE POSITION HANDLING ------------------

    /// <summary>
    /// Initializes per-tile lane world positions along Z-axis.
    /// </summary>
    private void InitializeLanePositions()
    {
        lanePositions.Clear();

        if (leftLanePos != Vector3.zero && rightLanePos != Vector3.zero)
        {
            // Use manually set world positions (you said you set them in Inspector)
            lanePositions[Lane.Left] = leftLanePos;
            lanePositions[Lane.Center] = centerLanePos;
            lanePositions[Lane.Right] = rightLanePos;

            Debug.Log($"[Tile] {name} using MANUAL lane positions:\n🟥 Left={leftLanePos}\n🟨 Center={centerLanePos}\n🟩 Right={rightLanePos}");
        }
        else
        {
            // Auto fallback (for generated tiles)
            Vector3 basePos = transform.position;
            lanePositions[Lane.Center] = basePos;
            lanePositions[Lane.Left] = basePos - transform.right * laneOffset;
            lanePositions[Lane.Right] = basePos + transform.right * laneOffset;

            Debug.Log($"[Tile] {name} auto-generated lane positions.");
        }
    }


    /// <summary>
    /// Returns the world position of the specified lane on this tile.
    /// </summary>
    public Vector3 GetLaneWorldPosition(Lane lane)
    {
        if (lanePositions.TryGetValue(lane, out Vector3 pos))
            return pos;

        return transform.position;
    }


    public float GetDynamicLaneWidth()
    {
        Renderer rend = GetComponentInChildren<Renderer>();
        if (rend == null)
            return fallbackLaneWidth;

        float totalWidth = rend.bounds.size.x;
        return totalWidth / Mathf.Max(1, laneCount);
    }

    public bool AllowsTurn()
    {
        return tileType == TileType.LeftTurn ||
               tileType == TileType.RightTurn ||
               tileType == TileType.TIntersection ||
               tileType == TileType.DoubleTurn;
    }

    public Tile GetConnectedTile(TurnDirection dir)
    {
        switch (dir)
        {
            case TurnDirection.Left:
                return nextLeftTile;
            case TurnDirection.Right:
                return nextRightTile;
            default:
                return nextForwardTile;
        }
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetLaneWorldPosition(Lane.Left), 0.15f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(GetLaneWorldPosition(Lane.Center), 0.15f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(GetLaneWorldPosition(Lane.Right), 0.15f);
    }
    #endif
}
*/

using System.Collections.Generic;
using UnityEngine;
using Chakwa;

public enum TileType
{
    Straight,
    LeftTurn,
    RightTurn,
    DoubleTurn,
    TIntersection
}

public enum Lane
{
    Left = -1,
    Center = 0,
    Right = 1
}

[System.Serializable]
public class LaneSegment
{
    public Transform start;
    public Transform end;
}

public class Tile : MonoBehaviour
{
    [Header("Tile Properties")]
    public TileType tileType;

    [Header("Entry/Exit Points (for tile chaining)")]
    public Transform entryPoint;
    public Transform exitPoint;

    [Header("T-Intersection Exits (for branch points)")]
    public List<Transform> tIntersectionExits = new List<Transform>(2);

    [Header("Obstacle Settings")]
    public GameObject hurdel;
    private bool hasActivatedObstacle = false;

    [Header("Respawn / Safety Points")]
    public Transform safeBeforeTurn;
    public Transform safeAfterTurn;
    public Transform checkpointTransform;

    [Header("Collectible Spots")]
    public GameObject[] collectSpot;

    [Header("Lane Settings")]
    [Tooltip("Distance between lanes")]
    public float laneOffset = 3.0f;

    [Tooltip("If true, automatically generate lane positions. If false, use manual world-space positions.")]
    public bool autoGenerateLanes = true;

    [Header("Manual Lane World Positions (optional)")]
    public Vector3 leftLanePos;
    public Vector3 centerLanePos;
    public Vector3 rightLanePos;

   

    [Header("Lane Width Settings")]
    [Tooltip("Fallback if mesh bounds missing")]
    public float fallbackLaneWidth = 3.5f;
    public int laneCount = 3;

    private Dictionary<Lane, Vector3> lanePositions = new Dictionary<Lane, Vector3>();

    private void Awake()
    {
        if (checkpointTransform == null)
            checkpointTransform = transform;
    }

    private void Start()
    {
        if (hurdel != null)
            hurdel.SetActive(false);

        Invoke(nameof(ShowHurdelIfNeeded), 0.1f);
        InitializeLanePositions();
    }

    private void OnTriggerEnter(Collider other)
    {
        var pm = other.GetComponent<Chakwa.Player.PlayerMovement>();
        if (pm != null && pm.IsDead()) return;

        Transform cp = entryPoint != null ? entryPoint :
                       (checkpointTransform != null ? checkpointTransform : transform);

        if (RespawnManager.Instance != null)
        {
            RespawnManager.Instance.SetRespawnPoint(cp.position, cp.rotation);
        }
    }

    private void ShowHurdelIfNeeded()
    {
        if (!hasActivatedObstacle && hurdel != null && gameObject.activeInHierarchy)
        {
            hurdel.SetActive(true);
            hasActivatedObstacle = true;
        }
    }

    // -------------------------------------------------------------------------
    // LANE POSITION HANDLING
    // -------------------------------------------------------------------------

    /// <summary>
    /// Initializes per-tile lane world positions that align with tile rotation and placement.
    /// </summary>
    private void InitializeLanePositions()
    {
        lanePositions.Clear();

        // Determine the forward axis using entry/exit if possible
        Vector3 forwardDir = (exitPoint != null && entryPoint != null)
            ? (exitPoint.position - entryPoint.position).normalized
            : transform.forward;

        // Right direction is perpendicular to forward on the horizontal plane
        Vector3 rightDir = Vector3.Cross(Vector3.up, forwardDir).normalized;

        // Base lane position (center)
        Vector3 basePos = (entryPoint != null && exitPoint != null)
            ? Vector3.Lerp(entryPoint.position, exitPoint.position, 0.5f)
            : transform.position;

        if (!autoGenerateLanes && leftLanePos != Vector3.zero && rightLanePos != Vector3.zero)
        {
            // ✅ Use manually defined lane positions
            lanePositions[Lane.Left] = leftLanePos;
            lanePositions[Lane.Center] = centerLanePos;
            lanePositions[Lane.Right] = rightLanePos;

            Debug.Log($"[Tile] {name} using MANUAL lane positions:\n🟥 Left={leftLanePos}\n🟨 Center={centerLanePos}\n🟩 Right={rightLanePos}");
        }
        else
        {
            // ✅ Auto-generate based on rotation and offset
            lanePositions[Lane.Center] = basePos;
            lanePositions[Lane.Left] = basePos - rightDir * laneOffset;
            lanePositions[Lane.Right] = basePos + rightDir * laneOffset;

            Debug.Log($"[Tile] {name} auto-generated lane positions aligned with rotation.");
        }
    }

    /// <summary>
    /// Returns the world position of the specified lane for this tile.
    /// </summary>
    public Vector3 GetLaneWorldPosition(Lane lane)
    {
        if (lanePositions.TryGetValue(lane, out Vector3 pos))
            return pos;

        // Fallback if not initialized
        Vector3 forwardDir = (exitPoint != null && entryPoint != null)
            ? (exitPoint.position - entryPoint.position).normalized
            : transform.forward;
        Vector3 rightDir = Vector3.Cross(Vector3.up, forwardDir).normalized;
        Vector3 basePos = transform.position;

        switch (lane)
        {
            case Lane.Left:
                return basePos - rightDir * laneOffset;
            case Lane.Right:
                return basePos + rightDir * laneOffset;
            default:
                return basePos;
        }
    }

    public float GetDynamicLaneWidth()
    {
        Renderer rend = GetComponentInChildren<Renderer>();
        if (rend == null)
            return fallbackLaneWidth;

        float totalWidth = rend.bounds.size.x;
        return totalWidth / Mathf.Max(1, laneCount);
    }

    public bool AllowsTurn()
    {
        return tileType == TileType.LeftTurn ||
               tileType == TileType.RightTurn ||
               tileType == TileType.TIntersection ||
               tileType == TileType.DoubleTurn;
    }

   
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetLaneWorldPosition(Lane.Left), 0.15f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(GetLaneWorldPosition(Lane.Center), 0.15f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(GetLaneWorldPosition(Lane.Right), 0.15f);

        // Draw forward direction for debugging
        if (exitPoint != null && entryPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(entryPoint.position, exitPoint.position);
        }
    }
#endif
}

