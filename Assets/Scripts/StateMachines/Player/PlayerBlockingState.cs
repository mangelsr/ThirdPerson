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
        stateMachine.Health.OnTakeDamageWhileInvulnerable += HandleParry;
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
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            return;
        }

        if (stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);
        stateMachine.Health.OnTakeDamageWhileInvulnerable -= HandleParry;
    }

    private void HandleParry(GameObject attacker)
    {
        if (remainingParryWindow > 0)
        {
            stateMachine.SwitchState(new PlayerParryState(stateMachine, attacker));
        }
    }
}
