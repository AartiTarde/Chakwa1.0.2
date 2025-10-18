/*using System.Collections.Generic;
using UnityEngine;

namespace Chakwa
{
    public class TileSpawner : MonoBehaviour
    {
        [Header("Tile Settings")]
        [SerializeField]
        private int tileStartCount = 10;   

        [SerializeField]
        private int minimumStraightTiles = 3;  
        [SerializeField]
        private int maximumStraightTiles = 15;  

        [SerializeField]
        private GameObject startingTile;    
        [SerializeField]
        private List<GameObject> turnTiles;  
        [SerializeField]
        private List<GameObject> obstacles; 

        
        private Vector3 currentTileLocation = Vector3.zero;
        private Vector3 currentTileDirection = Vector3.forward;

        private GameObject prevTile;  
        private List<GameObject> currentTiles;  
        private List<GameObject> currentObstacles;
        private Quaternion nextRotation = Quaternion.identity;


        private void Start()
        {

           currentTiles = new List<GameObject>();
            currentObstacles = new List<GameObject>();

            Random.InitState(System.DateTime.Now.Millisecond);

           
            for (int i = 0; i < tileStartCount; ++i)
            {
                SpawnTile(startingTile.GetComponent<Tile>());
                if (i == 0)
                {
                    SpawnTile(startingTile.GetComponent<Tile>());
                }
                else
                {
                    
                    SpawnTile(SelectRandomGameObjectFromList(turnTiles).GetComponent<Tile>());
                }
            }
        }
        /*private void SpawnTile(Tile tile, bool spawnObstacle = false)
          {


            Quaternion newTileRotation = tile.gameObject.transform.rotation * Quaternion.LookRotation(currentTileDirection, Vector3.up);
            prevTile = GameObject.Instantiate(tile.gameObject, currentTileLocation, newTileRotation);
            currentTiles.Add(prevTile);

              if (spawnObstacle) SpawnObstacle();

              if (tile.tileType== TileType.Straight)
              {
                  currentTileLocation += Vector3.Scale(prevTile.GetComponent<Renderer>().bounds.size, currentTileDirection);
              }
              else
              {
                  currentTileDirection = prevTile.transform.forward;
                  currentTileLocation += Vector3.Scale(prevTile.GetComponent<Renderer>().bounds.size, currentTileDirection);
              }
         }*//*
        private void SpawnTile(Tile tile, bool spawnObstacle = false)
        {
            Quaternion newTileRotation = tile.gameObject.transform.rotation * Quaternion.LookRotation(currentTileDirection, Vector3.up);
            prevTile = GameObject.Instantiate(tile.gameObject, currentTileLocation, newTileRotation);
            currentTiles.Add(prevTile);

            if (spawnObstacle) SpawnObstacle();
            if (tile.tileType == TileType.Straight)
            {
                currentTileLocation += Vector3.Scale(prevTile.GetComponent<Renderer>().bounds.size, currentTileDirection);
            }
            else
            {
                currentTileDirection = prevTile.transform.forward;
                currentTileLocation += Vector3.Scale(prevTile.GetComponent<Renderer>().bounds.size, currentTileDirection);
            }
            Debug.Log($"Spawned tile at {currentTileLocation}");
        }
        private void DeletePreviousTiles()
        {
           
            while (currentTiles.Count != 1)
            {
                GameObject tile = currentTiles[0];
                currentTiles.RemoveAt(0);
                Destroy(tile);
            }
            while (currentTiles.Count != 1)
            {
                GameObject obstacle = currentObstacles[0];
                currentObstacles.RemoveAt(0);
                Destroy(obstacle);
            }
        }
        public void AddNewDirection(Vector3 direction)
        {
            currentTileDirection = direction;
            DeletePreviousTiles();

          
            Vector3 tilePlacementScale;

            if (prevTile.GetComponent<Tile>().tileType == TileType.DoubleTurn)
            {
                tilePlacementScale = Vector3.Scale(prevTile.GetComponent<Renderer>().bounds.size * 2 + (Vector3.one * startingTile.GetComponent<BoxCollider>().size.z / 2), currentTileDirection);
            }
            else
            {
                tilePlacementScale = Vector3.Scale(prevTile.GetComponent<Renderer>().bounds.size * 2 + (Vector3.one * startingTile.GetComponent<BoxCollider>().size.z / 2), currentTileDirection);
            }

            currentTileLocation += tilePlacementScale;

            int currentPathLength = Random.Range(minimumStraightTiles, maximumStraightTiles);

            for (int i = 0; i < currentPathLength; ++i)
            {
                SpawnTile(startingTile.GetComponent<Tile>(), (i == 0) ? false : true);
            }
            SpawnTile(SelectRandomGameObjectFromList(turnTiles).GetComponent<Tile>(), false);
        }
        private void SpawnObstacle()
        {
            if (Random.value > 0.2f) return;
            GameObject obstaclePrefab = SelectRandomGameObjectFromList(obstacles);
            Quaternion newObjectRotation = Quaternion.LookRotation(currentTileDirection, Vector3.up);
            GameObject obstacle = Instantiate(obstaclePrefab, currentTileLocation, newObjectRotation);  
            currentObstacles.Add(obstacle);
        }
        private GameObject SelectRandomGameObjectFromList(List<GameObject> list)
        {
            if (list.Count == 0) return null;
            return list[Random.Range(0, list.Count)];
        }
    }
}


*/