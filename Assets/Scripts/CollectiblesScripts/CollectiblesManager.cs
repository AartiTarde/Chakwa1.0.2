using System.Collections;
using System.Collections.Generic;   // 👈 this is required for List<>
using UnityEngine;
public class CollectiblesManager : MonoBehaviour
{
    [Range(0f, 1f)]
    public float spawnChance = 0.01f; // 1% per tile
    [Header("Collectibles List (Scriptable Objects)")]
    public List<CollectiblesDatabase> collectibles;

    public void TrySpawnCollectibles(Tile tile)
    {
        if (tile.collectSpot == null || tile.collectSpot.Length == 0) return;

        foreach (GameObject spot in tile.collectSpot)
        {
            if (spot == null) continue;

            // Pick collectible using weights
            CollectiblesDatabase selected = GetRandomCollectible();

            if (selected != null && selected.prefab != null)
            {
                //GameObject obj = Instantiate(selected.prefab, spot.transform.position, Quaternion.identity, tile.transform);
                GameObject obj = Instantiate(selected.prefab,spot.transform.position,selected.prefab.transform.rotation, tile.transform);

                print("Collectoible object Instatiate" + obj);
                var col = obj.GetComponent<Collectible>();
                if (col != null)
                {
                    col.collectibleTag = selected.collectName;
                    col.value = selected.value;
                }
            }
        }
    }


    CollectiblesDatabase GetRandomCollectible()
    {
        float totalWeight = 0f;
        foreach (var c in collectibles) totalWeight += c.weight;

        float randomValue = Random.value * totalWeight;
        float current = 0f;

        foreach (var c in collectibles)
        {
            current += c.weight;
            if (randomValue <= current) return c;
        }
        return null;
    }
}
