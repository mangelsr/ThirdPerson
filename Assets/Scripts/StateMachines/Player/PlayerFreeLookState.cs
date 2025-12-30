using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");

    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;
    private bool shouldFade;

    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public void Init(bool shouldFade = true)
    {
        this.shouldFade = shouldFade;
    }

    public override void Enter()
    {
        stateMachine.InputReader.TargetEvent += OnTarget;
        stateMachine.InputReader.JumpEvent += OnJump;

        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0);

        if (shouldFade)
            stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);
        else
            stateMachine.Animator.Play(FreeLookBlendTreeHash);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.AttackingState.Init(0);
            stateMachine.SwitchState(stateMachine.AttackingState);
            return;
        }

        Vector2 movementValue = stateMachine.InputReader.MovementValue;

        if (movementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }

        Vector3 movement = CalculateMovement();
        Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);

        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);
        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnTarget;
        stateMachine.InputReader.JumpEvent -= OnJump;
        shouldFade = true;
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
            stateMachine.SwitchState(stateMachine.TargetingState);
    }

    private void OnJump()
    {
        stateMachine.SwitchState(stateMachine.JumpingState);
    }
}
