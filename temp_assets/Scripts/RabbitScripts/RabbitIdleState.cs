using UnityEngine;

public class RabbitIdleState : MonoBehaviour, IState
{
    private RabbitController _rabbit;

    public RabbitIdleState(RabbitController rabbit) => _rabbit = rabbit;

    public void Enter() => _rabbit.PlayIdleAnimation();
    public void Tick() => _rabbit.CheckForRunTrigger();  // e.g., timer or sound
    public void Exit() => Debug.Log("Rabbit leaves idle.");
}
