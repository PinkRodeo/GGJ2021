using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelTracer : MonoBehaviour
{
    public float segmentLength = 2f;
    public TunnelContainer currentTunnelContainer;

    private bool _isTracing = false;

    private Vector3 _previousSamplePosition;


    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }


    // Start is called before the first frame update
    void Start()
    {
        // TODO: Should be activated by game manager
        StartTracing();
    }

    public void StartTracing()
    {
        if (currentTunnelContainer == null)
        {
            Debug.LogError("Cannot start tracing tunnel if no tunnel container is provided!");
            return;
        }

        _isTracing = true;

        AddTrace();
    }

    public void StopTracing()
    {
        _isTracing = false;

        // TODO: call a finishup function on the TunnelContainer
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!_isTracing)
        {
            // Not active
            return;
        }

        var distanceSinceLastPoint = Vector3.Distance(_previousSamplePosition, _transform.position);

        if (distanceSinceLastPoint > segmentLength)
        {
            AddTrace();
        }
    }

    private void AddTrace()
    {
        _previousSamplePosition = _transform.position;
        currentTunnelContainer.AddTunnelPoint(new TunnelPoint(_previousSamplePosition));
    }
}
