using UnityEngine;

public class RabbitDisappearState : IState
{
    private RabbitController _rabbit;

    public RabbitDisappearState(RabbitController rabbit) => _rabbit = rabbit;

    public void Enter()
    {
        _rabbit.FadeOutOrDespawn(); // Existing method
        _rabbit.MakeInvisible();    // New helper method
    }

    public void Tick() { }

    public void Exit() => Debug.Log("Rabbit is gone.");
}
