using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugUtil
{
    public static void DrawPath(Vector3[] pathPoints)
    {
        DrawPath(pathPoints, Color.white);
    }

    public static void DrawPath(Vector3[] pathPoints, Color lineColor)
    {
        if(pathPoints.Length < 1) { return; }

        Vector3 prevPoint = pathPoints[0];
        for (int i = 1; i < pathPoints.Length; i++)
        {
            Debug.DrawLine(prevPoint, pathPoints[i], lineColor);
            prevPoint = pathPoints[i];
        }
    }

    public static void DrawPath(Vector3[] pathPoints, Color lineColor, float duration)
    {
        if (pathPoints.Length < 1) { return; }

        Vector3 prevPoint = pathPoints[0];
        for (int i = 1; i < pathPoints.Length; i++)
        {
            Debug.DrawLine(prevPoint, pathPoints[i], lineColor, duration);
            prevPoint = pathPoints[i];
        }
    }
}
