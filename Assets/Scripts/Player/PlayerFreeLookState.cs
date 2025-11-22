using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private const float AnimatorDampTime = 0.1f;

    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Enter");
        stateMachine.InputReader.TargetEvent += OnTarget;
    }

    public override void Tick(float deltaTime)
    {
        Vector2 movementValue = stateMachine.InputReader.MovementValue;

        if (movementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }

        Vector3 movement = CalculateMovement();

        stateMachine.CharacterController.Move(movement * deltaTime * stateMachine.FreeLookMovementSpeed);

        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);
        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        Debug.Log("Exit");
        stateMachine.InputReader.TargetEvent -= OnTarget;
    }

    private Vector3 CalculateMovement()
    {
        Vector2 movement = stateMachine.InputReader.MovementValue;

        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return (forward * movement.y) + (right * movement.x);
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * stateMachine.RotationDamping
        );
    }

    private void OnTarget()
    {
        if (stateMachine.Targeter.CanSelectTarget())
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
    }
}
