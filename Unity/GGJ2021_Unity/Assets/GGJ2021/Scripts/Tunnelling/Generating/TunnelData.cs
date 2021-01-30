using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnTunnelUpdated(TunnelPoint newTunnelPoint, int newTunnelIndex);

public struct TunnelPoint
{
    public TunnelPoint(Vector3 position)
    {
        this.position = position;
        this.radius = 4f;
    }

    public Vector3 position;
    public float radius;
}
/// <summary>
/// Storage for the raw data of tunnels
/// </summary>
public class TunnelData : MonoBehaviour
{
    // [UnityEngine.SerializeField]
    public List<TunnelPoint> tunnelPoints = new List<TunnelPoint>();
    public OnTunnelUpdated onTunnelUpdated;

    public TunnelTracer tunnelTracer
    {
        get
        {
            return _tunnelTracer;
        }
    }

    private TunnelTracer _tunnelTracer;

    public void StartTunnel(TunnelTracer activeTunnelTracer)
    {
        _tunnelTracer = activeTunnelTracer;
    }

    public void AddTunnelPoint(TunnelPoint tunnelPoint)
    {
        tunnelPoints.Add(tunnelPoint);

        if (onTunnelUpdated != null)
            onTunnelUpdated(tunnelPoint, tunnelPoints.Count);
    }

    public void FinishTunnel()
    {
        _tunnelTracer = null;
    }
}
