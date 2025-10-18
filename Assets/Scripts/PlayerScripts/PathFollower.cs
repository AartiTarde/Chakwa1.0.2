using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public Transform[] pathPoints; 
    public float movementSpeed = 5f; 
    public float offPathDistance = 1.5f; 
    private int currentPointIndex = 0;

    void Update()
    {
        FollowPath();
        CheckIfOffPath();
    }

    void FollowPath()
    {
     
        if (currentPointIndex >= pathPoints.Length)
            return;

        
        Transform targetPoint = pathPoints[currentPointIndex];
        Vector3 targetPosition = targetPoint.position;

       
        float step = movementSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

    
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentPointIndex++;
        }
    }

    void CheckIfOffPath()
    {
     
        if (currentPointIndex <= 0) return; 

        float distanceFromLastPoint = Vector3.Distance(transform.position, pathPoints[currentPointIndex - 1].position);

        if (distanceFromLastPoint > offPathDistance)
        {
           
            transform.position = pathPoints[currentPointIndex - 1].position;
            Debug.Log("Player off path, forced to the nearest point.");
        }
    }
    private void OnDrawGizmos()
    {
        if (pathPoints == null || pathPoints.Length == 0)
            return;

        Gizmos.color = Color.green;

        for (int i = 0; i < pathPoints.Length - 1; i++)
        {
            Gizmos.DrawLine(pathPoints[i].position, pathPoints[i + 1].position);
            Gizmos.DrawSphere(pathPoints[i].position, 0.1f); // Draw spheres at each path point
        }

      
        Gizmos.DrawSphere(pathPoints[pathPoints.Length - 1].position, 0.1f);
    }

}
