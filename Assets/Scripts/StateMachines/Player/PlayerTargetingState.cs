using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRightTreeHash = Animator.StringToHash("TargetingRight");
    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;


    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.InputReader.CancelTargetEvent += OnCancelTarget;
        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }

        if (stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }

        if (stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement();
        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);
        UpdateAnimator(deltaTime);
        FaceTarget();
    }

    public override void Exit()
    {
        stateMachine.InputReader.CancelTargetEvent -= OnCancelTarget;
    }

    private void OnCancelTarget()
    {
        stateMachine.Targeter.Cancel();
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    private Vector3 CalculateMovement()
    {
        Vector2 inputMovement = stateMachine.InputReader.MovementValue;

        Vector3 movement = new Vector3();
        movement += stateMachine.transform.right * inputMovement.x;
        movement += stateMachine.transform.forward * inputMovement.y;

        return movement;
    }

    private void UpdateAnimator(float deltaTime)
    {
        Vector2 movement = stateMachine.InputReader.MovementValue;
        stateMachine.Animator.SetFloat(TargetingForwardHash, Mathf.Round(movement.y), AnimatorDampTime, deltaTime);
        stateMachine.Animator.SetFloat(TargetingRightTreeHash, Mathf.Round(movement.x), AnimatorDampTime, deltaTime);
    }
}
