using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected bool IsInChaseRange()
    {
        Vector3 playerPos = stateMachine.Player.transform.position;
        Vector3 enemyPos = stateMachine.transform.position;
        float distanceSqr = (playerPos - enemyPos).sqrMagnitude;
        return distanceSqr <= Mathf.Pow(stateMachine.PlayerChasingRange, 2);
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        Vector3 movement = motion + stateMachine.ForceReceiver.Movement;
        stateMachine.CharacterController.Move(movement * deltaTime);
    }

    protected void FacePlayer()
    {
        GameObject player = stateMachine.Player;
        if (player == null) return;

        Vector3 lookPosition = player.transform.position - stateMachine.transform.position;
        lookPosition.y = 0;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPosition);
    }

        protected bool IsInAttackRange()
    {
        Vector3 playerPos = stateMachine.Player.transform.position;
        Vector3 enemyPos = stateMachine.transform.position;
        float distanceSqr = (playerPos - enemyPos).sqrMagnitude;
        return distanceSqr <= Mathf.Pow(stateMachine.AttackRange, 2);
    }
}
