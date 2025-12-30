using UnityEngine;

public class PlayerParryState : PlayerBaseState
{
    private readonly int ParryAnimationHash = Animator.StringToHash("Parry");
    private const float CrossFadeDuration = 0.1f;
    private GameObject attacker;
    private bool parryApplied;

    public PlayerParryState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public void SetAttacker(GameObject attacker)
    {
        this.attacker = attacker;
    }

    public void Reset()
    {
        parryApplied = false;
        attacker = null;
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ParryAnimationHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Parry");

        if (normalizedTime > 0.5f && !parryApplied)
            Parry();

        if (normalizedTime >= 1f)
        {
            if (stateMachine.Targeter.CurrentTarget == null)
            {
                stateMachine.SwitchState(stateMachine.FreeLookState);
                return;
            }
            else
            {
                stateMachine.SwitchState(stateMachine.TargetingState);
                return;
            }
        }
    }

    public override void Exit()
    {
        Reset();
    }

    private void Parry()
    {
        if (attacker != null)
        {
            FaceTarget();
            if (attacker.TryGetComponent(out ForceReceiver forceReceiver))
            {
                Vector3 direction = 
                    (attacker.transform.position - stateMachine.gameObject.transform.position)
                    .normalized;
                forceReceiver.AddForce(direction * stateMachine.ParryKnockback);
            }
            if (attacker.TryGetComponent(out EnemyStateMachine enemyStateMachine)) {
                enemyStateMachine.SwitchState(enemyStateMachine.ImpactState);
            }
            parryApplied = true;
        }
    }
}
