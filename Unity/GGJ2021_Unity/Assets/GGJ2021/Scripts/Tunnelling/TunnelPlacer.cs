using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelPlacer : MonoBehaviour
{

    private TunnelContainer _tunnelContainer;

    public GameObject tunnelChunkPrefab;

    private List<TunnelChunk> _tunnelChunks = new List<TunnelChunk>();
    private TunnelChunk _previousTunnelChunk = null;
    private TunnelChunk _currentTunnelChunk = null;

    private static int SEGMENTS_PER_CHUNK = 10;

    public int TunnelFaceCount = 6;


    void Start()
    {
        _tunnelContainer = GetComponent<TunnelContainer>();
        if (_tunnelContainer == null)
        {
            Debug.LogError("Couldn't find a TunnelContainer to draw");
            return;
        }

        _tunnelContainer.onTunnelUpdated += OnTunnelUpdated;

        AddTunnelChunk();
    }

    private void OnTunnelUpdated(TunnelPoint newTunnelPoint, int newTunnelIndex)
    {
        if (_previousTunnelChunk != null)
        {
            _previousTunnelChunk.AddFirstPointOfNextChunk(newTunnelPoint);
            _previousTunnelChunk = null;
        }

        if (!_currentTunnelChunk.CanAddMorePoints())
        {
            _currentTunnelChunk.AddCapPoint(newTunnelPoint);
            AddTunnelChunk();
        }

        _currentTunnelChunk.AddPoint(newTunnelPoint);
    }

    private void AddTunnelChunk()
    {
        var newTunnelChunk = Instantiate(tunnelChunkPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<TunnelChunk>();

        newTunnelChunk.Initialize(TunnelFaceCount, SEGMENTS_PER_CHUNK);

        _previousTunnelChunk = _currentTunnelChunk;
        _currentTunnelChunk = newTunnelChunk;
    }
}
