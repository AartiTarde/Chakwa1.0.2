/*using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeTurnHandler : MonoBehaviour
{
    private PlayerControls inputActions;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    [SerializeField] private float swipeThreshold = 50f;

    public System.Action OnSwipeLeft;
    public System.Action OnSwipeRight;

    private void Awake()
    {
        inputActions = new PlayerControls();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Touch.started += ctx => StartTouch(ctx);
        inputActions.Player.Touch.canceled += ctx => EndTouch(ctx);
    }

    private void OnDisable()
    {
        inputActions.Player.Touch.started -= StartTouch;
        inputActions.Player.Touch.canceled -= EndTouch;
        inputActions.Disable();
    }

    private void StartTouch(InputAction.CallbackContext ctx)
    {
        startTouchPosition = inputActions.Player.Touch.ReadValue<Vector2>();
    }

    private void EndTouch(InputAction.CallbackContext ctx)
    {
        endTouchPosition = inputActions.Player.Touch.ReadValue<Vector2>();
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        Vector2 delta = endTouchPosition - startTouchPosition;

        if (Mathf.Abs(delta.x) > swipeThreshold && Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x > 0)
            {
                Debug.Log("Swipe Right Detected");
                OnSwipeRight?.Invoke();
            }
            else
            {
                Debug.Log("Swipe Left Detected");
                OnSwipeLeft?.Invoke();
            }
        }
    }
}
*/