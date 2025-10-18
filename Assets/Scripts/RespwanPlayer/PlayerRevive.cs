using UnityEngine;

namespace Chakwa.Player
{
    public class PlayerRevive : MonoBehaviour
    {
        private PlayerMovement player;
        private bool isDead;

        private void Awake()
        {
            player = GetComponent<PlayerMovement>();
        }

        public void Die()
        {
            if (isDead) return;
            isDead = true;

            player.DisableController();
            player.GetAnimator().SetBool("DEAD", true);
        }

        public void RevivePlayer()
        {
            Debug.Log("✨ Revive button clicked");

            if (Path.Instance == null)
            {
                Debug.LogError("❌ No Path instance found!");
                return;
            }

            Path.Instance.RespawnPlayerFromBuffer(); // <— use buffer-based respawn

            Animator anim = player.GetAnimator();
            anim.SetBool("DEAD", false);
            anim.SetBool("Run", true);

            player.EnableController();
            player.ResetPlayerState();

            isDead = false;
            Debug.Log("✅ Player revived at tile from buffer.");
        }
    }
}
