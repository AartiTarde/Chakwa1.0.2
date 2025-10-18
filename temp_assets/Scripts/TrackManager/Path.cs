/*using System.Collections.Generic;
using UnityEngine;

namespace Chakwa 
{
    public class Path : MonoBehaviour
    {
        public List<TileTheme> themes;
        private int currentThemeIndex = 0;
        private TileTheme CurrentTheme => themes[currentThemeIndex];

        private Vector3 nextSpawnPosition = Vector3.zero;
        private Quaternion nextRotation = Quaternion.identity;

        private List<GameObject> activeTiles = new List<GameObject>();
        public int maxTiles = 10;

        public static Path Instance;
        public GameObject player;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
        }
        void Start()
        {
            SpawnInitialTile();
        }
        void SpawnInitialTile()
        {
            if (themes.Count == 0 || themes[0].straightTile == null)
            {
                Debug.LogWarning("No initial tile found in first theme.");
                return;
            }

            GameObject tile = Instantiate(themes[0].straightTile, nextSpawnPosition, nextRotation);
            Tile tileScript = tile.GetComponent<Tile>();
            if (tileScript != null && tileScript.entryPoint != null && player != null)
            {
                player.transform.position = tileScript.entryPoint.position;
                player.transform.rotation = tileScript.entryPoint.rotation;
            }
            UpdateNextTilePosition(tile);
            activeTiles.Add(tile);
            themes[0].currentTileIndex = 1; 
        }
        public void SpawnNextTile()
        {
            if (themes.Count == 0) return;

            if (CurrentTheme.IsFinished())
            {
                currentThemeIndex = (currentThemeIndex + 1) % themes.Count;
                CurrentTheme.Reset();
                Debug.Log(" Switched to Theme : " + currentThemeIndex);
            }

            GameObject prefab = CurrentTheme.GetNextTile();
            if (prefab == null)
            {
                Debug.LogWarning("Missing prefab in current theme.");
                return;
            }
            CleanupOldTiles();
            bool isT = prefab.GetComponent<Tile>()?.tileType == TileType.TIntersection;
         
            GameObject tile = SpawnTile(prefab, isT);

            if (tile != null)
                activeTiles.Add(tile);
        }
        GameObject SpawnTile(GameObject prefab, bool isTIntersection = false)
        {
            GameObject tile = Instantiate(prefab);
            Tile tileScript = tile.GetComponent<Tile>();

            if (tileScript == null || tileScript.entryPoint == null)
            {
                Debug.LogWarning("Tile missing entry point.");
                Destroy(tile);
                return null;
            }
          tile.transform.position = Vector3.zero;
          tile.transform.rotation = Quaternion.identity;


          Quaternion alignRotation = nextRotation * Quaternion.Inverse(tileScript.entryPoint.rotation);
          tile.transform.rotation = alignRotation * tile.transform.rotation;


          Vector3 offset = nextSpawnPosition - tileScript.entryPoint.position;
          tile.transform.position += offset;


          if (isTIntersection && tileScript.tileType == TileType.TIntersection && tileScript.tIntersectionExits.Count > 0)
          {
              Transform chosenExit = tileScript.tIntersectionExits[Random.Range(0, tileScript.tIntersectionExits.Count)];
              nextSpawnPosition = chosenExit.position;
              nextRotation = chosenExit.rotation;
          }
          else if (tileScript.exitPoint != null)
          {
              nextSpawnPosition = tileScript.exitPoint.position;
              nextRotation = tileScript.exitPoint.rotation;
          }
        return tile;
    }
    void UpdateNextTilePosition(GameObject tile)
    {
          Tile tileScript = tile.GetComponent<Tile>();
          if (tileScript.tileType == TileType.TIntersection && tileScript.tIntersectionExits.Count > 0)
          {
              Transform exit = tileScript.tIntersectionExits[Random.Range(0, tileScript.tIntersectionExits.Count)];
              nextSpawnPosition = exit.position;
              nextRotation = exit.rotation;
          }
          else
          {
              nextSpawnPosition = tileScript.exitPoint.position;
              nextRotation = tileScript.exitPoint.rotation;
          }
    }
    void CleanupOldTiles()
    {
        Debug.Log($"Cleaning up old tiles. Active tiles count: {activeTiles.Count}");

         while (activeTiles.Count > maxTiles)
         {
             GameObject tileToRemove = activeTiles[0]; // Get the first tile to remove

             if (tileToRemove != null)
             {

                 Destroy(tileToRemove);
                 Debug.Log($"Destroyed tile: {tileToRemove.name}");
             }
             activeTiles.RemoveAt(0); 

             if (tileToRemove == null)
             {
                 Debug.LogWarning("Tile to remove is already null.");
             }
         }
        Debug.Log($"Tile count after cleanup: {activeTiles.Count}");
    }
    public void TriggerLeftTurn()
    {
         if (CurrentTheme.leftTurnTile == null) return;
         CleanupOldTiles();
         GameObject tile = SpawnTile(CurrentTheme.leftTurnTile);
         if (tile != null)
         {
             activeTiles.Add(tile);
             UpdateNextTilePosition(tile);
             print("This is left turn of T tile");
             CleanupOldTiles();
         }

    }
    public void TriggerRightTurn()
    {
         if (CurrentTheme.rightTurnTile == null) return;
         CleanupOldTiles();
         GameObject tile = SpawnTile(CurrentTheme.rightTurnTile);
         if (tile != null)
         {
             activeTiles.Add(tile);
             UpdateNextTilePosition(tile);
             CleanupOldTiles();
         }
    }
    }

}
*/
using System.Collections.Generic;
using UnityEngine;

