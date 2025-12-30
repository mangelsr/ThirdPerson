using UnityEngine;

public class PlayerHangingState : PlayerBaseState
{
    private readonly int HangingAnimationHash = Animator.StringToHash("Hanging");
    private const float CrossFadeDuration = 0.1f;

    private Vector3 ledgeForward;
    private Vector3 closestPoint;

    public PlayerHangingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public void Init(Vector3 ledgeForward, Vector3 closestPoint)
    {
        this.ledgeForward = ledgeForward;
        this.closestPoint = closestPoint;
    }

    public override void Enter()
    {
        stateMachine.transform.rotation = Quaternion.LookRotation(ledgeForward, Vector3.up);

        Vector3 handsPosition = stateMachine.LedgeDetector.transform.position;
        Vector3 playerPosition = stateMachine.transform.position;

        stateMachine.CharacterController.enabled = false;
        stateMachine.transform.position = closestPoint - (handsPosition - playerPosition);
        stateMachine.CharacterController.enabled = true;

        stateMachine.Animator.CrossFadeInFixedTime(HangingAnimationHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Vector2 movement = stateMachine.InputReader.MovementValue;
        if (movement.y > 0)
        {
            stateMachine.SwitchState(stateMachine.PullUpState);
        }
        else if (movement.y < 0)
        {
            stateMachine.CharacterController.Move(Vector3.zero);
            stateMachine.ForceReceiver.Reset();
            stateMachine.SwitchState(stateMachine.FallingState);
            return;
        }
    }

    public override void Exit()
    {
    }

}
