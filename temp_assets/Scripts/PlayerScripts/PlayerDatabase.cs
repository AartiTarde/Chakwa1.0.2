
using UnityEngine;

[System.Serializable]
public class PlayerDetails
{
    public string playerName; //player name
    public GameObject player; //player model
    public Sprite playerImage; //player image
}

[CreateAssetMenu(fileName = "PlayerDatabase", menuName = "Database/PlayerDatabase")]
public class PlayerDatabase : ScriptableObject
{
    public PlayerDetails[] playerDetails;
    public int Count => playerDetails?.Length ?? 0;

}
