using UnityEngine;

public class PlayerTestState : PlayerBaseState
{

    public PlayerTestState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Enter");
    }

    public override void Tick(float deltaTime)
    {
        Vector2 movementValue = stateMachine.InputReader.MovementValue;

        if (movementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat("FreeLookSpeed", 0, 0.1f, deltaTime);
            return;
        }

        Vector3 movement = new Vector3();
        movement.x = movementValue.x;
        movement.y = 0;
        movement.z = movementValue.y;

        stateMachine.CharacterController.Move(movement * deltaTime * stateMachine.FreeLookMovementSpeed);

        stateMachine.Animator.SetFloat("FreeLookSpeed", 1, 0.1f, deltaTime);
        stateMachine.transform.rotation = Quaternion.LookRotation(movement);
    }

    public override void Exit()
    {
        Debug.Log("Exit");
    }

}
