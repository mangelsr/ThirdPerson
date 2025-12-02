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

        momentum = stateMachine.CharacterController.velocity;
        momentum.y = 0;

        stateMachine.ForceReceiver.Jump(stateMachine.JumpForce);
    }

    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);

        if (stateMachine.CharacterController.velocity.y <= 0)
        {
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));
            return;
        }

        FaceTarget();
    }

    public override void Exit()
    {
    }
}
