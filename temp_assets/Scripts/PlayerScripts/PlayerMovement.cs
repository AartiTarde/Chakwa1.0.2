using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

using System.Collections.Generic;


public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;
    private PlayerControls controls;
    public static PlayerMovement Instance;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 4.5f;
    public float gravity = 25f;
    private float verticalVelocity;
    private Vector3 moveDirection = Vector3.forward;

    private bool canTurn = false;
    private bool isJumpPressed = false;
    private Vector2 swipeDelta;

    [Header("UI")]
    private bool hasTurned = false; // NEW: Prevent turning multiple times per TurnPoint
    private TurnDirection allowedTurn = TurnDirection.Both; //Turn point script 14/07/2025


    //demon chaser script 
    public EnemyChase demon;
    private bool canTrigger = true;

    //save last position
    private Vector3 lastSafePosition;
    private float safePositionSaveInterval = 1f;
    private float safePositionTimer;

    private bool isDead = false;
    public void ResetDeathFlag() => isDead = false;
    private bool isInvincible = false;

    //Fall detection variable
    private float fallTimer = 0f;
    public float fallThreshold = 1.0f; // Time in air before fall is considered

    //PowerUps 
    private bool coinMagnetEnabled;
    // private float magnetRadius;

    //PowerUpsData
    [Header("PowerUpsData")]
    public PowerUpsData powerUps;

    [Header("Magnet Power-Up")]
    public float magnetRange = 100f;  // The range of the magnet's attraction
    public float magnetSpeed = 50f;  // The speed at which items are pulled toward the player
    int c;
    private bool isMagnetActive = false;


    [Header("Speed Boost Power-Up")]
    //public float boostedSpeed = 10f;'
    private float boostedSpeed = 0f;
    private float originalSpeed;
    private bool isBoostActive = false;
    private float boostTimer = 0f;


    private List<Transform> allCoins = new List<Transform>();


    [Header("Leaderboards")]
    public long currentScore = 0;
    public float submitInterval = 5f;


    [Header("ParticleSystemPrefab")]
    public GameObject speedBoostParticle;
    public GameObject magnetParticle;
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }

        controls = new PlayerControls();

        controls.Player.Jump.performed += ctx => isJumpPressed = true;

        controls.Player.TurnLeft.performed += ctx =>
        {
            if (canTurn) TurnLeft();
        };

        controls.Player.TurnRight.performed += ctx =>
        {
            if (canTurn) TurnRight();
        };

        controls.Player.Swipe.performed += ctx => swipeDelta = ctx.ReadValue<Vector2>();
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
        Time.timeScale = 1f;
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        controller.center = new Vector3(0f, 2.35f, 0f);
        moveDirection = transform.forward;
        boostedSpeed = powerUps.speed;


        //check particle system gameobject false or not 
        if (speedBoostParticle != null || magnetParticle != null)
        {
            speedBoostParticle.SetActive(false);
            //  magnetParticle.SetActive(false);
        }

        InvokeRepeating(nameof(SubmitScoreToLeaderboard), submitInterval, submitInterval);
    }


    void Update()
    {

        HandleSwipeInput();
        if (isJumpPressed)
        {
            Jump();
            isJumpPressed = false;
        }

        Move();
        animator.SetBool("Jump", !controller.isGrounded);
        moveSpeed += 0.1f * Time.deltaTime;

        // Save last safe position every interval
        safePositionTimer += Time.deltaTime;
        if (controller.isGrounded && safePositionTimer >= safePositionSaveInterval)
        {
            lastSafePosition = transform.position;
            safePositionTimer = 0f;
        }
        //Fall detection
        bool isGrounded = controller.isGrounded;
        print("Player on ground :" + isGrounded);
        bool isFalling = !isGrounded && verticalVelocity < -0.1f;
        bool isJumpingUp = !isGrounded && verticalVelocity > 0.1f;

        animator.SetBool("isFalling", isFalling);
        animator.SetBool("Jump", isJumpingUp);

        if (!isDead && isFalling)
        {
            fallTimer += Time.deltaTime;

            if (fallTimer >= fallThreshold)
            {
                Fall();
            }
        }
        else
        {
            fallTimer = 0f; // Reset if not falling
        }
        //Magnet logic
        /*if (isMagnetActive)
        {
            AttractNearbyItems();  
            magnetParticle.SetActive(false);
        }*/
        // Speed boost logic
        if (isBoostActive)
        {
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0f)
            {
                speedBoostParticle.SetActive(false);
                DeactivateSpeedBoost();
            }
        }

        if (isMagnetActive)
        {
            AttractNearbyItems();
            if (!magnetParticle.activeSelf)
                magnetParticle.SetActive(true); // Only activate if not already
        }
        else
        {
            if (magnetParticle.activeSelf)
                magnetParticle.SetActive(false); // Deactivate when not active
        }


    }
    public void Fall()
    {
        if (isDead) return;
        isDead = true;

        controller.enabled = false;
        animator.Play("Fall");

        Debug.Log("Player fell. Showing Game Over panel...");

        StartCoroutine(ShowGameOverAfterFall());
    }
    private IEnumerator ShowGameOverAfterFall()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Ads or direct Game Over
        if (NetworkManager.Instance.IsInternetReachable())
        {
            GameManager.Instance.AdsShow();
            Debug.Log("Internet Available - Showing ads...");
        }
        else
        {
            GameManager.Instance.GameOver();
            Debug.Log("Internet not Available - Showing Game Over.");

            yield return new WaitForSeconds(2f);
            ResetPlayerState();
        }
    }

    public void RespawnPlayer() //respwan player
    {
        Debug.Log("Respawning player at last safe position...");
        controller.enabled = false;

        Debug.Log("last Safe Position" + transform.position);
        transform.position = lastSafePosition;
        controller.enabled = true;

        verticalVelocity = 0f;
        moveDirection = transform.forward;
        animator.Play("Medium Run"); // Change to your idle animation state
        //StartCoroutine(EnableTriggerAfterDelay(2f));
        StartCoroutine(TemporaryInvincibility(2f));
    }
    private IEnumerator TemporaryInvincibility(float duration)
    {
        isInvincible = true;
        canTrigger = false;  // Prevent demon triggers
        Debug.Log("Player is invincible after respawn.");

        yield return new WaitForSeconds(duration);

        isInvincible = false;
        canTrigger = true;
        Debug.Log("Player is vulnerable again.");
    }

    void HandleSwipeInput()
    {
        if (swipeDelta == Vector2.zero) return;

        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            if (swipeDelta.x > 50 && canTurn)
                TurnRight();
            else if (swipeDelta.x < -50 && canTurn)
                TurnLeft();
        }
        else if (swipeDelta.y > 50)
        {
            Jump();
        }

        swipeDelta = Vector2.zero;
    }

    void Jump()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = jumpForce;
            animator.SetTrigger("Jump");
        }
    }

    void Move()
    {

        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -3f;
        else
            verticalVelocity -= gravity * Time.deltaTime;

        Vector3 move = new Vector3(moveDirection.x * moveSpeed, verticalVelocity, moveDirection.z * moveSpeed);
        controller.Move(move * Time.deltaTime);
        FlipCharacter();
        /*Vector3 pos = transform.position;
        move.x = Mathf.Clamp(move.x, -316.4f, -309.2f); // Ensure min < max
        print("clamp is working");
        transform.position = pos;*/
    }
    void FlipCharacter()
    {
        if (moveDirection.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveDirection.x);
            transform.localScale = scale;
        }
    }
    void TurnLeft()
    {

        if (!canTurn || hasTurned) return;

        transform.Rotate(0, -90, 0);
        moveDirection = transform.forward.normalized;
        hasTurned = true;

        animator?.SetTrigger("left");
    }
    void TurnRight()
    {
        if (!canTurn || hasTurned) return;

        transform.Rotate(0, 90, 0);
        moveDirection = transform.forward.normalized;
        hasTurned = true;

        animator?.SetTrigger("right");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TurnPoint"))
        {
            canTurn = true;
            hasTurned = false;

            TurnPoint tp = other.GetComponent<TurnPoint>();
            if (tp != null)
                allowedTurn = tp.allowedTurn;
        }
        if (other.CompareTag("Boundary"))
        {
            Debug.Log("Hit boundary wall — returning to center.");
            transform.position = Vector3.zero;
            moveDirection = transform.forward;
        }

        if (canTrigger && other.gameObject.CompareTag("Tree") || other.gameObject.CompareTag("Temple"))
        {
            canTrigger = false;
            //demon.AppearAfterHurdle();
            StartCoroutine(ResetTrigger());
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("hurdel") || other.gameObject.layer == LayerMask.NameToLayer("Tree"))
        {

            /*demon.ShowDemon();
            demon.HandlePlayerCaught();
            animator.Play("Fall");
            Debug.Log("Collided with HURDLE! Game Over.");
           
            if (NetworkManager.Instance.IsInternetReachable())
            {
                GameManager.Instance.AdsShow();
                print("Internet Available");
            }
            else
            {
                GameManager.Instance.GameOver();
                print("Internet not Available");
            }
            //OnFallDeath(); 
           */
            if (!isInvincible && !isDead)
                StartCoroutine(HandleHurdleCollisionSequence());
        }
        if (other.CompareTag("Magnet"))
        {
            magnetParticle.SetActive(true);
        }
        if (other.CompareTag("SpeedBoost"))
        {
            //speedBoostParticle.SetActive(true);
            ActivateSpeedBoost();
            print("Speed Run Activated");
            other.gameObject.SetActive(false);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (canTrigger && collision.gameObject.CompareTag("Tree") || collision.gameObject.CompareTag("Temple"))
        {
            canTrigger = false;
            //demon.AppearAfterHurdle();
            StartCoroutine(ResetTrigger());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TurnPoint"))
        {
            canTurn = false;
            hasTurned = false; // Reset after leaving the turn zone
            Debug.Log("Exited TurnPoint - can't turn");
        }
    }

    private IEnumerator ResetTrigger()
    {
        yield return new WaitForSeconds(10f); // Delay before demon can be triggered again
        canTrigger = true;
    }
    public void playerControl()
    {
        controller.enabled = true;
    }
    private IEnumerator HandleHurdleCollisionSequence()
    {
        if (isDead) yield break;  // Prevent double-trigger
        isDead = true;

        controller.enabled = false;
        animator.Play("Fall");
        //demon.ShowDemon();
        demon.HandlePlayerCaught();

        Debug.Log("Demon attack started...");

        yield return new WaitForSeconds(1.2f);

        animator.Play("Fall");
        Debug.Log("Playing fall animation...");

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        if (NetworkManager.Instance.IsInternetReachable())
        {
            GameManager.Instance.AdsShow();
            Debug.Log("Internet Available - Showing ads...");
        }
        else
        {
            GameManager.Instance.GameOver();
            Debug.Log("Internet not Available - Showing Game Over.");
        }

        ResetPlayerState();
    }


    //Magnet logic 
    public void ActivateMagnet(float duration)
    {
        isMagnetActive = true;
        magnetParticle.SetActive(true);
        StartCoroutine(DeactivateMagnetAfterTime(duration));
        print("Magnet Activated");
    }


    private IEnumerator DeactivateMagnetAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        isMagnetActive = false;
        magnetParticle.SetActive(false);
        print("Magnet deactivated");
    }
    private void AttractNearbyItems()
    {
        // Get all items within the magnet's range
        Collider[] items = Physics.OverlapSphere(transform.position, magnetRange);

        foreach (var item in items)
        {
            // Only attract objects tagged as "Coin"
            if (item.CompareTag("Coin"))
            {
                print("Magnet Working");
                // Get the world position of the coin
                Vector3 coinWorldPosition = item.transform.position;
                CoinManager.instance.addCount();
                // Calculate the direction to the player
                Vector3 directionToPlayer = (transform.position - coinWorldPosition).normalized;

                // Apply a smooth force towards the player using Lerp or MoveTowards
                // Using Lerp for a smoother pull towards the player
                item.transform.position = Vector3.Lerp(coinWorldPosition, transform.position, magnetSpeed * Time.deltaTime);

                // Optionally add a slight rotation to the coin to make it "rotate" as it moves (adds polish)
                item.transform.Rotate(Vector3.up, magnetSpeed * Time.deltaTime);

                // Check if the coin is close enough to the player
                if (Vector3.Distance(item.transform.position, transform.position) < 1f)
                {
                    // Destroy the coin or deactivate it (deactivating is more performance-friendly)
                    Destroy(item.gameObject);
                }
            }
        }
    }

    //Magnet logic end here


    // Run PowerUps data
    public void ActivateSpeedBoost()
    {
        if (!isBoostActive)
        {
            originalSpeed = moveSpeed;
            magnetParticle.SetActive(true);
            moveSpeed = boostedSpeed;
            isBoostActive = true;
            boostTimer = powerUps.runpowerupTime;
            speedBoostParticle.SetActive(true);
            magnetParticle.SetActive(false);
            Debug.Log("Speed boost activated!");
        }
    }
    private void DeactivateSpeedBoost()
    {
        moveSpeed = originalSpeed;
        isBoostActive = false;
        speedBoostParticle.SetActive(false);
        Debug.Log("Speed boost ended.");
    }
    //speed logic end here
    async void SubmitScoreToLeaderboard()
    {
        if (Leaderboards.Instance != null)
        {
            long finalScore = ScoreManager.instance.GetScore();
            await Leaderboards.Instance.SubmitScoreAsync(finalScore);
        }
    }

    public void ResetPlayerState()
    {
        isDead = false;
        canTrigger = true;
        isInvincible = false;
        verticalVelocity = 0f;
        moveDirection = transform.forward;

        controller.enabled = true;

        animator.Play("Medium Run");
        Debug.Log("Player state reset.");
    }


}
