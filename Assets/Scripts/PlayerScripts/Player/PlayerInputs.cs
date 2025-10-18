using UnityEngine;
using UnityEngine.InputSystem;
using InputTouchPhase = UnityEngine.InputSystem.TouchPhase;
using Chakwa.Player;
public class PlayerInputs : MonoBehaviour
{
    private PlayerControls controls;

    [Header("Swipe Settings")]
    private Vector2 startTouchPosition;
    private Vector2 swipeDelta;
    private float minSwipeDistance = 50f;

    private bool waitingForTurnInput = false;
    private bool canTurn = false;
    private bool hasTurned = false;

    public event System.Action OnTurnLeft;
    public event System.Action OnTurnRight;
    public event System.Action OnJump;
    private PlayerJump playerJump;


    void Awake()
    {
        controls = new PlayerControls();

        controls.Player.TurnLeft.performed += ctx =>
        {
            if (waitingForTurnInput) TriggerLeftTurn();
            else if (canTurn && !hasTurned) TriggerLeftTurn();
        };

        controls.Player.TurnRight.performed += ctx =>
        {
            if (waitingForTurnInput) TriggerRightTurn();
            else if (canTurn && !hasTurned) TriggerRightTurn();
        };

        controls.Player.Swipe.performed += ctx => swipeDelta = ctx.ReadValue<Vector2>();
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();
    void Update()
    {
        HandleSwipeInput();
        if (waitingForTurnInput)
        {
            if (Keyboard.current.aKey.wasPressedThisFrame) TriggerLeftTurn();
            else if (Keyboard.current.dKey.wasPressedThisFrame) TriggerRightTurn();
        }
    }
    private void TriggerLeftTurn()
    {
        if (hasTurned) return;
        OnTurnLeft?.Invoke();
        hasTurned = true;
        waitingForTurnInput = false;
        canTurn = false;
    }

    private void TriggerRightTurn()
    {
        if (hasTurned) return;
        OnTurnRight?.Invoke();
        hasTurned = true;
        waitingForTurnInput = false;
        canTurn = false;
    }
    private void HandleSwipeInput()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            var touch = Touchscreen.current.primaryTouch;
            if (touch.phase.ReadValue() == InputTouchPhase.Began)
            {
                startTouchPosition = touch.position.ReadValue();
            }
            else if (touch.phase.ReadValue() == InputTouchPhase.Ended)
            {
                swipeDelta = touch.position.ReadValue() - startTouchPosition;
            }
        }

        if (swipeDelta == Vector2.zero) return;

        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            if (swipeDelta.x > minSwipeDistance) TriggerRightTurn();
            else if (swipeDelta.x < -minSwipeDistance) TriggerLeftTurn();
        }
        else if (swipeDelta.y > minSwipeDistance)
        {
            OnJump?.Invoke();
        }

        swipeDelta = Vector2.zero;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TurnPoint"))
        {
            canTurn = true;
            hasTurned = false;
        }
        if (other.CompareTag("TIntersectionTrigger"))
        {
            waitingForTurnInput = true;
            hasTurned = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TurnPoint") || other.CompareTag("TIntersectionTrigger"))
        {
            canTurn = false;
            waitingForTurnInput = false;
            hasTurned = false;
        }
    }
    public void EnableTurnInput()
    {
        waitingForTurnInput = true;
        hasTurned = false;
        canTurn = true;
        Debug.Log("PlayerInputs is now waiting for turn input.");
    }

}
