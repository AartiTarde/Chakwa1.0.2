using UnityEngine;

public class LaneMover : MonoBehaviour
{
    [Header("Lane Settings")]
    public float laneDistance = 2f;   // distance between each lane
    public int totalLanes = 3;        // number of lanes (3 = left, center, right)
    public float moveSpeed = 20f;     // how fast to move

    private int currentLaneIndex = 1; // start in the center lane (0=left, 1=center, 2=right)
    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        // Set starting position once
        UpdateTargetPosition();
        transform.position = targetPosition;
    }

    void Update()
    {
        HandleInput();

        if (isMoving)
        {
            MoveToTarget();
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }
    }

    void MoveLeft()
    {
        if (currentLaneIndex > 0)
        {
            currentLaneIndex--;
            UpdateTargetPosition();
            isMoving = true;
        }
        else
        {
            Debug.Log("Already at leftmost tile!");
        }
    }

    void MoveRight()
    {
        if (currentLaneIndex < totalLanes - 1)
        {
            currentLaneIndex++;
            UpdateTargetPosition();
            isMoving = true;
        }
        else
        {
            Debug.Log("Already at rightmost tile!");
        }
    }

    void UpdateTargetPosition()
    {
        // Calculate the middle lane index
        float middleIndex = (totalLanes - 1) / 2f;
        float targetX = (currentLaneIndex - middleIndex) * laneDistance;
        targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
    }

    void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Stop when close enough
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition;
            isMoving = false;
        }
    }
}
