using UnityEngine;


public class ShootActionController : ActionController
{
    public Transform bulletSpawnPoint;
    public Transform rotationPoint;
    public Collider2D barrelCollider;
    public float rotationSpeed = 10f; // How fast turret rotate

    private ShootAction currentAction;
    private float rotateTo;
    private Vector2 currentTarget; 
    private float nextShootTime = 0;
    private int bulletsLeft = 0;
    /// <summary>
    /// Distance from rotation point to the spawn point
    /// </summary>
    public float SpawnPointDistance { get; private set; }
    private ParticleProjectileController instance;

    private void Start()
    {
        SpawnPointDistance = Vector2.Distance(rotationPoint.position, bulletSpawnPoint.position);
    }

    private void Update()
    {
        if(active)
        {
            if ((int)rotationPoint.localRotation.eulerAngles.z != (int)rotateTo)
            {
                var rotation = rotationPoint.localRotation.eulerAngles;
                rotation.z = Mathf.LerpAngle(rotation.z,rotateTo, rotationSpeed * Time.deltaTime);
                rotationPoint.localRotation = Quaternion.Euler(rotation);
            }
            else if(nextShootTime < Time.time)
            {
                Shoot();

                if (bulletsLeft <= 0)
                {
                    active = false;
                }
            }
        }
    }

    public override void Execute(Action action,ActionTarget actionTarget)
    {
        currentAction = (ShootAction)action;
        currentTarget = actionTarget.targetLocation;
        rotateTo = rotationPoint.GetLookAtAngle(currentTarget);
        rotateTo = rotateTo < 0 ? 360 - Mathf.Abs(rotateTo) : rotateTo;
        active = true;
        nextShootTime = 0;
        bulletsLeft = currentAction.numberOfShoots;
    }

    public override bool HasFinshed()
    {

        if(active == false && instance != null && instance.IsPlaying() == false)
        {
            //Enable barrel collider
            if(barrelCollider != null) barrelCollider.enabled = true;

            Destroy(instance.gameObject);
            return true;
        }

        return false;
    }


    private void Shoot()
    {
        if (FirstShot())//Spawn Instance before first shot
        {
            //Disable barrel collider
            if (barrelCollider != null) barrelCollider.enabled = false;

            instance = Instantiate(currentAction.prefab, bulletSpawnPoint.position, rotationPoint.localRotation);
            instance.range = currentAction.range;
            instance.bulletsSpread = currentAction.bulletSpread;
            instance.bulletSpeed = 25f;
            instance.Init();
        }
        instance.Shoot();
        bulletsLeft--;
        nextShootTime = Time.time + currentAction.fireRate;
    }

    private bool FirstShot()
    {
        return currentAction != null && currentAction.numberOfShoots - bulletsLeft == 0;
    }

    public override Vector2 GetActionStartPosition(Vector2 target)
    {
        Vector2 rotPointPosition = rotationPoint.position; 
        return rotPointPosition + SpawnPointDistance * (target - rotPointPosition).normalized;
    }
}

