using UnityEngine;

[CreateAssetMenu(menuName = "Action System/Action/Shoot Action")]
public class ShootAction : Action
{
    public enum ShotType
    {
        Precise,
        MultiShot,
        Expolosion
    }

    public ShotType shotType;
    public Bullet bulletPrefab;
    public float fireRate = 0.2f; // How fast turret shoots
    public float bulletSpread = 12f; // in degrees
    public int numberOfShoots = 3; // How much bullets turret shoot in one action

    private void OnEnable()
    {
        Type = ActionType.Shoot;
    }
}

