using System.Collections.Generic;
using UnityEngine;

namespace Chakwa
{
    public class Path : MonoBehaviour
    {
        [Header("Themes & Tiles")]
        public List<TileTheme> themes;
        private int currentThemeIndex = 0;

        private TileTheme CurrentTheme
        {
            get
            {
                if (themes == null || themes.Count == 0) return null;
                currentThemeIndex = Mathf.Clamp(currentThemeIndex, 0, themes.Count - 1);
                print("Current Theme Index : " + currentThemeIndex);
                return themes[currentThemeIndex];
            }
        }

        private Vector3 nextSpawnPosition = Vector3.zero;
        private Quaternion nextRotation = Quaternion.identity;

        private readonly List<GameObject> activeTiles = new();
        public int maxTiles = 10;

        [Header("Singleton & Player")]
        public static Path Instance;
        public GameObject player;

        [Header("Obstacles")]
        public List<GameObject> obstaclePrefabs;
        private readonly List<GameObject> activeObstacles = new();
        public int maxObstacles = 5;


        [Header("Collectibles")]
        public CollectiblesManager collectiblesManager;

        // --------------------
        // Revive System Fields
        // --------------------
        private Tile lastDeathTile;
        [Header("Buffers")]
        public List<Tile> spawnedTileBuffer = new List<Tile>();  // 💾 active tile buffer
        public int maxTilesInBuffer = 10;
        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            currentThemeIndex = 0;
        }

        void Start() => SpawnInitialTile();

        #region --- TILE SPAWNING ---
        /*
        void SpawnInitialTile()
        {
            if (themes == null || themes.Count == 0)
            {
                Debug.LogWarning("[Path] No themes assigned.");
                return;
            }

               TileTheme firstTheme = themes[0];
            if (firstTheme == null || firstTheme.straightTile == null)
            {
                Debug.LogWarning("[Path] No initial straight tile found in first theme.");
                return;
            }

            GameObject tile = Instantiate(firstTheme.straightTile, nextSpawnPosition, nextRotation);
            if (!tile.TryGetComponent(out Tile tileScript) || tileScript.entryPoint == null)
            {
                Debug.LogWarning($"[Path] SpawnInitialTile: prefab '{firstTheme.straightTile.name}' missing Tile or entryPoint. Destroying.");
                Destroy(tile);
                return;
            }

            if (player)
            {
                player.transform.SetPositionAndRotation(tileScript.entryPoint.position, tileScript.entryPoint.rotation);
            }
            SetNextSpawnPoint(tileScript);
            activeTiles.Add(tile);
            firstTheme.currentTileIndex = 1;
        }
           */
        void SpawnInitialTile()
        {
            if (themes == null || themes.Count == 0)
            {
                Debug.LogWarning("[Path] No themes assigned.");
                return;
            }

            TileTheme firstTheme = themes[0];
            if (firstTheme == null || firstTheme.straightTile == null)
            {
                Debug.LogWarning("[Path] No initial straight tile found in first theme.");
                return;
            }

            GameObject tile = Instantiate(firstTheme.straightTile, nextSpawnPosition, nextRotation);
            if (!tile.TryGetComponent(out Tile tileScript) || tileScript.entryPoint == null)
            {
                Debug.LogWarning($"[Path] SpawnInitialTile: prefab '{firstTheme.straightTile.name}' missing Tile or entryPoint. Destroying.");
                Destroy(tile);
                return;
            }


            if (player != null)
            {
                player.transform.SetPositionAndRotation(
                    tileScript.entryPoint.position,
                    tileScript.entryPoint.rotation
                );


                player.transform.position = new Vector3(-69.3f, 7f, -0.2f);
                player.transform.rotation = Quaternion.Euler(0f, 90.34278f, 0f);
                player.transform.localScale = new Vector3(6.2f, 6.2f, 6.2f);
            }


            tile.transform.position = nextSpawnPosition;
            tile.transform.rotation = nextRotation;


            SetNextSpawnPoint(tileScript);
            activeTiles.Add(tile);
            firstTheme.currentTileIndex = 1;

            Debug.Log("[Path] Initial tile spawned. Player positioned and transformed correctly.");
        }

