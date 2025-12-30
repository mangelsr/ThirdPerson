using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public Targeter Targeter { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public Attack[] Attacks { get; private set; }
    [field: SerializeField] public WeaponDamage Weapon { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public Ragdoll Ragdoll { get; private set; }
    [field: SerializeField] public LedgeDetector LedgeDetector { get; private set; }
    [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }
    [field: SerializeField] public float TargetingMovementSpeed { get; private set; }
    [field: SerializeField] public float RotationDamping { get; private set; }
    [field: SerializeField] public float DodgeDuration { get; private set; }
    [field: SerializeField] public float DodgeDistance { get; private set; }
    [field: SerializeField] public float JumpForce { get; private set; }
    [field: SerializeField] public float ParryWindowTime { get; private set; }
    [field: SerializeField] public float ParryKnockback { get; private set; }

    public float dodgeSpeed => DodgeDistance / DodgeDuration;

    public float PreviousDodgeTime { get; private set; } = Mathf.NegativeInfinity;


    public Transform MainCameraTransform { get; private set; }

    public PlayerAttackingState AttackingState { get; private set; }
    public PlayerBlockingState BlockingState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }
    public PlayerDodgingState DodgingState { get; private set; }
    public PlayerFallingState FallingState { get; private set; }
    public PlayerFreeLookState FreeLookState { get; private set; }
    public PlayerHangingState HangingState { get; private set; }
    public PlayerImpactState ImpactState { get; private set; }
    public PlayerJumpingState JumpingState { get; private set; }
    public PlayerParryState ParryState { get; private set; }
    public PlayerPullUpState PullUpState { get; private set; }
    public PlayerTargetingState TargetingState { get; private set; }

    private void Awake()
    {
        AttackingState = new PlayerAttackingState(this);
        BlockingState = new PlayerBlockingState(this);
        DeadState = new PlayerDeadState(this);
        DodgingState = new PlayerDodgingState(this);
        FallingState = new PlayerFallingState(this);
        FreeLookState = new PlayerFreeLookState(this);
        HangingState = new PlayerHangingState(this);
        ImpactState = new PlayerImpactState(this);
        JumpingState = new PlayerJumpingState(this);
        ParryState = new PlayerParryState(this);
        PullUpState = new PlayerPullUpState(this);
        TargetingState = new PlayerTargetingState(this);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        MainCameraTransform = Camera.main.transform;
        SwitchState(FreeLookState);
    }

    private void OnEnable()
    {
        Health.OnTakeDamage += HandleTakeDamage;
        Health.OnDie += HandleDie;
    }

    private void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;
        Health.OnDie -= HandleDie;
    }

    private void HandleTakeDamage()
    {
        SwitchState(ImpactState);
    }

    private void HandleDie()
    {
        SwitchState(DeadState);
    }
}
