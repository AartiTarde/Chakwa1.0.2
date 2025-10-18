/*using UnityEngine;
using UnityEngine.InputSystem;

public class LaneSwitcher : MonoBehaviour
{
    [Header("Forward Movement")]
    public float forwardSpeed = 10f;   // Player moves forward along X-axis

    [Header("Lane Switching Settings")]
    public float laneWidth = 2f;       // Distance between lanes along Z-axis
    public float laneSwitchSpeed = 10f;// Smooth horizontal speed
    private int currentLane = 1;       // 0 = Left, 1 = Center, 2 = Right

    private float targetZ;

    // Swipe detection
    private Vector2 startTouch;
    private float minSwipeDistance = 50f;

    private void Start()
    {
        // Set initial Z position based on center lane
        targetZ = transform.position.z;
    }

    private void Update()
    {
        HandleForwardMovement();
        HandleKeyboard();
        HandleSwipe();
        MoveHorizontally();
    }

    // ----------------------
    // Forward movement along X-axis
    // ----------------------
    private void HandleForwardMovement()
    {
        transform.position += Vector3.right * forwardSpeed * Time.deltaTime;
    }

    // ----------------------
    // Keyboard input
    // ----------------------
    private void HandleKeyboard()
    {
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame) SwitchLeft();
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame) SwitchRight();
    }

    // ----------------------
    // Swipe input
    // ----------------------
    private void HandleSwipe()
    {
        if (Touchscreen.current == null) return;

        var touch = Touchscreen.current.primaryTouch;
        if (!touch.press.isPressed) return;

        if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            startTouch = touch.position.ReadValue();
        else if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            Vector2 swipeDelta = touch.position.ReadValue() - startTouch;

            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y) && Mathf.Abs(swipeDelta.x) > minSwipeDistance)
            {
                if (swipeDelta.x > 0) SwitchRight();
                else SwitchLeft();
            }
        }
    }

    private void SwitchLeft()
    {
        currentLane = Mathf.Max(0, currentLane - 1);
        targetZ = (currentLane - 1) * laneWidth; // Left = -laneWidth, Center = 0, Right = laneWidth
        Debug.Log("Switched Left → lane " + currentLane + ", target Z = " + targetZ);
    }

    private void SwitchRight()
    {
        currentLane = Mathf.Min(2, currentLane + 1);
        targetZ = (currentLane - 1) * laneWidth; // Left = -laneWidth, Center = 0, Right = laneWidth
        Debug.Log("Switched Right → lane " + currentLane + ", target Z = " + targetZ);
    }

   
    private void MoveHorizontally()
    {
        Vector3 pos = transform.position;
        pos.z = Mathf.Lerp(pos.z, targetZ, laneSwitchSpeed * Time.deltaTime);
        transform.position = pos;
    }
}
*/