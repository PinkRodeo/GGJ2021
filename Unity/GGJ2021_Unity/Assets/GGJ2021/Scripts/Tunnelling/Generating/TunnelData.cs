using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnTunnelUpdated(TunnelPoint newTunnelPoint, int newTunnelIndex);

public struct TunnelPoint
{
    public TunnelPoint(Vector3 position, float radius)
    {
        this.position = position;
        this.radius = radius;
    }

    public Vector3 position;
    public float radius;
}
/// <summary>
/// Storage for the raw data of tunnels
/// </summary>
public class TunnelData : MonoBehaviour
{
#if UNITY_WEBGL
    public static int MAX_TUNNEL_POINTS = 500;
#else
    public static int MAX_TUNNEL_POINTS = 10000;
#endif
    // [UnityEngine.SerializeField]
    public TunnelPoint[] tunnelPoints = new TunnelPoint[0];
    public OnTunnelUpdated onTunnelUpdated;

    private int _currentTunnelIndex = 0;

    private bool _reachedMaxSize = false;

    private bool _isDrawing = false;

    public bool isDrawing
    {
        get
        {
            return _isDrawing;
        }
    }


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
        tunnelPoints = new TunnelPoint[MAX_TUNNEL_POINTS];
        _currentTunnelIndex = 0;

        _isDrawing = true;
    }

    public void AddTunnelPoint(TunnelPoint tunnelPoint)
    {
        if (_currentTunnelIndex + 1 > MAX_TUNNEL_POINTS)
        {
            if (_reachedMaxSize == false)
            {
                _reachedMaxSize = true;
                Debug.LogError("Reached max length of a tunnel!");
            }
            return;
        }

        tunnelPoints[_currentTunnelIndex] = tunnelPoint;
        _currentTunnelIndex += 1;

        if (onTunnelUpdated != null)
            onTunnelUpdated(tunnelPoint, _currentTunnelIndex - 1);
    }

    public void FinishTunnel()
    {
        _tunnelTracer = null;
        _isDrawing = false;
    }
}
