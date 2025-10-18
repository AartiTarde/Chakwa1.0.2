using UnityEngine;

public class RabbitRunForwardState : MonoBehaviour, IState
{
    private RabbitController _rabbit;

    public RabbitRunForwardState(RabbitController rabbit) => _rabbit = rabbit;

    public void Enter() => _rabbit.PlayRunAnimation();
    public void Tick() => _rabbit.MoveForward();
    public void Exit() => Debug.Log("Rabbit stops running.");
}
