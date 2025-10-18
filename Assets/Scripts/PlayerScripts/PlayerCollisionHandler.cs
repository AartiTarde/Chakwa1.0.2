using UnityEngine;
using UnityEngine.UI;     
using UnityEngine.Video;  
using System.Collections;


namespace Chakwa.Player
{

     [RequireComponent(typeof(PlayerMovement))]

    public class PlayerCollisionHandler : MonoBehaviour
    {
        private PlayerMovement player;
        private bool deathTriggered = false;
        public static PlayerCollisionHandler instance;
        public CameraFollow cameraFollow;
        public GameObject fadePanel;
        public Animator animaor;

        private PlayerInputs playerInputs;
        public enum DeathType
        {
            Fall,
            Hurdle,
            Wall,
            Demon
        }

        private void Awake()
        {
            player = GetComponent<PlayerMovement>();

            if (instance == null)
            {
                instance = this;
            }

        }
        void Start()
        {
            cameraFollow = Camera.main.GetComponent<CameraFollow>();
        }
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Collided with: " + other.gameObject.name);

           
            if (other.CompareTag("Boundary"))
            {
                Debug.Log("Hit boundary wall — resetting.");
                player.ResetPlayerState();
                transform.position = Vector3.zero;
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("hurdel") || other.gameObject.layer == LayerMask.NameToLayer("Tree"))
            {
                //HapticManager.PlayHaptic(HapticManager.HapticType.Heavy);
                HapticManager.Vibrate(500);
                HandleDeath(DeathType.Hurdle);
                StopMovement();

                Debug.Log("PlayerCollsionHandler Hurdels ");
            }
            if (other.CompareTag("Magnet"))
            {
                other.gameObject.SetActive(false);
            }
            if (other.CompareTag("SpeedBoost"))
            {
                SoundManager.Instance.PlaySound(player.soundDatabase.boostUpsSound);
                other.gameObject.SetActive(false);
            }
            if (other.CompareTag("TIntersectionTrigger"))
            {
                playerInputs.EnableTurnInput();
                Debug.Log("Reached T-intersection: Waiting for turn input.");
            }
        }

       
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Tree") ||
                collision.gameObject.CompareTag("Temple"))
            {
                HandleDeath(DeathType.Hurdle);
                PlayerMovement.Instance.enabled = false;
                StopMovement();
            }
        }
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Debug.Log("Hit object: " + hit.gameObject.name +
                      " | Layer: " + LayerMask.LayerToName(hit.gameObject.layer));

            if (hit.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                Debug.Log("Wall hit detected!");
                HapticManager.Vibrate(600);
                PlayerMovement.Instance.enabled = false;
                HandleDeath(DeathType.Wall);
                StopMovement();
            }
            else if (hit.gameObject.layer == LayerMask.NameToLayer("PathTree"))
            {
                print("Write here code for reduce speed when player colllide with tree");        
            }

        }
        private void HandleDeath(DeathType deathType)
        {
            if (deathTriggered) return; // already handled
            deathTriggered = true;

            if (player == null || player.IsDead()) return;
            player.MarkDead();

            // Stop movement
            player.DisableController();

            // Animations
            Animator animator = player.GetAnimator();
            EnemyChase demon = player.demon;

            animator.SetBool("Run", false);
            animator.SetBool("isFalling", false);
            animator.SetBool("DEAD", true);

            //SoundManager.Instance.PlaySound(soundDatabase.demonLaughSound);

            switch (deathType)
            {
                case DeathType.Fall:
                    animator.Play("Fall", 0, 0f);
                    Die();
                    GetComponent<PlayerRevive>()?.Die();
                    break;

                case DeathType.Hurdle:
                    animator.Play("Fall", 0, 0f);
                    demon?.HandlePlayerCaught();
                    Die();
                    GetComponent<PlayerRevive>()?.Die();
                    break;

                case DeathType.Wall:
                    animator.Play("Fall", 0, 0f);
                    demon?.HandlePlayerCaught();
                    Die();
                    GetComponent<PlayerRevive>()?.Die();
                    break;

                case DeathType.Demon:
                    animator.Play("Fall", 0, 0f);
                    demon?.HandlePlayerCaught();
                    Die();
                    GetComponent<PlayerRevive>()?.Die();
                    break;
            }

            StartCoroutine(ShowGameOverAfterDelay());
        }

        private void Die()
        {
            // Notify the camera
            if (cameraFollow != null)
            {
                cameraFollow.OnPlayerDeath();
            }

            // Do other death logic (disable movement, play animation, etc.)
            Debug.Log("Player has died!");
        }
        private IEnumerator ShowGameOverAfterDelay()
        {
            if (fadePanel != null)
            {
                animaor.Play("FadeOut");

            }

            yield return new WaitForSecondsRealtime(25f);

            if (NetworkManager.Instance.IsInternetReachable())
            {
                GameManager.Instance.GameOver();
                Debug.Log("Internet Available - Showing ads...");
            }
            else
            {
                GameManager.Instance.GameOver();
                Debug.Log("Internet not Available - Showing Game Over.");
            }
        }

        public void StopMovement()
        {
            // player.enabled = false;
            player.DisableController();
            print("Stop Player Movement");
        }
        public void startMovement()
        {
            //player.enabled = true;
            player.EnableController();
            print("Start Player Movement");
        }
        void Update()
        {
            if (transform.position.y < -10f) // fell off map
            {
                RespawnManager.Instance.Die();
            }
        }
        private Tile GetCurrentTile()
        {
            Ray ray = new Ray(transform.position + Vector3.up * 0.5f, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 5f))
            {
                Tile tile = hit.collider.GetComponentInParent<Tile>();
                if (tile != null)
                    return tile;
            }
            return null;
        }
    }
}