        // Replace your existing SpawnInitialTile() with this:
        public void SpawnNextTile()
        {
            if (CurrentTheme == null)
            {
                Debug.LogWarning("[Path] SpawnNextTile called but no CurrentTheme.");
                return;
            }

            if (CurrentTheme.IsFinished())
            {
                currentThemeIndex = (currentThemeIndex + 1) % themes.Count;

                CurrentTheme.Reset();
                Debug.Log("[Path] Switched to Theme: " + currentThemeIndex);
            }

            GameObject prefab = CurrentTheme.GetNextTile();
            if (!prefab)
            {
                Debug.LogWarning("[Path] Missing prefab in current theme.");
                return;
            }

            GameObject tile = SpawnTile(prefab);
            if (tile)
            {
                activeTiles.Add(tile);

                if (collectiblesManager != null && tile.TryGetComponent(out Tile tileScript))
                {
                    collectiblesManager.TrySpawnCollectibles(tileScript);
                }
            }

            CleanupOldObjects(activeTiles, maxTiles);
            SpawnObstacleNearNextTile();
        }

        GameObject SpawnTile(GameObject prefab)
        {
            if (prefab == null)
            {
                Debug.LogWarning("[Path] SpawnTile received null prefab.");
                return null;
            }

            GameObject tile = Instantiate(prefab);
            if (!tile.TryGetComponent(out Tile tileScript) || tileScript.entryPoint == null)
            {
                Debug.LogWarning($"[Path] Tile prefab '{prefab.name}' missing Tile component or entryPoint. Destroying spawned object.");
                Destroy(tile);
                return null;
            }
            tile.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            Quaternion desiredRotation = nextRotation * Quaternion.Inverse(tileScript.entryPoint.localRotation);
            Vector3 euler = desiredRotation.eulerAngles;
            desiredRotation = Quaternion.Euler(0f, euler.y, 0f);

            tile.transform.rotation = desiredRotation;

            Vector3 entryWorldPos = tile.transform.TransformPoint(tileScript.entryPoint.localPosition);
            tile.transform.position = nextSpawnPosition - (entryWorldPos - tile.transform.position);

            SetNextSpawnPoint(tileScript);

            return tile;
        }


        void SetNextSpawnPoint(Tile tileScript)
        {
            if (tileScript == null)
            {
                Debug.LogWarning("[Path] SetNextSpawnPoint called with null Tile.");
                return;
            }

            if (tileScript.tileType == TileType.TIntersection && tileScript.tIntersectionExits != null && tileScript.tIntersectionExits.Count > 0)
            {
                Transform exit = tileScript.tIntersectionExits[Random.Range(0, tileScript.tIntersectionExits.Count)];
                if (exit != null)
                {
                    nextSpawnPosition = exit.position;
                    nextRotation = exit.rotation;
                }
                else
                {
                    Debug.LogWarning("Path TIntersection exit was null - keeping previous spawn point.");
                }
            }
            else if (tileScript.exitPoint != null)
            {
                nextSpawnPosition = tileScript.exitPoint.position;
                nextRotation = tileScript.exitPoint.rotation;
            }
            else
            {
                Debug.LogWarning("Path Tile has no exitPoint (and isn't a usable TIntersection).");
            }
        }

        #endregion

        #region --- TURNING ---

        //public void TriggerTurn(bool leftTurn)
        //{
        //    if (CurrentTheme == null)
        //    {
        //        Debug.LogWarning("Path TriggerTurn called but no CurrentTheme.");
        //        return;
        //    }

        //    GameObject prefab = leftTurn ? CurrentTheme.leftTurnTile : CurrentTheme.rightTurnTile;
        //    if (!prefab)
        //    {
        //        Debug.LogWarning("Path Turn prefab missing in current theme.");
        //        return;
        //    }

