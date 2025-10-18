/*using UnityEngine;

[System.Serializable]
public class TileTheme
{
    public GameObject straightTile;
    public GameObject leftTurnTile;
    public GameObject rightTurnTile;
    public GameObject tIntersectionTile;

    [HideInInspector] public int currentTileIndex = 0;

    public GameObject GetNextTile()
    {
        GameObject tile = null;
        switch (currentTileIndex)
        {
            case 0: tile = straightTile; break;
            case 1: tile = leftTurnTile; break;
            case 2: tile = rightTurnTile; break;
            case 3: tile = tIntersectionTile; break;
        }
        currentTileIndex++;
        return tile;
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
*/


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
        currentTileIndex++;  // keep increasing if you want to track how many tiles you've gotten
        return tiles[randomIndex];
    }

    public bool IsFinished()
    {
        return currentTileIndex >= 4;  // or some other logic
    }

    public void Reset()
    {
        currentTileIndex = 0;
    }
}