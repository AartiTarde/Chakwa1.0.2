using UnityEngine;

public enum TurnDirection { Left, Right, Both }

public class TurnPoint : MonoBehaviour
{
    public TurnDirection allowedTurn = TurnDirection.Both;
}
