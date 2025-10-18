/*using UnityEngine;

public enum TileType
{
    Straight,
    LeftTurn,
    RightTurn,
    DoubleTurn  
}

public class Tile : MonoBehaviour
{
    [Header("Tile Points")]
    public Transform entryPoint;
    public Transform exitPoint;

    [Header("Tile Properties")]
    public TileType tileType;

    /// <summary>
    /// Gets the offset from entry to exit point (not used for positioning when using entry/exit snap method).
    /// </summary>
    public Vector3 GetOffset()
    {
        return exitPoint.position - entryPoint.position;
    }

    private void OnDrawGizmos()
    {
        if (entryPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(entryPoint.position, 0.2f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(entryPoint.position, entryPoint.position + entryPoint.forward * 2f);

    #if UNITY_EDITOR
                UnityEditor.Handles.Label(entryPoint.position + Vector3.up * 0.3f, "Entry");
    #endif
            }

            if (exitPoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(exitPoint.position, 0.2f);
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(exitPoint.position, exitPoint.position + exitPoint.forward * 2f);

    #if UNITY_EDITOR
                UnityEditor.Handles.Label(exitPoint.position + Vector3.up * 0.3f, "Exit");
    #endif
        }
    }
}
*//*
using UnityEngine;
using System.Collections.Generic;

public enum TileType
{
    Straight,
    LeftTurn,
    RightTurn,
     DoubleTurn,
    TIntersection
}
public class Tile : MonoBehaviour
{
    //public TileType tileType;
    //public Transform  pivot;


   
    [Header("Tile Points")]
    public Transform entryPoint;
    public Transform exitPoint;

     [Header("Tile Properties")]
     public TileType tileType;

     [Header("Optional Path Segments (for DoubleTurn Visuals)")]
     public List<Transform> pathSegments = new List<Transform>(); 

    
     public Vector3 GetOffset()
     {
         return exitPoint.position - entryPoint.position;
     }

     private void OnDrawGizmos()
     {
         if (entryPoint != null)
         {
             Gizmos.color = Color.green;
             Gizmos.DrawSphere(entryPoint.position, 0.2f);
             Gizmos.color = Color.yellow;
             Gizmos.DrawLine(entryPoint.position, entryPoint.position + entryPoint.forward * 2f);
             Gizmos.DrawRay(entryPoint.position, entryPoint.forward * 2);

 #if UNITY_EDITOR
             UnityEditor.Handles.Label(entryPoint.position + Vector3.up * 0.3f, "Entry");
 #endif
         }

         if (exitPoint != null)
         {
             Gizmos.color = Color.red;
             Gizmos.DrawSphere(exitPoint.position, 0.2f);
             Gizmos.color = Color.magenta;
             Gizmos.DrawLine(exitPoint.position, exitPoint.position + exitPoint.forward * 2f);
             Gizmos.DrawRay(exitPoint.position, exitPoint.forward * 2);

 #if UNITY_EDITOR
             UnityEditor.Handles.Label(exitPoint.position + Vector3.up * 0.3f, "Exit");
 #endif
         }

         // Draw internal segments if it's a double turn
         if (tileType == TileType.DoubleTurn && pathSegments != null && pathSegments.Count > 1)
         {
             Gizmos.color = Color.cyan;
             for (int i = 0; i < pathSegments.Count - 1; i++)
             {
                 if (pathSegments[i] != null && pathSegments[i + 1] != null)
                 {
                     Gizmos.DrawLine(pathSegments[i].position, pathSegments[i + 1].position);
                 }
             }

     #if UNITY_EDITOR
                 for (int i = 0; i < pathSegments.Count; i++)
                 {
                     if (pathSegments[i] != null)
                         UnityEditor.Handles.Label(pathSegments[i].position + Vector3.up * 0.2f, $"Path {i + 1}");
                 }
     #endif
             }
     }
}
*/
/*
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Straight,
    LeftTurn,
    RightTurn,
    DoubleTurn,
    TIntersection
}
public class Tile : MonoBehaviour
{
    [Header("Tile Points")]
    public Transform entryPoint;

    // Single exit for all except TIntersection
    public Transform exitPoint;

    // Optional: Two exit points specifically for TIntersection
    [Header("TIntersection Properties")]
    public List<Transform> tIntersectionExits = new List<Transform>(2);

    [Header("Tile Properties")]
    public TileType tileType;

    [Header("Optional Path Segments (for DoubleTurn Visuals)")]
    public List<Transform> pathSegments = new List<Transform>();

    //  [Header("Obstracles")]
    //public GameObject[] obstracles;
    //public List<Transform> obstacleSpawnPoints;
    [Header("Obstracles")]
    bool isActive;
    public GameObject hurdel;
    void Start()
    {
        hurdel.SetActive(false);
        if (this.gameObject.activeInHierarchy)
        {
            Debug.Log("Tile is active in the hierarchy.");
            isActive = true;
        }
        else
        {
            Debug.Log("Tile is NOT active in the hierarchy.");
        }
        showHurdel();
    }
    public Vector3 GetOffset()
    {
        return (tileType == TileType.TIntersection && tIntersectionExits.Count > 0)
            ? tIntersectionExits[0].position - entryPoint.position
            : exitPoint.position - entryPoint.position;
    }
    public void showHurdel()
    {
        if(isActive)
        {
            hurdel.SetActive(true);
            isActive = false;
            print("it's run");
        }
        else
        {
            print("it's not run");
        }
    }
    private void OnDrawGizmos()
    {
        if (entryPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(entryPoint.position, 0.2f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(entryPoint.position, entryPoint.position + entryPoint.forward * 2f);
            Gizmos.DrawRay(entryPoint.position, entryPoint.forward * 2);

#if UNITY_EDITOR
            UnityEditor.Handles.Label(entryPoint.position + Vector3.up * 0.3f, "Entry");
#endif
        }

        if (tileType == TileType.TIntersection && tIntersectionExits != null)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < tIntersectionExits.Count && i < 2; i++) // limit to 2 exits
            {
                if (tIntersectionExits[i] != null)
                {
                    Gizmos.DrawSphere(tIntersectionExits[i].position, 0.2f);
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawLine(tIntersectionExits[i].position, tIntersectionExits[i].position + tIntersectionExits[i].forward * 2f);
                    Gizmos.DrawRay(tIntersectionExits[i].position, tIntersectionExits[i].forward * 2);

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
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(exitPoint.position, exitPoint.position + exitPoint.forward * 2f);
            Gizmos.DrawRay(exitPoint.position, exitPoint.forward * 2);

#if UNITY_EDITOR
            UnityEditor.Handles.Label(exitPoint.position + Vector3.up * 0.3f, "Exit");
#endif
        }

        if (tileType == TileType.DoubleTurn && pathSegments != null && pathSegments.Count > 1)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < pathSegments.Count - 1; i++)
            {
                if (pathSegments[i] != null && pathSegments[i + 1] != null)
                {
                    Gizmos.DrawLine(pathSegments[i].position, pathSegments[i + 1].position);
                }
            }

    #if UNITY_EDITOR
                for (int i = 0; i < pathSegments.Count; i++)
                {
                    if (pathSegments[i] != null)
                        UnityEditor.Handles.Label(pathSegments[i].position + Vector3.up * 0.2f, $"Path {i + 1}");
                }
    #endif
        }
    }
}
*/


using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Straight,
    LeftTurn,
    RightTurn,
   DoubleTurn,
    TIntersection
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

    void Start()
    {
        if (hurdel != null)
            hurdel.SetActive(false);

        // Delay activation to next frame so tile is fully instantiated
        Invoke(nameof(ShowHurdelIfNeeded), 0.1f);
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

        if (tileType == TileType.DoubleTurn && pathSegments != null && pathSegments.Count > 1)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < pathSegments.Count - 1; i++)
            {
                if (pathSegments[i] != null && pathSegments[i + 1] != null)
                {   
                    Gizmos.DrawLine(pathSegments[i].position, pathSegments[i + 1].position);
                }
            }

#if UNITY_EDITOR
            for (int i = 0; i < pathSegments.Count; i++)
            {
                if (pathSegments[i] != null)
                    UnityEditor.Handles.Label(pathSegments[i].position + Vector3.up * 0.2f, $"Path {i + 1}");
            }
#endif
        }
    }
}
