using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    private readonly int BlockAnimationHash = Animator.StringToHash("Block");
    private const float CrossFadeDuration = 0.1f;
    private float remainingParryWindow = 0;

    public PlayerBlockingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(BlockAnimationHash, CrossFadeDuration);
        stateMachine.Health.SetInvulnerable(true);
        remainingParryWindow = stateMachine.ParryWindowTime;
        EventBus<EntityInvulnerableHitEvent>.Register(OnInvulnerableHit);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        if (remainingParryWindow > 0)
        {
            remainingParryWindow = Mathf.Max(remainingParryWindow - deltaTime, 0);
        }

        if (!stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(stateMachine.TargetingState);
            return;
        }

        if (stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(stateMachine.FreeLookState);
            return;
        }
    }

    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);
        EventBus<EntityInvulnerableHitEvent>.Deregister(OnInvulnerableHit);
    }

    private void OnInvulnerableHit(EntityInvulnerableHitEvent @event)
    {
        if (@event.Target == stateMachine.gameObject)
            HandleParry(@event.Attacker);
    }

    private void HandleParry(GameObject attacker)
    {
        if (remainingParryWindow > 0)
        {
            stateMachine.ParryState.SetAttacker(attacker);
            stateMachine.SwitchState(stateMachine.ParryState);
        }
    }
}
