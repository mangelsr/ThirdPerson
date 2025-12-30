using UnityEngine;

public class PlayerParryState : PlayerBaseState
{
    private readonly int ParryAnimationHash = Animator.StringToHash("Parry");
    private const float CrossFadeDuration = 0.1f;
    private readonly GameObject attacker;

    public PlayerParryState(PlayerStateMachine stateMachine, GameObject attacker) : base(stateMachine)
    {
        this.attacker = attacker;
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ParryAnimationHash, CrossFadeDuration);
        Parry();    
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Parry");

        if (normalizedTime >= 1f)
        {
            if (stateMachine.Targeter.CurrentTarget == null)
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
                return;
            }
            else
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
                return;
            }
        }
    }

    public override void Exit()
    {
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
                enemyStateMachine.SwitchState(new EnemyImpactState(enemyStateMachine));
            }
        }
    }
}
