using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("LocomotionBlendTree");
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;


    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        if (IsInChaseRange())
        {
            stateMachine.SwitchState(stateMachine.ChasingState);
            return;
        }

        FacePlayer();

        stateMachine.Animator.SetFloat(SpeedHash, 0.0f, AnimatorDampTime, deltaTime);
    }

    public override void Exit() { }
}
