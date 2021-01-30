using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TunnelColliderUtils
{

    // public TunnelChu

    // public static TunnelChunk[] GetClosestChunks()
    // {

    // }

    public static Vector3 FindNearestPointOnLine(Vector3 start, Vector3 end, Vector3 point)
    {
        //Get heading
        Vector3 heading = (end - start);
        float magnitudeMax = heading.magnitude;
        heading.Normalize();

        //Do projection from the point but clamp it
        Vector3 lhs = point - start;
        float dotP = Vector3.Dot(lhs, heading);
        dotP = Mathf.Clamp(dotP, 0f, magnitudeMax);
        return start + heading * dotP;
    }


}
