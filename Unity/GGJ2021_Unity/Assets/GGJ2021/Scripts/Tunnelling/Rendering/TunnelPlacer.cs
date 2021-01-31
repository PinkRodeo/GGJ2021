using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelPlacer : MonoBehaviour
{
    private TunnelData _tunnelData;

    public TunnelData tunnelData
    {
        get
        {
            return _tunnelData;
        }
    }

    public GameObject tunnelChunkPrefab;

    private List<TunnelChunk> _tunnelChunks = new List<TunnelChunk>();
    private TunnelChunk _previousTunnelChunk = null;
    private TunnelChunk _currentTunnelChunk = null;


    // TODO: tweak this number together with the cost of finding chunks to collide with
    private static int SEGMENTS_PER_CHUNK = 40;

    public int TunnelFaceCount = 6;

    void Start()
    {
        _tunnelData = GetComponent<TunnelData>();
        if (_tunnelData == null)
        {
            Debug.LogError("Couldn't find a TunnelData to draw");
            return;
        }

        _tunnelData.onTunnelUpdated += OnTunnelUpdated;

        AddTunnelChunk();
    }

    private void OnTunnelUpdated(TunnelPoint newTunnelPoint, int newTunnelIndex)
    {
        if (_previousTunnelChunk != null)
        {
            _previousTunnelChunk.AddFirstPointOfNextChunk(newTunnelPoint);
            _previousTunnelChunk.FinishChunk();
            // Destroy(_previousTunnelChunk);
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

        newTunnelChunk.Initialize(this, TunnelFaceCount, SEGMENTS_PER_CHUNK);

        _previousTunnelChunk = _currentTunnelChunk;
        if (_previousTunnelChunk != null)
        {
            _previousTunnelChunk.nextChunk = newTunnelChunk;
        }

        _currentTunnelChunk = newTunnelChunk;
        _currentTunnelChunk.previousChunk = _previousTunnelChunk;
    }
}
