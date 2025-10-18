//using UnityEngine;

//public class CameraFollow : MonoBehaviour  //Camera Follow script  
//{
//    public Transform player;
//    public float followDistance = 20f;
//    public float followHeight = 50f;
//    public float followSpeed = 5f;
//    public float lookAtHeight = 1f;

//    public float initialHeight = 100f;         
//    public float transitionDuration = 2f;     

//    private Vector3 targetPosition;
//    private float transitionTimer = 0f;
//    private bool isTransitioning = true;

//    void Start()
//    {
//        if (player == null) return;


//        Vector3 startPos = player.position;
//        startPos.y += initialHeight;
//        transform.position = startPos;


//        transform.LookAt(player.position + Vector3.up * lookAtHeight);
//    }

//    void LateUpdate()
//    {
//        if (player == null) return;

//        if (isTransitioning)
//        {

//            targetPosition = player.position - player.forward * followDistance;
//            targetPosition.y = player.position.y + followHeight;


//            transitionTimer += Time.deltaTime;
//            float t = Mathf.Clamp01(transitionTimer / transitionDuration);
//            transform.position = Vector3.Lerp(transform.position, targetPosition, t);


//            Vector3 lookTarget = player.position + Vector3.up * lookAtHeight;
//            transform.LookAt(lookTarget);

//        }
//        else
//        { 

//            targetPosition = player.position - player.forward * followDistance;

//            targetPosition.y = followHeight;
//            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);

//            Vector3 lookTarget = player.position + Vector3.up * lookAtHeight;
//            transform.LookAt(lookTarget);
//        }
//    }
//}*/
/*
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float followDistance = 20f;
    public float followHeight = 50f;
    public float followSpeed = 5f;
    public float lookAtHeight = 1f;

    public float initialHeight = 100f;
    public float transitionDuration = 2f;

    private Vector3 targetPosition;
    private float transitionTimer = 0f;
    private bool isTransitioning = true;

    void Start()
    {
        if (player == null) return;

        // Start high above player
        Vector3 startPos = player.position;
        startPos.y = initialHeight;
        transform.position = startPos;

        // Look at player
        transform.LookAt(player.position + Vector3.up * lookAtHeight);
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Always lock Y axis to followHeight
        targetPosition = player.position - player.forward * followDistance;
        targetPosition.y = followHeight;

        if (isTransitioning)
        {
            transitionTimer += Time.deltaTime;
            float t = Mathf.Clamp01(transitionTimer / transitionDuration);

            transform.position = Vector3.Lerp(transform.position, targetPosition, t);

            if (t >= 1f) isTransitioning = false;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
        }

        // Look at player
        Vector3 lookTarget = player.position + Vector3.up * lookAtHeight;
        transform.LookAt(lookTarget);
    }
}

*/
/*
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float followDistance = 20f;
    public float followHeight = 50f;
    public float followSpeed = 5f;
    public float lookAtHeight = 1f;

    public float initialHeight = 100f;
    public float transitionDuration = 2f;

    // Death camera settings
    public float deathOrbitSpeed = 30f;
    public float deathCameraHeight = 10f;
    public float deathCameraDistance = 15f;
    private bool isPlayerDead = false;

    private Vector3 targetPosition;
    private float transitionTimer = 0f;
    private bool isTransitioning = true;


    // Pause Arc settings
    public float arcDistance = 10f;
    public float arcHeight = 3f;
    public float arcSpeed = 2f;
    private bool isPaused = false;
    private bool isArcActive = false;
    private Vector3 arcTargetPosition;
    private Quaternion arcTargetRotation;
    private float arcAngle = 0f;
    private float targetArcAngle = 0f;

    public static CameraFollow instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        if (player == null) return;

        // Start high above player
        Vector3 startPos = player.position;
        startPos.y = initialHeight;
        transform.position = startPos;

        // Look at player
        transform.LookAt(player.position + Vector3.up * lookAtHeight);
    }

    void LateUpdate()
    {
        //if (player == null) return;

        //if (isPlayerDead)
        //{
        //    DeathCamera();
        //}
        //else
        //{
        //    FollowCamera();
        //}
        if (player == null) return;

        if (isPlayerDead)
        {
            DeathCamera();
        }
        else if (isArcActive)
        {
            ArcCamera();
        }
        else
        {
            FollowCamera();
        }
    }

    private void FollowCamera()
    {
        // Always lock Y axis to followHeight
        targetPosition = player.position - player.forward * followDistance;
        targetPosition.y = followHeight;

        if (isTransitioning)
        {
            transitionTimer += Time.deltaTime;
            float t = Mathf.Clamp01(transitionTimer / transitionDuration);

            transform.position = Vector3.Lerp(transform.position, targetPosition, t);

            if (t >= 1f) isTransitioning = false;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
        }

        // Look at player
        Vector3 lookTarget = player.position + Vector3.up * lookAtHeight;
        transform.LookAt(lookTarget);
    }

    private void DeathCamera()
    {
        if (player == null) return;

        // Orbit around player
        float angle = Time.time * deathOrbitSpeed;
        Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * deathCameraDistance;

        Vector3 orbitPosition = player.position + offset;
        orbitPosition.y = player.position.y + deathCameraHeight;

        transform.position = orbitPosition;
        transform.LookAt(player.position + Vector3.up * lookAtHeight);
    }

    // Call this when player dies
    public void OnPlayerDeath()
    {
        isPlayerDead = true;
    }
    // NEW: Arc camera (pause cinematic)
    private void ArcCamera()
    {
        if (player == null) return;

        // Smoothly move angle towards target
        arcAngle = Mathf.Lerp(arcAngle, targetArcAngle, Time.unscaledDeltaTime * arcSpeed);

        // Calculate offset based on angle
        float rad = Mathf.Deg2Rad * arcAngle;
        Vector3 offset = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad)) * arcDistance;

        Vector3 desiredPos = player.position + offset + Vector3.up * arcHeight;

        transform.position = Vector3.Lerp(transform.position, desiredPos, Time.unscaledDeltaTime * arcSpeed);

        // Always look at player
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(player.position + Vector3.up * 1.5f - transform.position),
            Time.unscaledDeltaTime * arcSpeed
        );

        // Stop when close to target angle
        if (Mathf.Abs(arcAngle - targetArcAngle) < 0.5f)
        {
            arcAngle = targetArcAngle;
            isArcActive = false;
        }
    }

    // Call this when pausing the game
    public void OnPauseArc()
    {
        if (player == null) return;

        // Calculate current angle relative to player
        Vector3 toCamera = (transform.position - player.position);
        toCamera.y = 0;
        arcAngle = Mathf.Atan2(toCamera.x, toCamera.z) * Mathf.Rad2Deg;

        // Target is in front of the player
        Vector3 forward = player.forward;
        forward.y = 0;
        targetArcAngle = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;

        isArcActive = true;
    }
}
*/





