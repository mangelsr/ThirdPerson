using UnityEngine;

public class PlayerTestState : PlayerBaseState
{
    private float timer = 5f;

    public PlayerTestState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Enter");
    }

    public override void Tick(float deltaTime)
    {
        timer -= deltaTime;
        Debug.Log("Tick");
        // Debug.Log($"Remaining time: {timer}");

        if (timer <= 0)
        {
            stateMachine.SwitchState(new PlayerTestState(stateMachine));
        }
    }

    public override void Exit()
    {
        Debug.Log("Exit");
    }
}
