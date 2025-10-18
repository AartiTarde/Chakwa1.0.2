using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    private Animator animator;

    // Store parameter hashes (faster than strings)
    private int hashTurnLeft;
    private int hashTurnRight;
    private int hashJump;
    private int hashIsRunning;
    private int fall;

    void Awake()
    {
        animator = GetComponent<Animator>();

        // Cache all parameter IDs
        hashTurnLeft = Animator.StringToHash("leftturn");
        hashTurnRight = Animator.StringToHash("rightturn");
        hashJump = Animator.StringToHash("newjump");
        hashIsRunning = Animator.StringToHash("Medium Run");

        fall = Animator.StringToHash("Fall");

    }

    public void PlayTurnLeft() => animator.SetTrigger(hashTurnLeft);
    public void PlayTurnRight() => animator.SetTrigger(hashTurnRight);
    public void PlayJump() => animator.SetBool(hashJump, true);
    public void EndJump() => animator.SetBool(hashJump, false);
    public void SetRunning(bool running) => animator.SetBool(hashIsRunning, running);
    public void Fall() => animator.SetBool(fall, true);
}
