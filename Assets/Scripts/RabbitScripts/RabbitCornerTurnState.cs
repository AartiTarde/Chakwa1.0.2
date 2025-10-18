using UnityEngine;

public class RabbitCornerTurnState : MonoBehaviour, IState
{
    private RabbitController _rabbit;

    public RabbitCornerTurnState(RabbitController rabbit) => _rabbit = rabbit;

    public void Enter() => _rabbit.PlayTurnAnimation();

    public void Tick()
    {
        bool finishedTurning = _rabbit.TurnTowardsNextPathPoint(); // update needed
        if (finishedTurning)
        {
            _rabbit.ChangeState(new RabbitDisappearState(_rabbit));
        }
    }

    public void Exit() => Debug.Log("Rabbit finished turning.");
}
