using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected bool _initialized = false;
    public float Speed = 3.0f;
    public float LifeTime = 7.0f;
    public int Damage = 45;
    public LayerMask hittableLayers = Physics.AllLayers;

    public virtual void InitializeProjectile()
    {
        _initialized = true;
        Destroy(gameObject, LifeTime);
    }

    protected virtual void FixedUpdate()
    {
        if(_initialized)
        {
            float distanceTravelledThisUpdate = Speed * Time.fixedDeltaTime;

            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, distanceTravelledThisUpdate, hittableLayers, QueryTriggerInteraction.Ignore))
            {
                DamageReciever dr = hit.transform.GetComponent<DamageReciever>();
                if(dr)
                {
                    dr.ApplyDamage(Damage);
                }
                transform.position = hit.point;
                Destroy(gameObject);
            }
            else
            {
                transform.position += transform.forward * distanceTravelledThisUpdate;
            }
        }
    }
}
