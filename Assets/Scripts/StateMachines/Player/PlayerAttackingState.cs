using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private Attack attack;
    private bool alreadyAppliedForce = false;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        stateMachine.Weapon.SetAttack(attack.Damage, attack.Knockback);
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        FaceTarget();

        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Attack");

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
    }

    public override void Exit()
    {
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
