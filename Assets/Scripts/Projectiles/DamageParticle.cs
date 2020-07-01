using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(ParticleSystem))]
public class DamageParticle : MonoBehaviour
{
    public float damage;
    private ParticleSystem particle;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent(out IDamageable damagable))
        {
            var amount = ParticlePhysicsExtensions.GetCollisionEvents(particle, other, collisionEvents);
            damagable.TakeDamage(damage * amount);
        }
    }
}
