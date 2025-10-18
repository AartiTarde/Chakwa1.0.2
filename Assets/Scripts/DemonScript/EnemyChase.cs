/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EnemyChase : MonoBehaviour
{
    public Transform player;
    public float followDistance = 5f;
    public float catchUpSpeed = 5f;
    public float minDistance = 1f;

    [Header("Timing Settings")]
    public float initialVisibleTime = 5f;       
    public float invisibleTimeAfterVisible = 5f;
    public float visibleTimeAfterCollision = 5f; 

    private float currentDistance;
    private bool isChasing = false;
    private Renderer demonRenderer;

    private Coroutine visibilityCycleCoroutine;
    public  Animator demonAnimator;
    private bool isPlayerDead = false;


    void Start()
    {
        currentDistance = followDistance;
        demonRenderer = GetComponentInChildren<Renderer>();

        if (demonRenderer == null)
        {
            Debug.LogError("Renderer not found on demon or children! Please assign Renderer.");
        }

        
        ShowDemon();
        isChasing = true;

      
        visibilityCycleCoroutine = StartCoroutine(VisibilityCycle());
    }

    void Update()
    {
        if (!isChasing) return;

        Vector3 targetPosition = player.position - player.forward * currentDistance;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * catchUpSpeed);

        transform.LookAt(player);

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= minDistance)
        {
            Debug.Log("Caught by the enemy! Game Over!");
            HandlePlayerCaught();

        }
    }
    public void OnPlayerStumble()
    {
        currentDistance = Mathf.Max(currentDistance - 2f, minDistance);
    }
    public void OnPlayerRecover()
    {
        currentDistance = Mathf.Min(currentDistance + 0.5f, followDistance);
    }
    private IEnumerator VisibilityCycle()
    {
        // Demon visible and chasing initially
        ShowDemon();
        isChasing = true;

        yield return new WaitForSeconds(initialVisibleTime);

        
        HideDemon();
        isChasing = false;

        yield return new WaitForSeconds(invisibleTimeAfterVisible);

      
    }
    private IEnumerator VisibleAfterCollisionCycle()
    {
        ShowDemon();
        isChasing = true;

        yield return new WaitForSeconds(visibleTimeAfterCollision);

        HideDemon();
        isChasing = false;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Demon collided with obstacle.");

            if (isChasing && demonRenderer.enabled) return;

            if (visibilityCycleCoroutine != null)
            {
                StopCoroutine(visibilityCycleCoroutine);
            }
            visibilityCycleCoroutine = StartCoroutine(VisibleAfterCollisionCycle());
        }
    }

    private void HideDemon()
    {
        if (demonRenderer != null)
            demonRenderer.enabled = false;
    }
    
    public void AppearAfterHurdle()
    {
     
        if (visibilityCycleCoroutine != null)
        {
            StopCoroutine(visibilityCycleCoroutine);
        }
        visibilityCycleCoroutine = StartCoroutine(VisibleAfterCollisionCycle());
    }
    public void ShowDemon()
    {
        if (demonRenderer != null)
            demonRenderer.enabled = true;

        // Optional: Reset demon position behind player
        if (player != null)
        {
            Vector3 offset = -player.forward * followDistance + Vector3.up * 0.5f;
            transform.position = player.position + offset;
            transform.LookAt(player);
        }
    }
    public void HandlePlayerCaught()
    {
        Debug.Log("Caught by the enemy! Game Over!");
        isPlayerDead = true;
        isChasing = false;

        
        ShowDemon();

     
        if (visibilityCycleCoroutine != null)
        {
            StopCoroutine(visibilityCycleCoroutine);
            StartCoroutine(DelayGameOverUntilAnimationEnds());
        }

       
        if (demonAnimator != null)
        {
           
            demonAnimator.SetBool("Attack",true);
            print("Demon Animator display");
        }
       
    }
    private IEnumerator DelayGameOverUntilAnimationEnds()
    {
        float animLength = 1f; 
        yield return new WaitForSeconds(animLength);
        Time.timeScale = 1f;
        demonAnimator.SetBool("Attack", false); 
        
    }

}
*/

