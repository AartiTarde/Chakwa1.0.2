/*using Unity.VisualScripting;
using UnityEngine;

public class RabbitController : MonoBehaviour
{

    private StateMachine _stateMachine;
    public float speed =10;
    private void Start()
    {
        _stateMachine = new StateMachine();

       
        _stateMachine.ChangeState(new RabbitIdleState(this));
    }

    private void Update()
    {
        _stateMachine.Tick();
    }
    public void PlayIdleAnimation() {    }
    public void MoveForward() { transform.Translate(Vector3.right *speed* Time.deltaTime); }
    public void PlayRunAnimation() { }
    public void PlayTurnAnimation() {  }
    public void TurnTowardsNextPathPoint() {  }
    public void FadeOutOrDespawn() {  }

    public void CheckForRunTrigger()
    {
        // Example condition
        if (Time.time > 5f)
            _stateMachine.ChangeState(new RabbitRunForwardState(this));
    }
}
*/
using UnityEngine;
using System.Collections.Generic;

public class RabbitController : MonoBehaviour
{
    private StateMachine _stateMachine;

    public float speed = 10f;
    public List<Transform> waypoints;
    private int currentWaypointIndex = 0;

    private void Start()
    {
        _stateMachine = new StateMachine();
        _stateMachine.ChangeState(new RabbitIdleState(this));
    }

    private void Update()
    {
        _stateMachine.Tick();
    }

    // Rabbit actions
    public void PlayIdleAnimation() { Debug.Log("Idle animation"); }
    public void PlayRunAnimation() { Debug.Log("Run animation"); }
    public void PlayTurnAnimation() { Debug.Log("Turn animation"); }
    public void FadeOutOrDespawn() { Debug.Log("Fade out/despawn"); }

    public void MoveForward()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    public void MoveTowardsCurrentWaypoint()
    {
        if (waypoints == null || waypoints.Count == 0) return;

        Transform target = waypoints[currentWaypointIndex];
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Count)
            {
                _stateMachine.ChangeState(new RabbitDisappearState(this)); // or another end state
            }
        }
    }

    public bool TurnTowardsNextPathPoint()
    {
        if (waypoints == null || waypoints.Count == 0 || currentWaypointIndex >= waypoints.Count) return true;

        Vector3 dir = (waypoints[currentWaypointIndex].position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);

        float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);
        return angleDifference < 1f; // Turning is done if angle difference is small
    }

    public void CheckForRunTrigger()
    {
        // Trigger follow state after 5s
        if (Time.time > 1f)
            _stateMachine.ChangeState(new RabbitFollowDotsState(this));
    }

    // Optional reset
    public void ResetWaypointIndex()
    {
        currentWaypointIndex = 0;
    }

    public void MakeInvisible()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }
    }

    public void ChangeState(IState newState)
    {
        _stateMachine.ChangeState(newState);
    }

    public List<Transform> GetWaypoints() => waypoints;
}

