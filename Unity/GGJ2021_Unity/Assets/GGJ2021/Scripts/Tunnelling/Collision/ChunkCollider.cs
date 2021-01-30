
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

    // Start is called before the first frame update
    void Start()
    {
        // var allTunnels = GetComponents

        InvokeRepeating("CheckCollision", 2f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

    }

    void CheckCollision()
    {
        var bounds = new Bounds(transform.position, Vector3.one * (colliderSize + boundExtends));
        DebugExtension.DebugBounds(bounds, Color.blue, 0.5f, true);

        var nearbyChunks = TunnelWorld.instance.GetNearbyChunks(bounds);

        foreach (var chunk in nearbyChunks)
        {
            chunk.DoDebugDraw();
        }

    }
}
