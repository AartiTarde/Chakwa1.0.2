using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ChakwaCollectibles/Collectibles")]
public class CollectiblesDatabase : ScriptableObject
{
    public string collectName;
    public GameObject prefab;   
    public float weight = 1f;  
    public int value = 1;
}
