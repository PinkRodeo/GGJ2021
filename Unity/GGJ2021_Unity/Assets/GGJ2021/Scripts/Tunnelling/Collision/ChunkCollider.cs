
#if UNITY_EDITOR
#define DEBUGDRAW
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnCollideWithTunnel(TunnelChunk collider);

public class ChunkCollider : MonoBehaviour
{
    // public List<TunnelData> activeTunnels = new List<TunnelData>();

    public OnCollideWithTunnel onCollideWithTunnel;

    public float colliderSize = 2f;

    public float boundExtends = 0.1f;

    public float CollisionRate = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        // var allTunnels = GetComponents

        InvokeRepeating("CheckCollision", CollisionRate, CollisionRate);
    }

    public void CheckCollisionTimerHack()
    {
        CheckCollision();
        Invoke("CheckCollision", CollisionRate);
    }

    void CheckCollision()
    {
        var center = transform.position;

        var colliderBounds = new Bounds(center, Vector3.one * (colliderSize));
        var colliderBoundsExtended = new Bounds(center, Vector3.one * (colliderSize + boundExtends));

        DebugExtension.DebugBounds(colliderBounds, Color.blue, CollisionRate, true);

        var nearbyChunks = TunnelWorld.instance.GetNearbyChunks(colliderBoundsExtended);

        foreach (var chunk in nearbyChunks)
        {
            if (chunk.parent.tunnelData.tunnelTracer.gameObject == gameObject)
            {
                Debug.Log("Didn't collide with own tunnel!");
                // Don't try to collide with own tunnel
                continue;
            }

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
                continue;
            }

            var start = tunnelPoints[closestStartIndex];
            var end = tunnelPoints[closestStartIndex + 1];

            var ratio = TunnelColliderUtils.FindNearestPointOnLineRatio(start.position, end.position, center);
            var position = TunnelColliderUtils.FindNearestPointOnLine(start.position, end.position, center);

            var radiusAtClosestPoint = Mathf.Lerp(start.radius, end.radius, ratio);

            // For pushing out of tunnel

            // var isOverlap = TunnelColliderUtils.DoesSphereIntersectBox(position, radiusAtClosestPoint, colliderBounds);

            // if (isOverlap)
            // {
            //     DebugExtension.DebugCapsule(position, position + Vector3.one * 0.01f, Color.red, radiusAtClosestPoint, CollisionRate * 10, false);

            //     // TODO: Push away player from this point

            //     var rigidBody = GetComponent<Rigidbody>();

            //     if (rigidBody == null)
            //     {
            //         Debug.LogError("Added a collider on something without a rigidbody!");
            //         continue;
            //     }

            //     // rigidBody.AddForce((center - position).normalized * 10f, ForceMode.Impulse);
            //     rigidBody.AddForce((position - center).normalized * 10f, ForceMode.Impulse);
            //     //    rigidBody.AddTorque()
            // }
            // else
            // {
            //     DebugExtension.DebugCapsule(position, position + Vector3.one * 0.01f, Color.green, radiusAtClosestPoint, CollisionRate, false);
            // }

            // For keeping inside tunnel

            var isExiting = TunnelColliderUtils.DoesBoxLeaveSphere(position, radiusAtClosestPoint, colliderBounds);

            if (isExiting)
            {
                DebugExtension.DebugCapsule(position, position + Vector3.one * 0.01f, Color.red, radiusAtClosestPoint, CollisionRate * 10, false);

                // TODO: Push away player from this point

                var rigidBody = GetComponent<Rigidbody>();

                if (rigidBody == null)
                {
                    Debug.LogError("Added a collider on something without a rigidbody!");
                    continue;
                }

                // rigidBody.AddForce((center - position).normalized * 10f, ForceMode.Impulse);

                var pushAway = (position - center).normalized * 2f;

                DebugExtension.DebugArrow(position, pushAway, Color.black, 1f);
                rigidBody.AddForce(pushAway, ForceMode.Impulse);
                //    rigidBody.AddTorque()
            }
            else
            {
                DebugExtension.DebugCapsule(position, position + Vector3.one * 0.01f, Color.green, radiusAtClosestPoint, CollisionRate, false);
            }

        }
    }
    // Vector3.cl

    // bounds.ClosestPoint

}
