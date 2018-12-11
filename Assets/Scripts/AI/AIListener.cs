using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIListener : MonoBehaviour
{
    public AIBrain MyBrain;

    public void Alert(Vector3 point, int priority)
    {
        if(MyBrain)
        {
            MyBrain.Alert(point, priority);
        }
    }
}
