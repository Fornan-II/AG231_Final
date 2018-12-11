using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class AlertBrainToSound : MonoBehaviour
{
    private static float _velocityLowerThreshold = 1.0f;

    private void OnCollisionEnter(Collision collision)
    {
        float relVelSquared = collision.relativeVelocity.sqrMagnitude;
        if(relVelSquared >= _velocityLowerThreshold * _velocityLowerThreshold)
        {
            relVelSquared /= 3.0f;

            Debug.Log("Alerting brains within " + relVelSquared + " units");

            Collider[] nearby = Physics.OverlapSphere(transform.position, relVelSquared);
            foreach(Collider c in nearby)
            {
                AIListener ears = c.GetComponent<AIListener>();
                if(ears)
                {
                    ears.Alert(transform.position, 0);
                    Debug.Log("Alerting " + ears.name);
                }
            }
        }
    }
}
