using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelTracer : MonoBehaviour
{
    public float segmentLength = 2f;

    public float segmentRadius = 5f;
    public TunnelData targetTunnelData;

    private bool _isTracing = false;

    private Vector3 _previousSamplePosition;

    // Used by a tunnel that's still being generated to place tunnel pieces relative to the player
    // public static TunnelTracer activeTunnelTracer;

    private Transform _transform;

    private Rigidbody _rigidBody;

    private Vector3 _previousPosition;
    public Vector3 currentDirection = Vector3.forward;

    private void Awake()
    {
        _transform = transform;
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void OnDestroy()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        // TODO: Should be activated by game manager
        StartTracing();
    }

    public void StartTracing()
    {
        if (targetTunnelData == null)
        {
            Debug.LogError("Cannot start tracing tunnel if no tunnel data is provided!");
            // TODO: Create new tunnel if none is available
            return;
        }

        targetTunnelData.StartTunnel(this);
        _isTracing = true;

        AddTrace();
    }

    public void StopTracing()
    {
        _isTracing = false;

        targetTunnelData.FinishTunnel();
    }

    private void Update()
    {
        if (_rigidBody != null)
        {
            currentDirection = _rigidBody.velocity.normalized;
        }
        else
        {
            var currentPosition = transform.position;
            currentDirection = (currentPosition - _previousPosition).normalized;
        }
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

        // FOr test purposes

    }

    private void AddTrace()
    {
        _previousSamplePosition = _transform.position;
        targetTunnelData.AddTunnelPoint(new TunnelPoint(_previousSamplePosition, segmentRadius));
    }
}