using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float followDistance = 20f;
    public float followHeight = 50f;
    public float followSpeed = 5f;

    public float initialHeight = 100f;
    public float transitionDuration = 2f;

    // Death camera settings
    public float deathOrbitSpeed = 30f;
    public float deathCameraHeight = 10f;
    public float deathCameraDistance = 15f;
    private bool isPlayerDead = false;

    private Vector3 targetPosition;
    private float transitionTimer = 0f;
    private bool isTransitioning = true;

    // Pause Arc settings
    public float arcDistance = 10f;
    public float arcHeight = 3f;
    public float arcSpeed = 2f;
    private bool isArcActive = false;
    private float arcAngle = 0f;
    private float targetArcAngle = 0f;

    public static CameraFollow instance;
    public float lookAtHeight = 1f;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        if (player == null) return;

        // Start high above player
        Vector3 startPos = player.position;
        startPos.y = initialHeight;
        transform.position = startPos;

        // Look at player (horizontally locked)
        Vector3 lookTarget = player.position;
        lookTarget.y = transform.position.y;
        transform.LookAt(lookTarget);
    }

    void LateUpdate()
    {
        if (player == null) return;

        if (isPlayerDead)
        {
            DeathCamera();
        }
        else if (isArcActive)
        {
            ArcCamera();
        }
        else
        {
            FollowCamera();
        }
    }

    private void FollowCamera()
    {
        // Always lock Y axis to followHeight
        targetPosition = player.position - player.forward * followDistance;
        targetPosition.y = followHeight;

        if (isTransitioning)
        {
            transitionTimer += Time.deltaTime;
            float t = Mathf.Clamp01(transitionTimer / transitionDuration);

            transform.position = Vector3.Lerp(transform.position, targetPosition, t);

            if (t >= 1f) isTransitioning = false;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
        }

        // Look at player horizontally
        Vector3 lookTarget = player.position;
        lookTarget.y = transform.position.y;
        //transform.LookAt(lookTarget);
        transform.LookAt(player.position + Vector3.up * lookAtHeight);
    }

    private void DeathCamera()
    {
        if (player == null) return;

        // Orbit around player
        float angle = Time.time * deathOrbitSpeed;
        Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * deathCameraDistance;

        Vector3 orbitPosition = player.position + offset;
        orbitPosition.y = player.position.y + deathCameraHeight;

        transform.position = orbitPosition;

        // Look at player horizontally
        Vector3 lookTarget = player.position;
        lookTarget.y = transform.position.y;
        transform.LookAt(lookTarget);
    }

    // Call this when player dies
    public void OnPlayerDeath()
    {
        isPlayerDead = true;
    }

    // Arc camera (pause cinematic)
    private void ArcCamera()
    {
        if (player == null) return;

        // Smoothly move angle towards target
        arcAngle = Mathf.Lerp(arcAngle, targetArcAngle, Time.unscaledDeltaTime * arcSpeed);

        // Calculate offset based on angle
        float rad = Mathf.Deg2Rad * arcAngle;
        Vector3 offset = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad)) * arcDistance;

        Vector3 desiredPos = player.position + offset + Vector3.up * arcHeight;

        transform.position = Vector3.Lerp(transform.position, desiredPos, Time.unscaledDeltaTime * arcSpeed);

        // Always look at player horizontally
        Vector3 flatTarget = player.position;
        flatTarget.y = transform.position.y;
        Quaternion targetRot = Quaternion.LookRotation(flatTarget - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.unscaledDeltaTime * arcSpeed);

        // Stop when close to target angle
        if (Mathf.Abs(arcAngle - targetArcAngle) < 0.5f)
        {
            arcAngle = targetArcAngle;
            isArcActive = false;
        }
    }

    // Call this when pausing the game
    public void OnPauseArc()
    {
        if (player == null) return;

        // Calculate current angle relative to player
        Vector3 toCamera = (transform.position - player.position);
        toCamera.y = 0;
        arcAngle = Mathf.Atan2(toCamera.x, toCamera.z) * Mathf.Rad2Deg;

        // Target is in front of the player
        Vector3 forward = player.forward;
        forward.y = 0;
        targetArcAngle = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;

        isArcActive = true;
    }
}
