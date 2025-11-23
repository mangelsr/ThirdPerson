using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");

    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.InputReader.CancelTargetEvent += OnCancelTarget;
        stateMachine.Animator.Play(TargetingBlendTreeHash);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement();
        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);
        FaceTarget();
    }

    public override void Exit()
    {
        stateMachine.InputReader.CancelTargetEvent -= OnCancelTarget;
    }

    private void OnCancelTarget()
    {
        stateMachine.Targeter.Cancel();
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    private Vector3 CalculateMovement()
    {
        Vector2 inputMovement = stateMachine.InputReader.MovementValue;

        Vector3 movement = new Vector3();
        movement += stateMachine.transform.right * inputMovement.x;
        movement += stateMachine.transform.forward * inputMovement.y;

        return movement;
    }
}