namespace Chakwa
{
    public class Path : MonoBehaviour
    {
        public List<TileTheme> themes;
        private int currentThemeIndex = 0;
        private TileTheme CurrentTheme => themes[currentThemeIndex];

        private Vector3 nextSpawnPosition = Vector3.zero;
        private Quaternion nextRotation = Quaternion.identity;

        private List<GameObject> activeTiles = new List<GameObject>();
        public int maxTiles = 10;

        public static Path Instance;
        public GameObject player;

        public List<GameObject> obstaclePrefabs;
        private List<GameObject> activeObstacles = new List<GameObject>();
        public int maxObstacles = 5;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
        }
        void Start()
        {
            SpawnInitialTile();
        }
        void SpawnInitialTile()
        {
            if (themes.Count == 0 || themes[0].straightTile == null)
            {
                Debug.LogWarning("No initial tile found in first theme.");
                return;
            }

            GameObject tile = Instantiate(themes[0].straightTile, nextSpawnPosition, nextRotation);
            Tile tileScript = tile.GetComponent<Tile>();
            if (tileScript != null && tileScript.entryPoint != null && player != null)
            {
                player.transform.position = tileScript.entryPoint.position;
                player.transform.rotation = tileScript.entryPoint.rotation;
            }
            UpdateNextTilePosition(tile);
            activeTiles.Add(tile);
            themes[0].currentTileIndex = 1;
        }
        public void SpawnNextTile()
        {
            if (themes.Count == 0) return;

            if (CurrentTheme.IsFinished())
            {
                currentThemeIndex = (currentThemeIndex + 1) % themes.Count;
                CurrentTheme.Reset();
                Debug.Log(" Switched to Theme : " + currentThemeIndex);
            }

            GameObject prefab = CurrentTheme.GetNextTile();
            if (prefab == null)
            {
                Debug.LogWarning("Missing prefab in current theme.");
                return;
            }
            CleanupOldTiles();
            bool isT = prefab.GetComponent<Tile>()?.tileType == TileType.TIntersection;

            GameObject tile = SpawnTile(prefab, isT);

            if (tile != null)
                activeTiles.Add(tile);

            //  Spawn an obstacle near the new tile 
            SpawnObstacleNearNextTile();
        }
        GameObject SpawnTile(GameObject prefab, bool isTIntersection = false)
        {
            GameObject tile = Instantiate(prefab);
            Tile tileScript = tile.GetComponent<Tile>();

            if (tileScript == null || tileScript.entryPoint == null)
            {
                Debug.LogWarning("Tile missing entry point.");
                Destroy(tile);
                return null;
            }
            tile.transform.position = Vector3.zero;
            tile.transform.rotation = Quaternion.identity;


            Quaternion alignRotation = nextRotation * Quaternion.Inverse(tileScript.entryPoint.rotation);
            tile.transform.rotation = alignRotation * tile.transform.rotation;


            Vector3 offset = nextSpawnPosition - tileScript.entryPoint.position;
            tile.transform.position += offset;


            if (isTIntersection && tileScript.tileType == TileType.TIntersection && tileScript.tIntersectionExits.Count > 0)
            {
                Transform chosenExit = tileScript.tIntersectionExits[Random.Range(0, tileScript.tIntersectionExits.Count)];
                nextSpawnPosition = chosenExit.position;
                nextRotation = chosenExit.rotation;
            }
            else if (tileScript.exitPoint != null)
            {
                nextSpawnPosition = tileScript.exitPoint.position;
                nextRotation = tileScript.exitPoint.rotation;
            }
            return tile;
        }