/*
using UnityEngine;
using System.Collections;

public class EnemyChase : MonoBehaviour
{
    public Transform player;
    public float followDistance = 5f;
    public float catchUpSpeed = 5f;
    public float minDistance = 1f;

    [Header("Timing Settings")]
    public float initialVisibleTime = 5f;
    public float invisibleTimeAfterVisible = 5f;
    public float visibleTimeAfterCollision = 5f;

    private float currentDistance;
    private bool isChasing = true; // Demon is always chasing now
    private Renderer demonRenderer;
    private Coroutine visibilityCycleCoroutine;
    public Animator demonAnimator;
    private bool isPlayerDead = false;


   // [Header("Speed")]
    //public float demonSpeed = 3f; // Add demon speed for more flexibility

    void Start()
    {
        currentDistance = followDistance;
        demonRenderer = GetComponentInChildren<Renderer>();

        if (demonRenderer == null)
        {
            Debug.LogError("Renderer not found on demon or children! Please assign Renderer.");
        }

        ShowDemon();
        visibilityCycleCoroutine = StartCoroutine(VisibilityCycle());
    }

    void Update()
    {
        if (!isChasing || player == null) return;

        // Move the demon towards the player
        Vector3 targetPosition = player.position - player.forward * currentDistance;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * catchUpSpeed);
        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, demonSpeed * Time.deltaTime);
        transform.LookAt(player);

        // Check if the demon has caught the player
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= minDistance)
        {
            Debug.Log("Caught by the enemy! Game Over!");
            HandlePlayerCaught();
        }
    }
    public void OnPlayerStumble()
    {
        currentDistance = Mathf.Max(currentDistance - 2f, minDistance);
    }
    public void OnPlayerRecover()
    {
        currentDistance = Mathf.Min(currentDistance + 0.5f, followDistance);
    }
    private IEnumerator VisibilityCycle()
    {
        // Keep the demon visible at all times (no hiding)
        ShowDemon();
        isChasing = true;
        yield return null; // Nothing to wait for, demon will chase the player continuously
    }
    private IEnumerator VisibleAfterCollisionCycle()
    {
        ShowDemon();
        isChasing = true;
        yield return new WaitForSeconds(visibleTimeAfterCollision);
        HideDemon();
        isChasing = false;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Demon collided with obstacle.");

            if (isChasing && demonRenderer.enabled) return;

            if (visibilityCycleCoroutine != null)
            {
                StopCoroutine(visibilityCycleCoroutine);
            }

            visibilityCycleCoroutine = StartCoroutine(VisibleAfterCollisionCycle());
        }
    }

    private void HideDemon()
    {
        if (demonRenderer != null)
            demonRenderer.enabled = false;
    }

    public void AppearAfterHurdle()
    {
        if (visibilityCycleCoroutine != null)
        {
            StopCoroutine(visibilityCycleCoroutine);
        }
        visibilityCycleCoroutine = StartCoroutine(VisibleAfterCollisionCycle());
    }

    public void ShowDemon()
    {
        if (demonRenderer != null)
            demonRenderer.enabled = true;

        
        if (player != null)
        {
            Vector3 offset = -player.forward * followDistance + Vector3.up * 0.5f;
            transform.position = player.position + offset;
            transform.LookAt(player);
        }
    }

    public void HandlePlayerCaught()
    {
        Debug.Log("Caught by the enemy! Game Over!");
        isPlayerDead = true;
        isChasing = false;

        ShowDemon();

        if (visibilityCycleCoroutine != null)
        {
            StopCoroutine(visibilityCycleCoroutine);
            StartCoroutine(DelayGameOverUntilAnimationEnds());
        }
        if (demonAnimator != null)
        {
            demonAnimator.SetBool("Attack", true);
            print("Demon Animator display");
        }
    }

    private IEnumerator DelayGameOverUntilAnimationEnds()
    {
        float animLength = 1f;

        yield return new WaitForSeconds(animLength);
        Time.timeScale = 1f;
        demonAnimator.SetBool("Attack", false);
    }
}



*/


using UnityEngine;
using System.Collections;

public class EnemyChase : MonoBehaviour
{
    public Transform player;
    public float followDistance = 5f;
    public float catchUpSpeed = 5f;
    public float minDistance = 1f;

    public Animator demonAnimator;

    private float currentDistance;
    private bool isChasing = true;
    private bool isPlayerDead = false;


