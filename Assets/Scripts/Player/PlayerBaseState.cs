using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        Vector3 movement = motion + stateMachine.forceReceiver.Movement;
        stateMachine.CharacterController.Move(movement * deltaTime);
    }

    protected void FaceTarget()
    {
        Target target = stateMachine.Targeter.CurrentTarget;
        if (target == null) return;

        Vector3 lookPosition = target.transform.position - stateMachine.transform.position;
        lookPosition.y = 0;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPosition);
    }
}
