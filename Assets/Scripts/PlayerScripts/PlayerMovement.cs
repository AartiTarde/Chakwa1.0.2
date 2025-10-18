
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Chakwa;
using InputTouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Chakwa.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private CharacterController controller;
        private Animator animator;
        private PlayerControls controls;

        public static PlayerMovement Instance;

        [Header("Movement")]
        public float moveSpeed = 5f;

        private float verticalVelocity;
        private Vector3 moveDirection = Vector3.forward;
        private bool isJumpPressed = false;

        [Header("UI")]
        private TurnDirection allowedTurn = TurnDirection.Both;
        public EnemyChase demon;
        private bool canTrigger = true;
        private Vector3 lastSafePosition;
        private float safePositionSaveInterval = 1f;
        private float safePositionTimer;

        private bool isDead = false;
        private bool isInvincible = false;

        [Header("PowerUpsData")]
        public PowerUpsData powerUps;

        [Header("Speed Boost Power-Up")]
        public GameObject speedBoostParticle;

        private GameObject currentTTrigger = null;

        [Header("PlayerSounds")]
        public SoundDatabase soundDatabase;

        public bool IsDead() => isDead;
        public void MarkDead() => isDead = true;
        public void DisableController() => controller.enabled = false;
        public Animator GetAnimator() => animator;

        [Header("Obstacle Collision")]
        public LayerMask obstacleLayer;

        public PlayerAnimationHandler playerAnimationHandler;
        public PlayerJump playerJump;

        // Turn system
        private bool canTurn = false;

        // Swipe detection
        private Vector2 startTouchPosition;
        private Vector2 swipeDelta;
        private float minSwipeDistance = 50f;

        [Header("Lane Switching")]
        public float laneDistance = 3f;           // Distance between lanes (e.g. 3 meters)
        public float laneChangeSpeed = 10f;       // Speed of lateral movement
        private int currentLane = 1;              // 0 = Left, 1 = Middle, 2 = Right
        private Vector3 targetLanePosition;       // Used for smooth movement
        private bool isChangingLane = false;      // Prevents spam switching


        private Vector3 laneRoot;


        //  private PlayerLaneSwitch laneSwitch;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            playerJump = GetComponent<PlayerJump>();

            //laneSwitch = GetComponent<PlayerLaneSwitch>();
        }

        void OnEnable()
        {
            if (controls == null)
            {
                controls = new PlayerControls();
            }
            controls.Enable();
            animator?.SetTrigger("newjump");
            controls.Player.Jump.performed += ctx => playerJump.Jump();
        }

        void OnDisable()
        {
            controls.Player.Jump.performed -= ctx => playerJump.Jump();
            controls.Disable();
        }

        void Start()
        {
            var input = GetComponent<PlayerInputs>();
            if (input != null)
            {
                input.OnTurnLeft += HandleTurnLeft;
                input.OnTurnRight += HandleTurnRight;
            }

            Time.timeScale = 1f;
            controller = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            controller.center = new Vector3(0.02f, 0.99f, 0f);

            moveDirection = transform.forward;

            if (speedBoostParticle != null)
                speedBoostParticle.SetActive(false);

            if (animator != null)
                animator.updateMode = AnimatorUpdateMode.UnscaledTime;

            targetLanePosition = transform.position; // Start in middle lane

            laneRoot = transform.position;

        }

        void Update()
        {
            if (isDead) return;

            playerJump.HandleGravity();
            Move();

            // Keyboard input for testing
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                playerJump.Jump();
            }

            // Handle touch/swipe input
            HandleSwipeTurn();

        }
        /*
        private void HandleTurnLeft()
        {
            if (!canTurn || allowedTurn == TurnDirection.Right) return;

            transform.Rotate(0, -90, 0);
            moveDirection = transform.forward.normalized;

            if (playerAnimationHandler != null)
                playerAnimationHandler.PlayTurnLeft();

            animator?.SetTrigger("left");

            canTurn = false;

            Debug.Log("Player turned LEFT");
        }

        private void HandleTurnRight()
        {
            if (!canTurn || allowedTurn == TurnDirection.Left) return;

            transform.Rotate(0, 90, 0);
            moveDirection = transform.forward.normalized;
            animator?.SetTrigger("right");

            canTurn = false;
            Debug.Log("Player turned RIGHT");
        }*/
        private void HandleTurnLeft()
        {
            if (!canTurn || allowedTurn == TurnDirection.Right) return;

            transform.Rotate(0, -90, 0);
            moveDirection = transform.forward.normalized;   // 🔥 UPDATED

            laneRoot = transform.position;                  // 🔥 NEW
            StopAllCoroutines();                            // 🔥 NEW
            isChangingLane = false;                         // 🔥 NEW

            if (playerAnimationHandler != null)
                playerAnimationHandler.PlayTurnLeft();
            animator?.SetTrigger("left");

            canTurn = false;
            Debug.Log("Player turned LEFT");
        }
        private void HandleTurnRight()
        {
            if (!canTurn || allowedTurn == TurnDirection.Left) return;

            transform.Rotate(0, 90, 0);
            moveDirection = transform.forward.normalized;   // 🔥 UPDATED

            laneRoot = transform.position;                  // 🔥 NEW
            StopAllCoroutines();                            // 🔥 NEW
            isChangingLane = false;                         // 🔥 NEW

            animator?.SetTrigger("right");
            canTurn = false;
            Debug.Log("Player turned RIGHT");
        }



        public void ResetPlayerState()
        {
            isDead = false;
            canTrigger = true;
            isInvincible = false;
            verticalVelocity = 0f;
            moveDirection = transform.forward;

            controller.enabled = true;

            if (animator != null)
            {
                animator.Play("Medium Run");
            }

            Debug.Log("Player state reset.");
        }

        public void EnableController()
        {
            if (controller != null)
                controller.enabled = true;
        }

        //void Move()
        //{
        //    if (controller == null || playerJump == null) return;

        //    Vector3 forwardMove = transform.forward * moveSpeed * Time.deltaTime;
        //    Vector3 verticalMove = Vector3.up * playerJump.verticalVelocity * Time.deltaTime;

        //    Vector3 lateralMove = Vector3.zero;
        //    //if (laneSwitch != null)
        //    //    lateralMove = laneSwitch.GetLateralMoveDelta();

        //    Vector3 totalMove = forwardMove + verticalMove + lateralMove;
        //    controller.Move(totalMove);


        //}
        void Move()
        {
            if (controller == null || playerJump == null) return;

            // compute moves (same as before)
            Vector3 forwardMove = transform.forward * moveSpeed * Time.deltaTime;
            Vector3 verticalMove = Vector3.up * playerJump.verticalVelocity * Time.deltaTime;

            // lateralMove will come from lane switching coroutine (we don't add it here)
            Vector3 lateralMove = Vector3.zero;

            // total move applied this frame
            Vector3 totalMove = forwardMove + verticalMove + lateralMove;

            // move controller
            controller.Move(totalMove);

            // --- Update laneRoot using **the actual forward portion** of the movement we just applied ---
            // Project the actual move onto forward direction so laneRoot only follows forward progress,
            // and is not affected by lateral changes.
            Vector3 forwardDelta = Vector3.Project(totalMove, transform.forward);
            laneRoot += forwardDelta;
            laneRoot.y = transform.position.y; // keep lane root at player height (optional)
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("TurnPoint") || other.CompareTag("TIntersectionTrigger"))
            {
                canTurn = true;
                TurnPoint tp = other.GetComponent<TurnPoint>();
                allowedTurn = tp != null ? tp.allowedTurn : TurnDirection.Both;
                Debug.Log($"Entered turn point - Allowed Turn: {allowedTurn}");
            }
        }
        private void HandleSwipeTurn()
        {
            if (Touchscreen.current == null) return;

            var touch = Touchscreen.current.primaryTouch;

            if (touch.press.isPressed)
            {
                if (touch.phase.ReadValue() == InputTouchPhase.Began)
                {
                    startTouchPosition = touch.position.ReadValue();
                    swipeDelta = Vector2.zero;
                }
                else if (touch.phase.ReadValue() == InputTouchPhase.Moved ||
                         touch.phase.ReadValue() == InputTouchPhase.Ended)
                {
                    swipeDelta = touch.position.ReadValue() - startTouchPosition;


                    bool isHorizontal = Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y);
                    bool isVertical = Mathf.Abs(swipeDelta.y) > Mathf.Abs(swipeDelta.x);


                    if (isHorizontal && Mathf.Abs(swipeDelta.x) > minSwipeDistance)
                    {
                        bool swipeRight = swipeDelta.x > 0;
                        bool swipeLeft = swipeDelta.x < 0;

                        if (canTurn)
                        {
                            // Turn has priority
                            if (swipeLeft && allowedTurn != TurnDirection.Right)
                                HandleTurnLeft();
                            else if (swipeRight && allowedTurn != TurnDirection.Left)
                                HandleTurnRight();
                        }
                        else
                        {
                            // If not at a turn point — switch lanes
                            if (swipeRight)
                                ChangeLane(1);
                            else if (swipeLeft)
                                ChangeLane(-1);
                        }

                        startTouchPosition = touch.position.ReadValue();
                        swipeDelta = Vector2.zero;
                    }


                    else if (isVertical && swipeDelta.y > minSwipeDistance)
                    {
                        if (playerJump != null)
                        {
                            playerJump.Jump();
                            Debug.Log("Swipe UP - Jump");
                        }
                        startTouchPosition = touch.position.ReadValue();
                        swipeDelta = Vector2.zero;
                    }
                }
            }
            else
            {
                swipeDelta = Vector2.zero;
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("TurnPoint") || other.CompareTag("TIntersectionTrigger"))
            {
                canTurn = false;
                allowedTurn = TurnDirection.Both;


                Debug.Log("Exited turn point - Lane switching enabled");
            }
        }

        //============= lane switch ====================//
        // Calculates world-space lane position based on player's forward direction
        //private Vector3 GetLaneWorldPosition(int lane)
        //{
        //    Vector3 laneDir = Vector3.Cross(Vector3.up, transform.forward);
        //    int laneOffset = lane - 1; // Left = -1, Middle = 0, Right = +1
        //    return transform.position + laneDir * laneDistance * laneOffset;
        //}
        // Calculates lane position relative to laneRoot (which moves forward only)
        private Vector3 GetLaneWorldPosition(int lane)
        {
            Vector3 laneDir = Vector3.Cross(Vector3.up, transform.forward).normalized; // right/left axis
            int laneOffset = lane - 1; // Left = -1, Middle = 0, Right = +1
            return laneRoot + laneDir * laneDistance * laneOffset;
        }

        private void ChangeLane(int direction)
        {
            if (isChangingLane) return; // Prevent rapid switching

            int targetLane = Mathf.Clamp(currentLane + direction, 0, 2);
            if (targetLane == currentLane) return;

            currentLane = targetLane;
            StartCoroutine(SmoothLaneChange());
        }
        //private IEnumerator SmoothLaneChange()
        //{
        //    isChangingLane = true;

        //    Vector3 start = transform.position;
        //    Vector3 target = GetLaneWorldPosition(currentLane);

        //    float t = 0f;
        //    while (t < 1f)
        //    {
        //        t += Time.deltaTime * laneChangeSpeed;
        //        Vector3 newPos = Vector3.Lerp(start, target, t);

        //        // Keep y position from character controller
        //        newPos.y = transform.position.y;

        //        controller.Move(newPos - transform.position);
        //        yield return null;
        //    }

        //    isChangingLane = false;
        //}
        private IEnumerator SmoothLaneChange()
        {
            isChangingLane = true;

            // start position is current world position
            Vector3 start = transform.position;

            // compute stable target (based on laneRoot which follows forward only)
            Vector3 target = GetLaneWorldPosition(currentLane);

            float t = 0f;
            // Use a normalized interpolation (0->1) where speed is controlled by laneChangeSpeed
            // We lerp position but keep the target forward (z) and vertical (y) consistent.
            while (t < 1f)
            {
                t += Time.deltaTime * laneChangeSpeed;
                Vector3 newPos = Vector3.Lerp(start, target, t);

                // keep vertical movement from the controller/jump (so jump still works)
                newPos.y = transform.position.y;

                // move only the lateral/forward delta relative to current transform.position
                Vector3 move = newPos - transform.position;
                // Avoid altering vertical velocity via this lateral move
                move.y = 0f;

                controller.Move(move);

                yield return null;
            }

            isChangingLane = false;
        }
        private void HandleTapInput()
        {
            // Handle no touch (ignore when not touching)
            if (Touchscreen.current == null) return;

            var touch = Touchscreen.current.primaryTouch;

            if (touch.press.wasPressedThisFrame)
            {
                Vector2 tapPosition = touch.position.ReadValue();
                float screenWidth = Screen.width;
                float sectionWidth = screenWidth / 3f; // Divide into 3 equal parts

                // Determine tap zone
                if (tapPosition.x < sectionWidth)
                {
                    // LEFT TAP
                    if (canTurn)
                    {
                        if (allowedTurn != TurnDirection.Right)
                            HandleTurnLeft();
                    }
                    else
                    {
                        ChangeLane(-1);
                    }

                    Debug.Log("Tapped LEFT zone");
                }
                else if (tapPosition.x > 2f * sectionWidth)
                {
                    // RIGHT TAP
                    if (canTurn)
                    {
                        if (allowedTurn != TurnDirection.Left)
                            HandleTurnRight();
                    }
                    else
                    {
                        ChangeLane(1);
                    }

                    Debug.Log("Tapped RIGHT zone");
                }
                else
                {
                    // CENTER TAP
                    if (playerJump != null)
                        playerJump.Jump();

                    Debug.Log("Tapped CENTER zone (JUMP)");
                }
            }
        }

    }
}

