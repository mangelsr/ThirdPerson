using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{
    private readonly int DodgingBlendTreeHash = Animator.StringToHash("DodgingBlendTree");
    private readonly int DodgingForwardHash = Animator.StringToHash("DodgingForward");
    private readonly int DodgingRightTreeHash = Animator.StringToHash("DodgingRight");
    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;

    private Vector3 dodgingDirectionInput;
    private float remainingDodgeTime;

    public PlayerDodgingState(PlayerStateMachine stateMachine, Vector3 dodgingDirectionInput) : base(stateMachine)
    {
        this.dodgingDirectionInput = dodgingDirectionInput;
    }

    public override void Enter()
    {
        remainingDodgeTime = stateMachine.DodgeDuration;
        stateMachine.Health.SetInvulnerable(true);

        stateMachine.Animator.SetFloat(DodgingForwardHash, dodgingDirectionInput.y);
        stateMachine.Animator.SetFloat(DodgingRightTreeHash, dodgingDirectionInput.x);
        stateMachine.Animator.CrossFadeInFixedTime(DodgingBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = new Vector3();

        movement += stateMachine.transform.right * dodgingDirectionInput.x * stateMachine.dodgeSpeed;
        movement += stateMachine.transform.forward * dodgingDirectionInput.y * stateMachine.dodgeSpeed;

        Move(movement, deltaTime);

        FaceTarget();

        remainingDodgeTime -= deltaTime;
        if (remainingDodgeTime <= 0f)
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);
    }
}
