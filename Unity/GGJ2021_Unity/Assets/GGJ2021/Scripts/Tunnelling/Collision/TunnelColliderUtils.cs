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


    public static int GetClosestSegmentStartIndex(ref TunnelPoint[] tunnelPoints, int maxIndex, Vector3 center, float CollisionRate)
    {
        float shortestDistanceToCenter = float.MaxValue;
        int closestStartIndex = -1;

        // TODO: extend tunnelpoints with those from neighbor chunks or use ChunkData

        for (int i = 0; i < maxIndex - 1; i++)
        {
            var start = tunnelPoints[i];
            var end = tunnelPoints[i + 1];

            Debug.DrawLine(start.position, end.position, Color.yellow, CollisionRate);

            var nearestPoint = TunnelColliderUtils.FindNearestPointOnLine(start.position, end.position, center);

            var distanceToCenter = Vector3.SqrMagnitude(nearestPoint - center);

            if (distanceToCenter < shortestDistanceToCenter)
            {
                shortestDistanceToCenter = distanceToCenter;
                closestStartIndex = i;
            }
        }

        return closestStartIndex;
    }

    public static bool IsExitingTunnel(TunnelChunk chunk, float CollisionRate, Vector3 center, Bounds colliderBounds, out Vector3 collisionPosition, out float radiusAtClosestPoint)
    {
        var isExiting = IsExitingTunnelChunk(chunk, CollisionRate, center, colliderBounds, out collisionPosition, out radiusAtClosestPoint);

        if (isExiting)
        {
            if (chunk.previousChunk != null)
            {
                if (!IsExitingTunnelChunk(chunk.previousChunk, CollisionRate, center, colliderBounds, out collisionPosition, out radiusAtClosestPoint))
                {
                    return false;
                }
            }

            if (chunk.nextChunk != null)
            {
                if (!IsExitingTunnelChunk(chunk.nextChunk, CollisionRate, center, colliderBounds, out collisionPosition, out radiusAtClosestPoint))
                {
                    return false;
                }
            }
        }

        return isExiting;
    }

    private static bool IsExitingTunnelChunk(TunnelChunk chunk, float CollisionRate, Vector3 center, Bounds colliderBounds, out Vector3 collisionPosition, out float radiusAtClosestPoint)
    {
        chunk.DoDebugDraw(CollisionRate);

        var tunnelPoints = chunk.tunnelPoints;
        var maxIndex = chunk.currentLengthSegment - 1;

        Debug.DrawLine(
            tunnelPoints[0].position,
            tunnelPoints[maxIndex].position,
            Color.red, 0.5f);

        var closestStartIndex = TunnelColliderUtils.GetClosestSegmentStartIndex(ref tunnelPoints, maxIndex, center, CollisionRate);

        if (closestStartIndex == -1)
        {
            Debug.LogWarning("No closest?");
            collisionPosition = Vector3.zero;
            radiusAtClosestPoint = 0f;
            return false;
        }

        var start = tunnelPoints[closestStartIndex];
        var end = tunnelPoints[closestStartIndex + 1];

        var ratio = TunnelColliderUtils.FindNearestPointOnLineRatio(start.position, end.position, center);
        collisionPosition = TunnelColliderUtils.FindNearestPointOnLine(start.position, end.position, center);
        radiusAtClosestPoint = Mathf.Lerp(start.radius, end.radius, ratio);

        return TunnelColliderUtils.DoesBoxLeaveSphere(collisionPosition, radiusAtClosestPoint, colliderBounds);
    }


    public static bool IsEnteringTunnel(TunnelChunk chunk, float CollisionRate, Vector3 center, Bounds colliderBounds, out Vector3 collisionPosition, out float radiusAtClosestPoint)
    {
        chunk.DoDebugDraw(CollisionRate);

        var tunnelPoints = chunk.tunnelPoints;
        var maxIndex = chunk.currentLengthSegment - 1;

        Debug.DrawLine(
            tunnelPoints[0].position,
            tunnelPoints[maxIndex].position,
            Color.red, 0.5f);

        var closestStartIndex = TunnelColliderUtils.GetClosestSegmentStartIndex(ref tunnelPoints, maxIndex, center, CollisionRate);

        if (closestStartIndex == -1)
        {
            Debug.LogWarning("No closest?");
            collisionPosition = Vector3.zero;
            radiusAtClosestPoint = 0f;
            return false;
        }

        var start = tunnelPoints[closestStartIndex];
        var end = tunnelPoints[closestStartIndex + 1];

        var ratio = TunnelColliderUtils.FindNearestPointOnLineRatio(start.position, end.position, center);
        collisionPosition = TunnelColliderUtils.FindNearestPointOnLine(start.position, end.position, center);
        radiusAtClosestPoint = Mathf.Lerp(start.radius, end.radius, ratio);


        return TunnelColliderUtils.DoesSphereIntersectBox(collisionPosition, radiusAtClosestPoint, colliderBounds);
    }

}