        //    GameObject tile = SpawnTile(prefab);
        //    if (tile)
        //    {
        //        activeTiles.Add(tile);
        //        Debug.Log(leftTurn ? "This is LEFT turn of T tile" : "This is RIGHT turn of T tile");
        //        CleanupOldObjects(activeTiles, maxTiles);
        //    }
        //}
        public void TriggerTurn(bool leftTurn)
        {
            if (CurrentTheme == null)
            {
                Debug.LogWarning("[Path] TriggerTurn called but no CurrentTheme.");
                return;
            }

            // Pick correct prefab
            GameObject prefab = leftTurn ? CurrentTheme.leftTurnTile : CurrentTheme.rightTurnTile;
            Debug.Log($"[DEBUG] TriggerTurn: leftTurn={leftTurn}, prefab={(prefab ? prefab.name : "null")}");

            if (prefab == null)
            {
                Debug.LogWarning("[Path] Missing turn prefab.");
                return;
            }

            // Spawn the new tile
            GameObject tile = Instantiate(prefab);
            if (!tile.TryGetComponent(out Tile tileScript))
            {
                Debug.LogWarning("[Path] Spawned prefab missing Tile component.");
                Destroy(tile);
                return;
            }

            // Normalize previous rotation
            Vector3 euler = nextRotation.eulerAngles;
            euler.x = 0f; euler.z = 0f;
            euler.y = Mathf.Round(euler.y / 90f) * 90f;
            nextRotation = Quaternion.Euler(euler);

            // Align the new tile’s entry to the previous exit
            tile.transform.rotation = nextRotation;
            Vector3 offset = tileScript.entryPoint.position - tile.transform.position;
            tile.transform.position = nextSpawnPosition - offset;

            // Now pick deterministic exit based on player input
            if (tileScript.tIntersectionExits != null && tileScript.tIntersectionExits.Count >= 2)
            {
                int chosenIndex = leftTurn ? 0 : 1; // always 0=Left, 1=Right
                Transform exit = tileScript.tIntersectionExits[chosenIndex];
                print("Exit position is :" + exit.position);
                nextSpawnPosition = exit.position;
                nextRotation = exit.rotation;
               
                // Snap rotation to perfect 90° increments
                Vector3 nEuler = nextRotation.eulerAngles;
                nEuler.x = 0f; nEuler.z = 0f;
                nEuler.y = Mathf.Round(nEuler.y / 90f) * 90f;
                nextRotation = Quaternion.Euler(nEuler);

                Debug.Log($"[Path] TriggerTurn: Used exit[{chosenIndex}] for {(leftTurn ? "LEFT" : "RIGHT")} turn. New nextRotation={nextRotation.eulerAngles}");
            }
            else if (tileScript.exitPoint != null)
            {
                // Fallback for normal turn tiles
                nextSpawnPosition = tileScript.exitPoint.position;
                nextRotation = tileScript.exitPoint.rotation;
            }
            else
            {
                Debug.LogWarning("[Path] Turn tile missing exits!");
            }

            // Final cleanup
            activeTiles.Add(tile);
            CleanupOldObjects(activeTiles, maxTiles);
        }



        #endregion

        #region --- RANDOM TILE ---

        public GameObject GetRandomTilePrefab()
        {
            if (CurrentTheme == null) return null;

            var possibleTiles = new List<GameObject>();
            if (CurrentTheme.straightTile) possibleTiles.Add(CurrentTheme.straightTile);
            if (CurrentTheme.leftTurnTile) possibleTiles.Add(CurrentTheme.leftTurnTile);
            if (CurrentTheme.rightTurnTile) possibleTiles.Add(CurrentTheme.rightTurnTile);

            return possibleTiles.Count > 0 ? possibleTiles[Random.Range(0, possibleTiles.Count)] : null;
        }

        public void SpawnRandomTile()
        {
            if (CurrentTheme == null) return;

            GameObject prefab = GetRandomTilePrefab();
            if (!prefab)
            {
                Debug.LogWarning("Path No tile prefabs available for random spawn.");
                return;
            }

            GameObject tile = SpawnTile(prefab);
            if (tile) activeTiles.Add(tile);

            CleanupOldObjects(activeTiles, maxTiles);
        }

