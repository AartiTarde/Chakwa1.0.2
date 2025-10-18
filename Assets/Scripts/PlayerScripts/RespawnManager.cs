/*using UnityEngine;

public enum PlayerDecision { None, Left, Right }

public class RespawnManager : MonoBehaviour
{
    public Transform player;
    public float invincibleSeconds = 2f;
    public float reviveSpeedScale = 0.8f;

    private Tile lastTile;
    private Transform lastSafePoint;
    private PlayerDecision lastDecision = PlayerDecision.None;
    private float lastDecisionTime = -99f;
    public float decisionGrace = 0.5f;

    public void RegisterTile(Tile tile)
    {
        lastTile = tile;
        if (tile.safeBeforeTurn != null)
            lastSafePoint = tile.safeBeforeTurn;
    }

    public void RegisterDecision(PlayerDecision decision)
    {
        lastDecision = decision;
        lastDecisionTime = Time.time;
    }

    public void OnPlayerDeath(bool duringTurn)
    {
        Debug.Log("Respawning player...");
        Transform target = ChooseRespawnPoint(duringTurn);
        DoRespawn(target);
    }

    private Transform ChooseRespawnPoint(bool duringTurn)
    {
        bool recentDecision = (Time.time - lastDecisionTime) <= decisionGrace;

        if (duringTurn && recentDecision && lastTile != null)
        {
            if (lastTile.tileType == TileType.TIntersection)
            {
                if (lastDecision == PlayerDecision.Left && lastTile.tIntersectionExits.Count > 0)
                    return lastTile.tIntersectionExits[0];
                if (lastDecision == PlayerDecision.Right && lastTile.tIntersectionExits.Count > 1)
                    return lastTile.tIntersectionExits[1];
            }
            else if (lastTile.safeAfterTurn != null)
            {
                return lastTile.safeAfterTurn;
            }
        }

      
        return lastSafePoint != null ? lastSafePoint : player;
    }

    private void DoRespawn(Transform target)
    {
        if (target == null) return;

        var controller = player.GetComponent<CharacterController>();
        if (controller) controller.enabled = false;

        player.SetPositionAndRotation(target.position, target.rotation);

        if (controller) controller.enabled = true;

      
        //var motor = player.GetComponent<IPlayerMotor>();
        //if (motor != null)
        //{
        //    motor.ZeroVelocities();
        //    motor.SetSpeedScale(reviveSpeedScale, invincibleSeconds);
        //}

        //var inv = player.GetComponent<IInvincibility>();
        //if (inv != null)
        //    inv.SetInvincible(invincibleSeconds);

        Debug.Log("Respawned at " + target.name);
    }
    public void RespawnPlayerButton()
    {
        Debug.Log("Respawn button clicked!");
        OnPlayerDeath(false); // respawn as if death happened on straight path
     
    }

}
*/

using UnityEngine;

using Chakwa;
    public class RespawnManager : MonoBehaviour
    {
        public static RespawnManager Instance;

        private Vector3 respawnPosition;
        private Quaternion respawnRotation;

        [Header("Player Reference")]
        public GameObject player;
       
    private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            Instance = this;
        }

        private void Start()
        {
            if (player != null)
            {
                respawnPosition = player.transform.position;
                respawnRotation = player.transform.rotation;
            }
        }

        public void SetRespawnPoint(Vector3 position, Quaternion rotation)
        {
            respawnPosition = position;
            respawnRotation = rotation;
            Debug.Log($"[RespawnManager] SetRespawnPoint -> {respawnPosition}");
        }

        // Called when the player dies. Mark player dead and hide it (UI handled elsewhere).
        public void Die()
        {
            if (player == null) return;

            // Mark movement as dead so tiles won't update checkpoint while player is down
            var pm = player.GetComponent<Chakwa.Player.PlayerMovement>();
            pm?.MarkDead();

            Debug.Log("[RespawnManager] Player died. Last checkpoint: " + respawnPosition);
            player.SetActive(false);

            // Show UI etc. (you already do this in GameManager)
        }

        // Respawn called by button or after ad
        public void Respawn()
        {
            if (player == null) return;

            Debug.Log("[RespawnManager] Respawning at: " + respawnPosition);

        // Option A (recommended): Reset path aligned to checkpoint so tiles are rebuilt correctly.
        if (Path.Instance != null)
        {
            // This should synchronously spawn initial tile and position player onto tile.entryPoint
            Path.Instance.ResetPath(respawnPosition, respawnRotation);
        }
        else
        {
            // Fallback: directly place player at checkpoint
            player.transform.SetPositionAndRotation(respawnPosition, respawnRotation);
        }

        // Reset player state (controller, animator, flags)
        var movement = player.GetComponent<Chakwa.Player.PlayerMovement>();
            movement?.ResetPlayerState();

            // Re-enable player
            player.SetActive(true);

            Debug.Log("[RespawnManager] Respawn complete.");
        }
    }

