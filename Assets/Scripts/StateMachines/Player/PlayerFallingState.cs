using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    private readonly int FallAnimationHash = Animator.StringToHash("Fall");
    private const float CrossFadeDuration = 0.1f;
    private Vector3 momentum;


    public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(FallAnimationHash, CrossFadeDuration);
        momentum = stateMachine.CharacterController.velocity;
        momentum.y = 0;
    }

    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);

        if (stateMachine.CharacterController.isGrounded)
        {
            ReturnToLocomotion();
            return;
        }

        FaceTarget();
    }

    public override void Exit()
    {
    }
}
