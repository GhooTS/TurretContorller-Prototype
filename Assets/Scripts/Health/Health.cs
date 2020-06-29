using GTVariable;
using UnityEngine;
using UnityEngine.Events;


public class Health : MonoBehaviour
{
    [Tooltip("Whether or not take action when hp hit 0")]
    public bool autoDestroy = true;
    [Tooltip("If set to false game object will be disable")]
    public bool destroyIfDie = true;
    [SerializeField]
    protected FloatReference HP;
    public FloatReference MaxHP;
    public bool Alive { get; private set; }

    public UnityEvent damageTaken;
    public UnityEvent died;

    private void OnEnable()
    {
        HP.Value = MaxHP.Value;
        Alive = true;
    }

    private void OnDisable()
    {
        Alive = false;
    }

    public virtual void DealDamage(float damage)
    {
        HP.Value -= damage;
        damageTaken?.Invoke();
        if (HP.Value <= 0)
        {
            Alive = false;
            died?.Invoke();
            if (autoDestroy)
            {
                Die();
            }
        }
    }

    public virtual float GetCurrentHP()
    {
        return HP.Value;
    }

    public virtual void Heal(float amount)
    {
        HP.Value = Mathf.Min(HP.Value + amount, MaxHP.Value);
    }

    public virtual void Die()
    {
        Alive = false;
        if (destroyIfDie)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
