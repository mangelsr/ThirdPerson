using UnityEngine;

public class EnemyImpactState : EnemyBaseState
{
    private readonly int ImpactAnimationHash = Animator.StringToHash("Impact");
    private const float CrossFadeDuration = 0.1f;
    private float duration = 1f;

    public EnemyImpactState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ImpactAnimationHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        duration -= deltaTime;

        if (duration <= 0f)
        {
            stateMachine.SwitchState(stateMachine.IdleState);
            return;
        }
    }

    public override void Exit()
    {
    }
}
