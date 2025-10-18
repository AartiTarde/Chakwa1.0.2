using UnityEngine;

[System.Serializable]
public class TileTheme
{
    public GameObject straightTile;
    public GameObject leftTurnTile;
    public GameObject rightTurnTile;
    public GameObject tIntersectionTile;

    [HideInInspector] public int currentTileIndex = 0;

    private GameObject[] tiles;

    public TileTheme()
    {
         tiles = new GameObject[] { straightTile, leftTurnTile, rightTurnTile, tIntersectionTile };
    }

    public GameObject GetNextTile()
    {
        tiles = new GameObject[] { straightTile, leftTurnTile, rightTurnTile, tIntersectionTile };
        int randomIndex = Random.Range(0, tiles.Length);
        currentTileIndex++;  
        return tiles[randomIndex];
    }

    public bool IsFinished()
    {
        return currentTileIndex >= 4; 
    }

    public void Reset()
    {
        currentTileIndex = 0;
    }
}
