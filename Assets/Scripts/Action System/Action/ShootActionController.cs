using UnityEngine;


public class ShootActionController : ActionController
{
    public Bullet bulletPrefab;
    public Transform bulletSpawnPoint;
    public Transform rotationPoint;
    public float rotationSpeed = 10f;
    private Bullet instance;
    private float rotateTo;
    private Vector2 currentTarget;

    private void Update()
    {
        if(active)
        {
            var rotation = rotationPoint.localRotation.eulerAngles;
            rotation.z = Mathf.LerpAngle(rotation.z,rotateTo, rotationSpeed * Time.deltaTime);
            rotationPoint.localRotation = Quaternion.Euler(rotation);
            if((int)rotation.z == (int)rotateTo)
            {
                Shoot();
                active = false;
            }
        }
    }

    public override void Execute(ActionParameters parameters)
    {
        var currentParameters = (ShootActionParameters)parameters;
        currentTarget = currentParameters.target;
        rotateTo = rotationPoint.GetLookAtAngle(currentTarget);
        rotateTo = rotateTo < 0 ? 360 - Mathf.Abs(rotateTo) : rotateTo;
        active = true;
    }

    public override bool HasFinshed()
    {
        return active == false && instance == null;
    }


    private void Shoot()
    {
        var direction = (currentTarget - (Vector2)bulletSpawnPoint.position).normalized;
        instance = Instantiate(bulletPrefab, bulletSpawnPoint.position, rotationPoint.rotation);
        instance.Shoot(direction);
    }
}

