using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    private readonly int JumpAnimationHash = Animator.StringToHash("Jump");
    private const float CrossFadeDuration = 0.1f;
    private Vector3 momentum;

    public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(JumpAnimationHash, CrossFadeDuration);
        stateMachine.LedgeDetector.OnLedgeDetected += HandleLedgeDetect;

        if (stateMachine.InputReader.MovementValue != Vector2.zero)
        {
            momentum = stateMachine.CharacterController.velocity;
            momentum.y = 0;
        }

        stateMachine.ForceReceiver.Jump(stateMachine.JumpForce);
    }

    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);

        if (stateMachine.CharacterController.velocity.y <= 0)
        {
            stateMachine.SwitchState(stateMachine.FallingState);
            return;
        }

        FaceTarget();
    }

    public override void Exit()
    {
        stateMachine.LedgeDetector.OnLedgeDetected -= HandleLedgeDetect;
    }

    private void HandleLedgeDetect(Vector3 ledgeForward, Vector3 closestPoint)
    {
        stateMachine.HangingState.Init(ledgeForward, closestPoint);
        stateMachine.SwitchState(stateMachine.HangingState);
    }
}