        void UpdateNextTilePosition(GameObject tile)
        {
            Tile tileScript = tile.GetComponent<Tile>();
            if (tileScript.tileType == TileType.TIntersection && tileScript.tIntersectionExits.Count > 0)
            {
                Transform exit = tileScript.tIntersectionExits[Random.Range(0, tileScript.tIntersectionExits.Count)];
                nextSpawnPosition = exit.position;
                nextRotation = exit.rotation;
            }
            else
            {
                nextSpawnPosition = tileScript.exitPoint.position;
                nextRotation = tileScript.exitPoint.rotation;
            }
        }
        void CleanupOldTiles()
        {
            Debug.Log($"Cleaning up old tiles. Active tiles count: {activeTiles.Count}");

            while (activeTiles.Count > maxTiles)
            {
                GameObject tileToRemove = activeTiles[0]; // Get the first tile to remove

                if (tileToRemove != null)
                {

                    Destroy(tileToRemove);
                    Debug.Log($"Destroyed tile: {tileToRemove.name}");
                }
                activeTiles.RemoveAt(0);

                if (tileToRemove == null)
                {
                    Debug.LogWarning("Tile to remove is already null.");
                }
            }

            Debug.Log($"Tile count after cleanup: {activeTiles.Count}");
        }
        /* public void TriggerLeftTurn()
         {
              if (CurrentTheme.leftTurnTile == null) return;
              CleanupOldTiles();
              GameObject tile = SpawnTile(CurrentTheme.leftTurnTile);
              if (tile != null)
              {
                  activeTiles.Add(tile);
                  UpdateNextTilePosition(tile);
                  print("This is left turn of T tile");
                  CleanupOldTiles();
              }

         }*/
        public void TriggerLeftTurn()
        {
            if (CurrentTheme.leftTurnTile == null) return;

            CleanupOldTiles();

            // Check if it's actually a TIntersection
            bool isTIntersection = CurrentTheme.leftTurnTile.GetComponent<Tile>().tileType == TileType.TIntersection;

            GameObject tile = SpawnTile(CurrentTheme.leftTurnTile, isTIntersection);

            if (tile != null)
            {
                activeTiles.Add(tile);
                UpdateNextTilePosition(tile);
                Debug.Log("This is LEFT turn of T tile");
            }
        }


        /*public void TriggerRightTurn()
        {
         if (CurrentTheme.rightTurnTile == null) return;
         CleanupOldTiles();
         GameObject tile = SpawnTile(CurrentTheme.rightTurnTile);
         if (tile != null)
         {
             activeTiles.Add(tile);
             UpdateNextTilePosition(tile);
             CleanupOldTiles();
         }*/

        public void TriggerRightTurn()
        {
            if (CurrentTheme.rightTurnTile == null) return;

            CleanupOldTiles();

            bool isTIntersection = CurrentTheme.rightTurnTile.GetComponent<Tile>().tileType == TileType.TIntersection;

            GameObject tile = SpawnTile(CurrentTheme.rightTurnTile, isTIntersection);

            if (tile != null)
            {
                activeTiles.Add(tile);
                UpdateNextTilePosition(tile);
                Debug.Log("This is RIGHT turn of T tile");
            }
        }




        public GameObject GetRandomTilePrefab()
        {
            List<GameObject> possibleTiles = new List<GameObject>();

            if (CurrentTheme.straightTile != null)
                possibleTiles.Add(CurrentTheme.straightTile);
            if (CurrentTheme.leftTurnTile != null)
                possibleTiles.Add(CurrentTheme.leftTurnTile);
            if (CurrentTheme.rightTurnTile != null)
                possibleTiles.Add(CurrentTheme.rightTurnTile);

            // Optionally add other tiles you want to consider in your theme
            // For example: special tiles, TIntersection tiles, etc.

            if (possibleTiles.Count == 0)
                return null;

            int randomIndex = Random.Range(0, possibleTiles.Count);
            return possibleTiles[randomIndex];
        }
        public void SpawnRandomTile()
        {
            if (themes.Count == 0) return;

            GameObject randomPrefab = GetRandomTilePrefab();

            if (randomPrefab == null)
            {
                Debug.LogWarning("No tile prefabs available for random spawn.");
                return;
            }

            CleanupOldTiles();

            GameObject tile = SpawnTile(randomPrefab);

            if (tile != null)
            {
                activeTiles.Add(tile);
                UpdateNextTilePosition(tile);
            }
        }
        //SpwanObstracles
        public void SpawnObstacle(Vector3 position, Quaternion rotation)
        {
            if (obstaclePrefabs == null || obstaclePrefabs.Count == 0)
            {
                Debug.LogWarning("No obstacle prefabs assigned.");
                return;
            }

            GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];
            GameObject obstacle = Instantiate(obstaclePrefab, position, rotation);
            activeObstacles.Add(obstacle);
            print("Spwan obstracles");
            CleanupOldObstacles();
        }
        void CleanupOldObstacles()
        {
            while (activeObstacles.Count > maxObstacles)
            {
                GameObject oldObstacle = activeObstacles[0];
                if (oldObstacle != null)
                    Destroy(oldObstacle);
                activeObstacles.RemoveAt(0);
            }
        }
        public void SpawnObstacleNearNextTile()
        {

            Vector3 horizontalOffset = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
            float obstacleHeightOffset = 1f;
            Vector3 spawnPos = nextSpawnPosition + horizontalOffset;
            spawnPos.y += obstacleHeightOffset;  // Raise obstacle above the tile
            Quaternion spawnRot = Quaternion.identity;
            SpawnObstacle(spawnPos, spawnRot);
        }

    }

}

