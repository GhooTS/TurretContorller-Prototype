using UnityEngine;

public class HealthReference : MonoBehaviour,IDamageable
{
    public Health reference;

    public void TakeDamage(float damage)
    {
        reference.TakeDamage(damage);
    }
}
