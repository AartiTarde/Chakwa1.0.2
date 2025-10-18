using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float followDistance = 20f;
    public float followHeight = 50f;
    public float followSpeed = 5f;
    public float lookAtHeight = 1f;

    public float initialHeight = 100f;         // Starting height for top-down view
    public float transitionDuration = 2f;      // Time to move from top-down to follow view

    private Vector3 targetPosition;
    private float transitionTimer = 0f;
    private bool isTransitioning = true;

    void Start()
    {
        if (player == null) return;

        // Start directly above the player
        Vector3 startPos = player.position;
        startPos.y += initialHeight;
        transform.position = startPos;

        // Look directly at player during top-down
        transform.LookAt(player.position + Vector3.up * lookAtHeight);
    }

    void LateUpdate()
    {
        if (player == null) return;

        if (isTransitioning)
        {
            // Calculate target follow position
            targetPosition = player.position - player.forward * followDistance;
            targetPosition.y = player.position.y + followHeight;

            // Smooth transition from top-down to follow view
            transitionTimer += Time.deltaTime;
            float t = Mathf.Clamp01(transitionTimer / transitionDuration);
            transform.position = Vector3.Lerp(transform.position, targetPosition, t);

            // Look at the player
            Vector3 lookTarget = player.position + Vector3.up * lookAtHeight;
            transform.LookAt(lookTarget);

            if (t >= 1f)
            {
                isTransitioning = false; // Done transitioning
            }
        }
        else
        {
            // Follow player smoothly
            targetPosition = player.position - player.forward * followDistance;
            targetPosition.y = player.position.y + followHeight;

            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);

            Vector3 lookTarget = player.position + Vector3.up * lookAtHeight;
            transform.LookAt(lookTarget);
        }
    }
}