using UnityEngine;


public class ShootActionController : ActionController
{
    public Transform bulletSpawnPoint;
    public Transform rotationPoint;
    public float rotationSpeed = 10f; // How fast turret rotate
    public RangeShaderController display;

    private ShootActionParameters currentParams;
    private float rotateTo;
    private Vector2 currentTarget; 
    private float nextShootTime = 0;
    private int bulletLeft = 0;

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

                if (bulletLeft <= 0)
                {
                    active = false;
                }
            }
        }
    }

    public override void Execute(ActionParameters parameters,ActionTarget actionTarget)
    {
        currentParams = (ShootActionParameters)parameters;
        currentTarget = actionTarget.targetLocation;
        rotateTo = rotationPoint.GetLookAtAngle(currentTarget);
        rotateTo = rotateTo < 0 ? 360 - Mathf.Abs(rotateTo) : rotateTo;
        active = true;
        nextShootTime = 0;
        bulletLeft = currentParams.numberOfShoots;
    }

    public override bool HasFinshed()
    {
        return active == false;
    }


    private void Shoot()
    {
        var direction = Vector2.zero;
        var newShootAngle = rotationPoint.eulerAngles.z + Random.Range(0f, currentParams.bulletSpread) * (Random.value <= .5f ? -1 : 1);
        direction.x = Mathf.Cos(newShootAngle * Mathf.Deg2Rad);
        direction.y = Mathf.Sin(newShootAngle * Mathf.Deg2Rad);

        var instance = Instantiate(currentParams.bulletPrefab, bulletSpawnPoint.position, Quaternion.Euler(0,0,newShootAngle));
        instance.Shoot(direction);
        bulletLeft--;
        nextShootTime = Time.time + currentParams.fireRate;
    }
}

