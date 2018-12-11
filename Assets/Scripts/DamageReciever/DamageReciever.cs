using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReciever : MonoBehaviour
{
    public DamageReciever MasterDamageReciever;
    public int Health;
    public int MaxHealth;

    protected bool _isAlive = true;
    
    public virtual void ApplyDamage(int damage)
    {
        if(MasterDamageReciever)
        {
            MasterDamageReciever.ApplyDamage(damage);
        }
        else
        {
            Health -= damage;
            if(Health <= 0)
            {
                Die();
            }
        }
    }

    public virtual void Die()
    {
        if (!_isAlive) { return; };

        _isAlive = false;

        if (MasterDamageReciever)
        {
            MasterDamageReciever.Die();
            return;
        }
    }
}