    public LayerMask groundLayer;  
    public float groundCheckDistance = 0.1f;  
    private bool isOnGround = false;


    [Header("Fade Images")]
    public GameObject fadeOutImage;      
    public Animator fadeOutAnimator;

    public GameObject fadeInImage;       
    public Animator fadeInAnimator;
    public SoundDatabase soundDatabase;

    //  private CharacterController controller;
    /*void Start()
    {
        currentDistance = followDistance;
      
        if (demonAnimator == null)
        {
            Debug.LogError("Animator not assigned!");
        }
        demonAnimator?.SetTrigger("run");
    
       
    }*/
    void Start()
    {
        //controller = GetComponent<CharacterController>();
        currentDistance = followDistance;

        if (demonAnimator == null)
        {
            Debug.LogError("Animator not assigned!");
        }
        demonAnimator?.SetTrigger("run");

    }


    void Update()
    {
        FollowPlayer();
        CheckIfOnGround();

        if (!isPlayerDead)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= minDistance)
            {
                HandlePlayerCaught();
            }
        }
    }


    public void OnPlayerStumble()
    {
        currentDistance = Mathf.Max(currentDistance - 2f, minDistance);
    }

    public void OnPlayerRecover()
    {
        currentDistance = Mathf.Min(currentDistance + 0.5f, followDistance);
    }

    public void HandlePlayerCaught()
    {
        if (isPlayerDead) return;

        isPlayerDead = true;
        Debug.Log("Caught by the enemy! Playing attack and win...");

        if (demonAnimator != null)
        {
            currentDistance = 5;
            demonAnimator.ResetTrigger("run");
           
            demonAnimator.SetTrigger("Attack");
        }

        StartCoroutine(PerformAttackAndWinThenResume());
    }
    private IEnumerator PerformAttackAndWinThenResume()
    {
        float attackDuration = 2f;
        float winDuration = 2f;


        yield return new WaitForSeconds(attackDuration);

        if (demonAnimator != null)
        {

            SoundManager.Instance.PlaySound(soundDatabase.demonLaughSound);
            demonAnimator.SetTrigger("Won");
        }
        
        yield return new WaitForSeconds(winDuration);


        fadeOutImage.SetActive(true);
        fadeOutAnimator.CrossFade("FadeOut", 0f);

  
        float fadeOutTime = fadeOutAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(fadeOutTime);

   
        fadeOutImage.SetActive(false);
  

        fadeInImage.SetActive(false);
        GameManager.Instance.GameOver();
        Debug.Log("Game Over panel revealed with FadeIn.");
    }
    public void ResetAfterAdContinue()
    {
        Debug.Log("Player continued after ad. Resetting demon state.");
        currentDistance = followDistance;
        isPlayerDead = false;
        isChasing = true;

        if (demonAnimator != null)
        {
            demonAnimator.SetBool("Attack", false);
            demonAnimator.ResetTrigger("Won");
            demonAnimator.SetTrigger("Run");
        }
    }
    
    private void FollowPlayer()
    {
        Debug.Log("Following player with distance: " + currentDistance);
        Vector3 targetPosition = player.position - player.forward * currentDistance;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * catchUpSpeed);
        transform.LookAt(player);
    }
    //private void FollowPlayer()
    //{
    //    Debug.Log("Following player with distance: " + currentDistance);
    //    Vector3 targetPosition = player.position - player.forward * currentDistance;
    //    Vector3 moveDir = (targetPosition - transform.position);

    //    // Don’t teleport directly — move via CharacterController
    //   // controller.Move(moveDir * catchUpSpeed * Time.deltaTime);

    //    transform.LookAt(player);
    //}

    
    private void CheckIfOnGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, groundLayer))
        {
            isOnGround = true;
        }
        else
        {
            isOnGround = false;
        }
    }
    //void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    if (hit.collider.CompareTag("Sider"))
    //    {
    //        Debug.Log("Demon (CharacterController) hit Sider → increasing follow distance!");
    //        currentDistance = 40f;
    //    }
    //}


    //public  void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Sider"))
    //    {
    //        Debug.Log("Demon collided with Sider → increasing follow distance!");
    //        currentDistance = 40f;
    //    }
    //}
}