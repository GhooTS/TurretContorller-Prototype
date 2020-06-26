﻿using UnityEngine;


public class ShootActionController : ActionController
{
    public Transform bulletSpawnPoint;
    public Transform rotationPoint;
    public float rotationSpeed = 10f; // How fast turret rotate

    private ShootAction currentAction;
    private float rotateTo;
    private Vector2 currentTarget; 
    private float nextShootTime = 0;
    private int bulletsLeft = 0;

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
        return active == false;
    }


    private void Shoot()
    {
        var direction = (currentTarget - (Vector2)rotationPoint.position).normalized;
        Quaternion bulletRotation = rotationPoint.rotation;

        if (currentAction.shotType == ShootAction.ShotType.MultiShot)
        {
            var newShootAngle = rotationPoint.eulerAngles.z + Random.Range(0f, currentAction.bulletSpread) * (Random.value <= .5f ? -1 : 1);
            direction.x = Mathf.Cos(newShootAngle * Mathf.Deg2Rad);
            direction.y = Mathf.Sin(newShootAngle * Mathf.Deg2Rad);
            bulletRotation = Quaternion.Euler(0, 0, newShootAngle);
        }
        

        var instance = Instantiate(currentAction.bulletPrefab, bulletSpawnPoint.position, bulletRotation);
        instance.Shoot(direction);
        bulletsLeft--;
        nextShootTime = Time.time + currentAction.fireRate;
    }
}