        #endregion

        #region --- OBSTACLES ---

        public void SpawnObstacle(Vector3 position, Quaternion rotation)
        {
            if (obstaclePrefabs == null || obstaclePrefabs.Count == 0)
            {
                Debug.LogWarning("Path No obstacle prefabs assigned.");
                return;
            }

            GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];
            if (prefab == null)
            {
                Debug.LogWarning("Path Selected obstacle prefab is null.");
                return;
            }

            GameObject obstacle = Instantiate(prefab, position, rotation);
            activeObstacles.Add(obstacle);

            CleanupOldObjects(activeObstacles, maxObstacles);
        }

        public void SpawnObstacleNearNextTile()
        {
            Vector3 offset = new Vector3(Random.Range(-2f, 2f), 1f, Random.Range(-2f, 2f));
            SpawnObstacle(nextSpawnPosition + offset, Quaternion.identity);
        }

        #endregion

        #region --- CLEANUP ---
        void CleanupOldObjects(List<GameObject> list, int maxAllowed)
        {

            list.RemoveAll(item => item == null);

            while (list.Count > maxAllowed)
            {
                if (list[0] != null) Destroy(list[0]);
                list.RemoveAt(0);
            }
        }
        //ResetPath
        // Add this overload to Reset the entire path at a checkpoint:
        public void ResetPath(Vector3 startPos, Quaternion startRot)
        {
            Debug.Log("[Path] ResetPath at checkpoint: " + startPos);

            // destroy current tiles & obstacles
            foreach (var t in activeTiles) if (t != null) Destroy(t);
            activeTiles.Clear();

            foreach (var o in activeObstacles) if (o != null) Destroy(o);
            activeObstacles.Clear();

            // reset state
            currentThemeIndex = 0;
            nextSpawnPosition = startPos;
            nextRotation = startRot;
            if (themes != null)
            {
                foreach (var theme in themes) theme?.Reset();
            }

            // spawn initial tile aligned to nextSpawnPosition/nextRotation (and put player on entryPoint)
            SpawnInitialTile();
        }
        // Returns the best exit index for left (wantLeft=true) or right (wantLeft=false).
        // Uses vector signed angle (entry->exit) around Y: positive = left, negative = right.
        // --- robust helper to choose exit index for left/right ---
        int GetBestTExitIndex(Tile tileScript, bool wantLeft)
        {
            if (tileScript == null || tileScript.tIntersectionExits == null || tileScript.tIntersectionExits.Count == 0)
                return -1;

            Vector3 entryDir = tileScript.entryPoint.forward.normalized;
            int bestIndex = -1;
            float bestScore = float.NegativeInfinity;

            // First pass: prefer correct sign (+ for left, - for right) and magnitude close to 90 deg
            for (int i = 0; i < tileScript.tIntersectionExits.Count; i++)
            {
                Transform exit = tileScript.tIntersectionExits[i];
                if (exit == null) continue;
                Vector3 exitDir = exit.forward.normalized;
                float angle = Vector3.SignedAngle(entryDir, exitDir, Vector3.up); // left>0 right<0

                // score: prefer angle magnitude near 90 in desired sign
                float score;
                if (wantLeft)
                {
                    score = (angle > 0f) ? (angle) : (angle - 10000f); // large negative penalty for wrong sign
                }
                else
                {
                    score = (angle < 0f) ? (-angle) : (-10000f - angle); // penalty for wrong sign
                }

                if (score > bestScore)
                {
                    bestScore = score;
                    bestIndex = i;
                }
            }

            // If first pass failed to find a meaningful index (e.g. both penalized), do a fallback: choose the exit with largest absolute signed-angle toward desired side
            if (bestIndex < 0)
            {
                float bestAbs = float.NegativeInfinity;
                for (int i = 0; i < tileScript.tIntersectionExits.Count; i++)
                {
                    Transform exit = tileScript.tIntersectionExits[i];
                    if (exit == null) continue;
                    float angle = Vector3.SignedAngle(entryDir, exit.forward.normalized, Vector3.up);
                    float absScore = Mathf.Abs(angle);
                    if (absScore > bestAbs)
                    {
                        bestAbs = absScore;
                        bestIndex = i;
                    }
                }
            }

            return bestIndex;
        }

        // =============================================
        // ========== BUFFER MANAGEMENT =================
        // =============================================

        private void AddTileToBuffer(Tile tile)
        {
            if (tile == null) return;

            spawnedTileBuffer.Add(tile);

            if (spawnedTileBuffer.Count > maxTilesInBuffer)
            {
                Tile oldest = spawnedTileBuffer[0];
                if (oldest != null)
                    Destroy(oldest.gameObject);
                spawnedTileBuffer.RemoveAt(0);
            }

            Debug.Log($"[Path] 📦 Tile added to buffer ({spawnedTileBuffer.Count} total): {tile.name}");
        }

        public Tile GetLastTileFromBuffer()
        {
            if (spawnedTileBuffer.Count == 0)
            {
                Debug.LogWarning("[Path] ❌ Tile buffer empty!");
                return null;
            }

            return spawnedTileBuffer[spawnedTileBuffer.Count - 1];
        }

        //===============Revive system called here=======================//
        // =============================================
        // ========== REVIVE LOGIC ======================
        // =============================================

        public void RespawnPlayerFromBuffer()
        {
            if (player == null)
            {
                Debug.LogError("[Path] ❌ No player assigned!");
                return;
            }

            // ✅ Step 1: Get the most recent valid tile
            Tile reviveTile = null;
            for (int i = spawnedTileBuffer.Count - 1; i >= 0; i--)
            {
                if (spawnedTileBuffer[i] != null)
                {
                    reviveTile = spawnedTileBuffer[i];
                    break;
                }
            }

            if (reviveTile == null)
            {
                Debug.LogWarning("[Path] ⚠️ No valid tile found in buffer, respawning at start.");
                SpawnInitialTile();
                return;
            }

            // ✅ Step 2: Find revive point on this tile
            RevivePoint revivePoint = reviveTile.GetComponentInChildren<RevivePoint>();
            if (revivePoint == null)
            {
                Debug.LogWarning($"[Path] ⚠️ No RevivePoint found in tile: {reviveTile.name}. Using tile center instead.");
                revivePoint = reviveTile.gameObject.AddComponent<RevivePoint>();
                revivePoint.reviveTransform = reviveTile.transform;
            }

            Transform revivePos = revivePoint.reviveTransform != null
                ? revivePoint.reviveTransform
                : revivePoint.transform;

            // ✅ Step 3: Use revive point position + tile alignment
            Vector3 targetPos = revivePos.position;
            Quaternion targetRot = reviveTile.transform.rotation;

            // Align Y height exactly to the tile’s top
            targetPos.y = reviveTile.transform.position.y + 1f;

            // ✅ Step 4: Teleport player cleanly
            player.transform.position = targetPos;
            player.transform.rotation = targetRot;

            // Reset physics
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            Debug.Log($"[Path] ✅ Player respawned on tile '{reviveTile.name}' at {targetPos}");

            // ✅ Step 5: Continue spawning forward
            if (reviveTile.exitPoint != null)
            {
                nextSpawnPosition = reviveTile.exitPoint.position;
                nextRotation = reviveTile.exitPoint.rotation;
            }

            for (int i = 0; i < 5; i++)
                SpawnNextTile();

            // ✅ Step 6: Optional debug
            Debug.DrawRay(targetPos, Vector3.up * 3, Color.green, 3f);
        }


        #endregion

        #region --- EDITOR SAFETY ---

#if UNITY_EDITOR
        void OnValidate()
        {
            if (maxTiles < 1) maxTiles = 1;
            if (maxObstacles < 0) maxObstacles = 0;
         }
#endif

        #endregion
    }
}
