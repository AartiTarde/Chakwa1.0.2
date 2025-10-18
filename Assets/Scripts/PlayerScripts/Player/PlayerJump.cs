using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerJump : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpForce = 9f;
    public float gravity = 20f;
    public float fallMultiplier = 4f;
    public float lowJumpMultiplier = 2f;

    [HideInInspector] public bool IsJumping = false;

    private PlayerAnimationHandler animatorHandler;
    private CharacterController controller;

    [HideInInspector] public float verticalVelocity = 0f;

    public Animator animator;
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animatorHandler = GetComponent<PlayerAnimationHandler>();
    }

    public void Jump()
    {
        if (controller.isGrounded && !IsJumping) 
        {
            verticalVelocity = jumpForce;
            IsJumping = true;

            if (animatorHandler != null)
            { animator.Play("JUMP06"); }
               
        }
    }

    public void HandleGravity()
    {
        if (controller.isGrounded && verticalVelocity <= 0)
        {
            verticalVelocity = -2f;

            if (IsJumping) 
            {
                IsJumping = false;
                if (animatorHandler != null)
                    animatorHandler.EndJump();
            }
        }
        else
        {
           
            verticalVelocity -= gravity * Time.deltaTime;

            if (verticalVelocity < 0)
                verticalVelocity -= gravity * (fallMultiplier - 1) * Time.deltaTime;

           
            else if (verticalVelocity > 0 && !IsJumping)
                verticalVelocity -= gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    public float GetVerticalVelocity() => verticalVelocity;
}
