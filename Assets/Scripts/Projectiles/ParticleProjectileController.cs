using UnityEngine;

public class ParticleProjectileController : MonoBehaviour
{
    public ParticleSystem projectile;
    public float bulletsSpread = 0f;
    public int bulletsPerShot = 1;
    public float range = 20f;
    public float bulletSpeed = 25f;


    private void Start()
    {
        Init();
    }

    public void Init()
    {
        var shape = projectile.shape;
        shape.arc = bulletsSpread * 2;
        shape.rotation = new Vector3(0, 0, -bulletsSpread);
        var main = projectile.main;
        var startSpeed = main.startSpeed;
        startSpeed.mode = ParticleSystemCurveMode.Constant;
        startSpeed.constant = bulletSpeed;
        main.startSpeed = startSpeed;
        main.startLifetime = range / bulletSpeed;
    }

    [ContextMenu("Shoot")]
    public void Shoot()
    {
        projectile.Emit(bulletsPerShot);
    }

    public bool IsPlaying()
    {
        if (projectile.isPlaying)
        {
            return true;
        }

        for (int i = 0; i < projectile.subEmitters.subEmittersCount; i++)
        {
            if (projectile.subEmitters.GetSubEmitterSystem(i).isPlaying)
            {
                return true;
            }
        }

        return false;
    }
}
