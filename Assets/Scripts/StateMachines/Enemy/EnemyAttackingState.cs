using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private readonly int AttackAnimationHash = Animator.StringToHash("Attack");
    private const float CrossFadeDuration = 0.1f;

    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Weapon.SetAttack(stateMachine.AttackDamage, stateMachine.AttackKnockback);
        stateMachine.Animator.CrossFadeInFixedTime(AttackAnimationHash, CrossFadeDuration);
    }


    public override void Tick(float deltaTime)
    {
        float normalizedTime = GetNormalizedTime(stateMachine.Animator);

        if (normalizedTime >= 1f)
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
    }

    public override void Exit()
    {
    }



}
