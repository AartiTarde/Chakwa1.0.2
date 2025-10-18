/*using UnityEngine;

public class RabbitFollowDotsState : MonoBehaviour, IState
{
    private RabbitController _rabbit;

    public RabbitFollowDotsState(RabbitController rabbit) => _rabbit = rabbit;

    public void Enter()
    {
        _rabbit.PlayRunAnimation();
    }

    public void Tick()
    {
       // _rabbit.MoveTowardsCurrentWaypoint();
    }

    public void Exit()
    {
        Debug.Log("Rabbit finished path.");
    }
}
     */
using UnityEngine;
public class RabbitFollowDotsState : IState
{
    private RabbitController _rabbit;

    public RabbitFollowDotsState(RabbitController rabbit)
    {
        _rabbit = rabbit;
        _rabbit.ResetWaypointIndex();
    }

    public void Enter()
    {
        _rabbit.PlayRunAnimation();
    }

    public void Tick()
    {
        _rabbit.MoveTowardsCurrentWaypoint();
    }

    public void Exit()
    {
        Debug.Log("Rabbit finished path.");
    }
}
