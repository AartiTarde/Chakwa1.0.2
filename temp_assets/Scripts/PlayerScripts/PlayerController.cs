using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    public Animator _animator;

    /***player movements***/
    public float normalSpeed = 8f;
    public float boostSpeed = 15f;
    public float jumpForce = 8f;
    public float gravity = 20f;

    private float currentSpeed;
    private float verticalVelocity;
    private Vector3 moveDirection;

    private CharacterController controller;
    private Animator animator;

    private bool isBoosting = false;
    private Camera mainCam;

    public static PlayerController instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
      
    }
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentSpeed = normalSpeed;
        mainCam = Camera.main;
    }
    void Update()
    {
        HandleMovement();
        HandleAnimation();
        HandleTouchInput();
    }
    void HandleMovement()
    {
        
        moveDirection = transform.forward * currentSpeed;
        if (controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
                animator.Play("Running Jump");
            }
        }

        verticalVelocity -= gravity * Time.deltaTime;
        moveDirection.y = verticalVelocity;

        controller.Move(moveDirection * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isBoosting = true;
            currentSpeed = boostSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isBoosting = false;
            currentSpeed = normalSpeed;
        }
    }
    void HandleAnimation()
    {
        
        float animationSpeed = isBoosting ? 1.5f : 1.0f;
        animator.SetFloat("Medium Run", animationSpeed);
    }
    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

          
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = mainCam.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform == transform && controller.isGrounded)
                    {
                        verticalVelocity = jumpForce;
                        animator.SetTrigger("Running Jump");
                    }
                }
            }
        }
    }
}