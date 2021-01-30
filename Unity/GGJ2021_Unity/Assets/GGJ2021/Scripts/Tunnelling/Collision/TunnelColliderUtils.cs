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

    public static float FindNearestPointOnLineRatio(Vector3 start, Vector3 end, Vector3 point)
    {
        //Get heading
        Vector3 heading = (end - start);
        float magnitudeMax = heading.magnitude;
        heading.Normalize();

        //Do projection from the point but clamp it
        Vector3 lhs = point - start;
        float dotP = Vector3.Dot(lhs, heading);
        dotP = Mathf.Clamp(dotP, 0f, magnitudeMax);
        return dotP;
    }

    public static bool DoesSphereIntersectBox(Vector3 sphereCenter, float sphereRadius, Bounds box)
    {
        var boxClosestPointToSphere = box.ClosestPoint(sphereCenter);

        var distance = Mathf.Sqrt(
            Mathf.Pow(boxClosestPointToSphere.x - sphereCenter.x, 2f) +
            Mathf.Pow(boxClosestPointToSphere.y - sphereCenter.y, 2f) +
            Mathf.Pow(boxClosestPointToSphere.z - sphereCenter.z, 2f)
        );

        return distance < sphereRadius;
    }

    public static bool DoesBoxLeaveSphere(Vector3 sphereCenter, float sphereRadius, Bounds box)
    {
        // return DoesSphereIntersectBox(sphereCenter - (box.center - sphereCenter) * 2, sphereRadius, box);

        var boxClosestPointToSphere = box.ClosestPoint(sphereCenter + (box.center - sphereCenter) * 2);

        var distance = Mathf.Sqrt(
            Mathf.Pow(boxClosestPointToSphere.x - sphereCenter.x, 2f) +
            Mathf.Pow(boxClosestPointToSphere.y - sphereCenter.y, 2f) +
            Mathf.Pow(boxClosestPointToSphere.z - sphereCenter.z, 2f)
        );

        return distance > sphereRadius;
    }

    //     function intersect(sphere, box) {
    //   // get box closest point to sphere center by clamping
    //   var x = Math.max(box.minX, Math.min(sphere.x, box.maxX));
    //   var y = Math.max(box.minY, Math.min(sphere.y, box.maxY));
    //   var z = Math.max(box.minZ, Math.min(sphere.z, box.maxZ));

    //   // this is the same as isPointInsideSphere
    //   var distance = Math.sqrt((x - sphere.x) * (x - sphere.x) +
    //                            (y - sphere.y) * (y - sphere.y) +
    //                            (z - sphere.z) * (z - sphere.z));

    //   return distance < sphere.radius;
    // }


}
