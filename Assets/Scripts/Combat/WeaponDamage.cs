using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider myCollider;
    private int damage;
    private List<Collider> alreadyCollidedWith = new List<Collider>();

    private void OnEnable()
    {
        alreadyCollidedWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == myCollider) return;

        if (other.TryGetComponent(out Health health))
        {
            if (!alreadyCollidedWith.Contains(other))
            {
                alreadyCollidedWith.Add(other);
                health.DealDamage(damage);
            }
        }
    }

    public void SetAttack(int damage)
    {
        this.damage = damage;
    }
}
