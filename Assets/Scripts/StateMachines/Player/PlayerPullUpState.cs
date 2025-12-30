using UnityEngine;

public class PlayerPullUpState : PlayerBaseState
{
    private readonly int PullUpAnimationHash = Animator.StringToHash("PullUp");
    private const float CrossFadeDuration = 0.1f;
    private readonly Vector3 Offset = new Vector3(0f, 2.325f, 0.65f);
    public PlayerPullUpState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(PullUpAnimationHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Climbing");
        if (normalizedTime < 1f) return;

        stateMachine.CharacterController.enabled = false;
        stateMachine.transform.Translate(Offset, Space.Self);
        stateMachine.CharacterController.enabled = true;

        stateMachine.FreeLookState.Init(false);
        stateMachine.SwitchState(stateMachine.FreeLookState);
    }


    public override void Exit()
    {
        stateMachine.CharacterController.Move(Vector3.zero);
        stateMachine.ForceReceiver.Reset();
    }
}
