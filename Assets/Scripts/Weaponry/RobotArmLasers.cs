using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArmLasers : RangedWeapon
{
    public Transform leftArmPivot;
    public Transform rightArmPivot;
    public Transform leftProjectileSource;
    public Transform rightProjectileSource;
    public float laserCoolDown = 1.0f;
    public float laserAlternatingTime = 0.2f;

    protected bool _leftLaserOnCoolDown = false;
    protected bool _rightLaserOnCoolDown = false;
    protected float _leftCoolDownRemaining;
    protected float _rightCoolDownRemaining;
    protected Quaternion _leftArmInitialRotation;
    protected Quaternion _rightArmInitialRotation;

    private void Start()
    {
        if(!ProjectileSource)
        {
            ProjectileSource = leftProjectileSource;
        }

        if(leftArmPivot)
        {
            _leftArmInitialRotation = leftArmPivot.localRotation;
        }
        if(rightArmPivot)
        {
            _rightArmInitialRotation = rightArmPivot.localRotation;
        }
    }

    private void FixedUpdate()
    {
        if(_leftLaserOnCoolDown)
        {
            if(_leftCoolDownRemaining > 0.0f)
            {
                _leftCoolDownRemaining -= Time.fixedDeltaTime;
            }
            else
            {
                _leftLaserOnCoolDown = false;
            }
        }

        if (_rightLaserOnCoolDown)
        {
            if (_rightCoolDownRemaining > 0.0f)
            {
                _rightCoolDownRemaining -= Time.fixedDeltaTime;
            }
            else
            {
                _rightLaserOnCoolDown = false;
            }
        }
    }

    public void ResetAiming()
    {
        if(leftArmPivot)
        {
            leftArmPivot.rotation = _leftArmInitialRotation;
        }
        if(rightArmPivot)
        {
            rightArmPivot.rotation = _rightArmInitialRotation;
        }
    }

    public void AimAtPoint(Vector3 point)
    {
        //I know currently this is going to make them aim incorrectly.
        //Yes I should fix this.
        if(leftArmPivot)
        {
            leftArmPivot.forward = point - leftArmPivot.position;
        }
        if(rightArmPivot)
        {
            rightArmPivot.forward = point - rightArmPivot.position;
        }
    }

    public override void Attack()
    {
        if(ProjectileSource == leftProjectileSource && !_leftLaserOnCoolDown)
        {
            base.Attack();

            _rightLaserOnCoolDown = true;
            _rightCoolDownRemaining = laserAlternatingTime;
            ProjectileSource = rightProjectileSource;
            _leftLaserOnCoolDown = true;
            _leftCoolDownRemaining = laserCoolDown;
        }
        else if(ProjectileSource == rightProjectileSource && !_rightLaserOnCoolDown)
        {
            base.Attack();

            _leftLaserOnCoolDown = true;
            _leftCoolDownRemaining = laserAlternatingTime;
            ProjectileSource = leftProjectileSource;
            _rightLaserOnCoolDown = true;
            _leftCoolDownRemaining = laserCoolDown;
        }
    }
}
