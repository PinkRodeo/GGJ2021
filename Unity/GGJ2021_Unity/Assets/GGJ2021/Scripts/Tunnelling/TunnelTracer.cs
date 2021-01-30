using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelTracer : MonoBehaviour
{
    public float segmentLength = 2f;
    public TunnelContainer currentTunnelContainer;

    private bool _isTracing = false;

    private Vector3 _previousSamplePosition;

    public static TunnelTracer activeTunnelTracer;

    private Transform _transform;


    private Vector3 _previousPosition;
    public Vector3 currentDirection = Vector3.forward;

    private void Awake()
    {
        activeTunnelTracer = this;
        _transform = transform;
    }

    private void OnDestroy()
    {
        activeTunnelTracer = null;
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

        var currentPosition = transform.position;

        currentDirection = (currentPosition - _previousPosition).normalized;

    }

    private void AddTrace()
    {
        _previousSamplePosition = _transform.position;
        currentTunnelContainer.AddTunnelPoint(new TunnelPoint(_previousSamplePosition));
    }
}
