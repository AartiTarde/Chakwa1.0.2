using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class DemoJumpWithAnimation : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpForce = 12f;      // initial jump velocity
    public float gravity = 30f;        // gravity strength
    public float fallMultiplier = 4f;  // makes falling faster
    public float lowJumpMultiplier = 2f;

    [Header("Animation")]
    public Animator animator;          // assign your player Animator here
    //public string jumpAnimationName = "HumanM@Jump01 [RM]"; // name of the jump animation
    public string jumpAnimationName = "newjump";
    private CharacterController controller;
    private float verticalVelocity = 0f;
    private bool isJumping = false;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        HandleJumpInput();
        ApplyGravity();
        MovePlayer();
    }

    private void HandleJumpInput()
    {
        // Jump on Space
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            verticalVelocity = jumpForce;
            isJumping = true;

            // Play jump animation directly from start
            animator?.Play(jumpAnimationName, -1, 0f);
        }
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && verticalVelocity <= 0)
        {
            verticalVelocity = -2f; // keep grounded
            isJumping = false;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;

            if (verticalVelocity < 0)
                verticalVelocity -= gravity * (fallMultiplier - 1) * Time.deltaTime;
            else if (verticalVelocity > 0 && !isJumping)
                verticalVelocity -= gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void MovePlayer()
    {
        Vector3 move = Vector3.up * verticalVelocity * Time.deltaTime;
        controller.Move(move);
    }
}
