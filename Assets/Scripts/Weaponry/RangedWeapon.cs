using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    public Transform ProjectileSource;
    public GameObject ProjectilePrefab;

    public override void Attack()
    {
        if(!ProjectilePrefab)
        {
            Debug.LogWarning(name + " is attempting to be fired but it has no ProjectilePrefab!");
            return;
        }

        if(ProjectileSource)
        {
            GameObject projectile = Instantiate(ProjectilePrefab, ProjectileSource.position, ProjectileSource.rotation);
            Projectile projScript = projectile.GetComponent<Projectile>();
            if (projScript)
            {
                projScript.InitializeProjectile();
            }
        }
    }
}
