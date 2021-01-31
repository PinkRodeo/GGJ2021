
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
                if (chunk.nextChunk == null)
                    continue;

                // if (chunk.nextChunk.nextChunk == null)
                //     continue;
                // Don't try to collide with own tunnel
                // continue;
            }

            Vector3 collisionPosition;
            float radiusAtClosestPoint;

            // keeping outside tunnel
            if (TunnelColliderUtils.IsEnteringTunnel(chunk, CollisionRate, center, colliderBounds, out collisionPosition, out radiusAtClosestPoint))
            {
                DebugExtension.DebugCapsule(collisionPosition, collisionPosition + Vector3.one * 0.01f, Color.red, radiusAtClosestPoint, CollisionRate * 10, false);

                // TODO: Push away player from this point

                var rigidBody = GetComponent<Rigidbody>();

                if (rigidBody == null)
                {
                    Debug.LogError("Added a collider on something without a rigidbody!");
                    continue;
                }

                rigidBody.AddForce((collisionPosition - center).normalized * -1.5f, ForceMode.Impulse);
            }
            else
            {
                DebugExtension.DebugCapsule(collisionPosition, collisionPosition + Vector3.one * 0.01f, Color.green, radiusAtClosestPoint, CollisionRate, false);

            }
        }
    }
}
