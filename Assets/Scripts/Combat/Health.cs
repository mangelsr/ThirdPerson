using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int health;
    private bool isInvulnerable;
    public bool isDead => health == 0;

    private void Start()
    {
        health = maxHealth;
    }

    public void DealDamage(int damage, GameObject attacker)
    {
        if (health == 0) return;

        if (isInvulnerable)
        {
            EventBus<EntityInvulnerableHitEvent>.Raise(new EntityInvulnerableHitEvent(gameObject, attacker));
            return;
        }

        health = Mathf.Max(health - damage, 0);

        EventBus<EntityDamagedEvent>.Raise(new EntityDamagedEvent(gameObject, damage, attacker));

        if (health == 0)
            EventBus<EntityDiedEvent>.Raise(new EntityDiedEvent(gameObject));

        Debug.Log($"Health: {health}");
    }

    public void SetInvulnerable(bool isInvulnerable)
    {
        this.isInvulnerable = isInvulnerable;
    }
}