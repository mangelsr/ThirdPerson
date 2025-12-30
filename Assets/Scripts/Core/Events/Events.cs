using UnityEngine;

public struct EntityDamagedEvent : IEvent
{
    public GameObject Target;
    public int DamageAmount;
    public GameObject Attacker;

    public EntityDamagedEvent(GameObject target, int damageAmount, GameObject attacker)
    {
        Target = target;
        DamageAmount = damageAmount;
        Attacker = attacker;
    }
}

public struct EntityDiedEvent : IEvent
{
    public GameObject Target;

    public EntityDiedEvent(GameObject target)
    {
        Target = target;
    }
}

public struct EntityInvulnerableHitEvent : IEvent
{
    public GameObject Target;
    public GameObject Attacker;

    public EntityInvulnerableHitEvent(GameObject target, GameObject attacker)
    {
        Target = target;
        Attacker = attacker;
    }
}
