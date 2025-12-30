using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }
    [field: SerializeField] public WeaponDamage Weapon { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public Target Target { get; private set; }
    [field: SerializeField] public Ragdoll Ragdoll { get; private set; }
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float PlayerChasingRange { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public int AttackDamage { get; private set; }
    [field: SerializeField] public float AttackKnockback { get; private set; }


    public Health Player { get; private set; }

    public EnemyAttackingState AttackingState { get; private set; }
    public EnemyChasingState ChasingState { get; private set; }
    public EnemyDeadState DeadState { get; private set; }
    public EnemyIdleState IdleState { get; private set; }
    public EnemyImpactState ImpactState { get; private set; }

    private void Awake()
    {
        AttackingState = new EnemyAttackingState(this);
        ChasingState = new EnemyChasingState(this);
        DeadState = new EnemyDeadState(this);
        IdleState = new EnemyIdleState(this);
        ImpactState = new EnemyImpactState(this);
    }

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

        Agent.updatePosition = false;
        Agent.updateRotation = false;

        SwitchState(IdleState);
    }

    private void OnEnable()
    {
        EventBus<EntityDamagedEvent>.Register(OnDamage);
        EventBus<EntityDiedEvent>.Register(OnDie);

    }

    private void OnDisable()
    {
        EventBus<EntityDamagedEvent>.Deregister(OnDamage);
        EventBus<EntityDiedEvent>.Deregister(OnDie);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, PlayerChasingRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }

    private void OnDamage(EntityDamagedEvent @event)
    {
        if (@event.Target == gameObject)
            SwitchState(ImpactState);
    }

    private void OnDie(EntityDiedEvent @event)
    {
        if (@event.Target == gameObject)
            SwitchState(DeadState);
    }
}
