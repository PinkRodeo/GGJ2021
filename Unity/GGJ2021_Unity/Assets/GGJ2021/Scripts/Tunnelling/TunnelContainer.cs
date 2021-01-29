using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnTunnelUpdated();

public struct TunnelPoint
{
    public TunnelPoint(Vector3 position)
    {
        this.position = position;
        this.radius = 2f;
    }

    public Vector3 position;
    public float radius;
}

public class TunnelContainer : MonoBehaviour
{
    public List<TunnelPoint> tunnelPoints = new List<TunnelPoint>();
    public OnTunnelUpdated onTunnelUpdated;


    public void AddTunnelPoint(TunnelPoint tunnelPoint)
    {
        tunnelPoints.Add(tunnelPoint);

        if (onTunnelUpdated != null)
            onTunnelUpdated();
    }

    // Update is called once per frame
    void Update()
    {

        // for (int i = 0; i < tunnelPoints.Count - 1; i++)
        // {
        //     Debug.DrawLine(tunnelPoints[i].position, tunnelPoints[i + 1].position, Color.green, Time.deltaTime);
        // }

    }
}
