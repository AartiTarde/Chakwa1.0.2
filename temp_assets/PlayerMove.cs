using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private TouchControls controls;
    private CharacterController controller;

    public float jumpForce = 5f;
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    private Vector3 velocity;
    private bool isJumpPressed;

    void Awake()
    {
        controls = new TouchControls();

        controls.Touch.Jump.started += ctx =>
        {
            Debug.Log("Jump pressed via touch");
            isJumpPressed = true;
        };
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Move horizontally based on phone tilt
        Vector3 tilt = Input.acceleration;
        Vector3 move = new Vector3(tilt.x * moveSpeed, 0, 0);
        controller.Move(move * Time.deltaTime);

        // Ground check: reset vertical velocity when grounded
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Jump if pressed and grounded
        if (isJumpPressed && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            Debug.Log("Jump executed");
            isJumpPressed = false;
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Move vertically
        controller.Move(velocity * Time.deltaTime);
    }
}
