using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelWorld : MonoBehaviour
{
    public static TunnelWorld instance;

    public BoundsOctree<TunnelChunk> boundsTree;

    public bool drawAllBounds = true;
    public bool drawCollisionChecks = false;

    private void Awake()
    {
        instance = this;
        boundsTree = new BoundsOctree<TunnelChunk>(50, transform.position, 5, 1.15f);
    }

    public void AddChunk(TunnelChunk tunnelChunk)
    {
        boundsTree.Add(tunnelChunk, tunnelChunk.GetBounds());
    }

    public void OnDestroy()
    {
        boundsTree = null;
        instance = null;
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (boundsTree == null)
            return;

        if (drawAllBounds == true)
            boundsTree.DrawAllBounds();

        if (drawCollisionChecks == true)
            boundsTree.DrawCollisionChecks();
    }
#endif

    public TunnelChunk[] GetNearbyChunks(Bounds bounds)
    {
        var collidingWith = new List<TunnelChunk>();

        boundsTree.GetColliding(collidingWith, bounds);

        return collidingWith.ToArray();
    }
}
