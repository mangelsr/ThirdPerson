using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private Attack attack;
    private float previousFrameTime;
    private bool alreadyAppliedForce = false;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        stateMachine.WeaponDamage.SetAttack(attack.Damage);
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        FaceTarget();

        float normalizedTime = GetNormalizedTime();

        if (normalizedTime < 1f)
        {
            if (normalizedTime >= attack.ForceTime)
                TryApplyForce();
            
            if (stateMachine.InputReader.IsAttacking)
                TryComboAttack(normalizedTime);
        }
        else
        {
            PlayerBaseState newState;

            if (stateMachine.Targeter.CurrentTarget == null)
                newState = new PlayerFreeLookState(stateMachine);
            else
                newState = new PlayerTargetingState(stateMachine);

            stateMachine.SwitchState(newState);
        }

        previousFrameTime = normalizedTime;
    }

    public override void Exit()
    {
    }

    private float GetNormalizedTime()
    {
        AnimatorStateInfo currentInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = stateMachine.Animator.GetNextAnimatorStateInfo(0);

        if (stateMachine.Animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
        {
            return nextInfo.normalizedTime;
        }
        else if (!stateMachine.Animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            return currentInfo.normalizedTime;
        }

        return 0f;
    }

    private void TryComboAttack(float normalizedTime)
    {
        if (attack.ComboStateIndex == -1) return;

        if (normalizedTime < attack.ComboAttackTime) return;

        stateMachine.SwitchState(
            new PlayerAttackingState(stateMachine, attack.ComboStateIndex)
        );
    }

    private void TryApplyForce()
    {
        if (alreadyAppliedForce) return;

        stateMachine.ForceReceiver.AddForce(
            stateMachine.transform.forward * attack.Force
        );
        alreadyAppliedForce = true;
    }
}
